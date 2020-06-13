using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Core.Entities;
using SmartSchool.Core.Contracts;

namespace SmartSchool.Persistence
{
  public class SensorRepository : ISensorRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public SensorRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    /// <summary>
    /// Liefert eine Liste aller Sensoren sortiert nach dem SensorName
    /// </summary>
    /// <returns></returns>
    public async Task<Sensor[]> GetAllAsync() =>
      await _dbContext.Sensors
        .OrderBy(sensor => sensor.Name)
        .ToArrayAsync();

    public async Task<(Sensor Sensor, int Count)[]> GetAllWithMeasurementsCounterAsync()
    {
      var groupedMeasurements = await _dbContext.Measurements
          .GroupBy(m => m.SensorId)
          .Select(group => new { SensorId = group.Key, Count = group.Count() })
          .ToArrayAsync();
      var sensors = await _dbContext.Sensors.ToArrayAsync();
      var result = sensors
          .OrderBy(sensor => sensor.Name)
          .ThenBy(s => s.Location)
          .Select(s =>
          (
              s,
              groupedMeasurements.Any(gm => gm.SensorId == s.Id)
                  ? groupedMeasurements.Single(gm => gm.SensorId == s.Id).Count
                  : 0
          )).ToArray();
      return result;
    }

    /// <summary>
    /// Liefert den Sensor mit der übergebenen Id --> null wenn nicht gefunden
    /// </summary>
    /// <param name="sensorId"></param>
    /// <returns></returns>
    public async Task<Sensor> GetByIdAsync(int sensorId) =>
      await _dbContext.Sensors
        .SingleOrDefaultAsync(c => c.Id == sensorId);

    public void Remove(Sensor sensor)
    {
      _dbContext.Sensors.Remove(sensor);
    }

    public async Task<(Sensor Sensor, Double Average)[]> GetSensorsWithAverageValuesAsync()
    {
      var groupedMeasurements = await _dbContext.Measurements
          .GroupBy(m => m.SensorId)
          .Select(group => new { SensorId = group.Key, Average = group.Average(m => m.Value) })
          .ToArrayAsync();
      var sensors = await _dbContext.Sensors.ToArrayAsync();
      return sensors
          .OrderBy(sensor => sensor.Location)
          .ThenBy(s => s.Name)
          .Select(s => (s, groupedMeasurements.Any(gm => gm.SensorId == s.Id) ? groupedMeasurements.Single(gm => gm.SensorId == s.Id).Average : 0))
          .ToArray();
    }


    /// <summary>
    /// Neue Kategorie wird in Datenbank eingefügt
    /// </summary>
    /// <param name="sensor"></param>
    public async Task InsertAsync(Sensor sensor) =>
      await _dbContext.Sensors
        .AddAsync(sensor);

    public void Update(Sensor sensor) =>
      _dbContext.Sensors.Update(sensor);

    public async Task<bool> HasDuplicateAsync(Sensor sensor) =>
      await _dbContext
        .Sensors
        .AnyAsync(m => m.Id != sensor.Id && m.Name == sensor.Name && m.Location == sensor.Location);
  }
}