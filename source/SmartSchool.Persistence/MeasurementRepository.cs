using Microsoft.EntityFrameworkCore;
using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.Persistence
{
  public class MeasurementRepository : IMeasurementRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public MeasurementRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    /// <summary>
    /// Liefert alle Measurements zu einem übergebenen Sensor sortiert nach Zeit
    /// </summary>
    /// <param name="sensorId"></param>
    /// <returns></returns>
    public async Task<Measurement[]> GetAllBySensorIdAsync(int sensorId) =>
      await _dbContext.Measurements
        .Where(m => m.SensorId == sensorId)
        .OrderBy(m => m.Time)
        .ToArrayAsync();

    /// <summary>
    /// Liefert den Messwert mit der übergebenen Id (null wenn nicht gefunden)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Measurement> GetByIdAsync(int id) =>
      await _dbContext.Measurements
        .Include(m => m.Sensor)
        .SingleOrDefaultAsync(m => m.Id == id);

    /// <summary>
    /// Liefert die Anzahl aller Messwerte in der Datenbank
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetCountAsync() =>
      await _dbContext.Measurements
        .CountAsync();

    public async Task AddRangeAsync(Measurement[] measurements) =>
      await _dbContext.Measurements
        .AddRangeAsync(measurements);

    public async Task<Measurement[]> GetAllAsync() =>
      await _dbContext.Measurements
        .OrderBy(m => m.Time)
        .ToArrayAsync();

    public async Task AddAsync(Measurement measurement) =>
      await _dbContext.Measurements
        .AddAsync(measurement);

    public async Task<Measurement[]> GetLast20BySensorAsync(int sensorId) =>
      await _dbContext
        .Measurements
        .Where(m => m.SensorId == sensorId)
        .OrderByDescending(m => m.Time)
        .Take(20)
        .ToArrayAsync();

    public async Task<Measurement> GetBySensorAndDateTimeAsync(int sensorId, DateTime dateTime) =>
      await _dbContext.Measurements
        .Include(m => m.Sensor)
        .Where(m => m.SensorId == sensorId && m.Time <= dateTime)
        .OrderByDescending(m => m.Time)
        .FirstOrDefaultAsync();

    public async Task<long> GetCountOfMeasurementsAsync(string name, string location) =>
      await _dbContext.Measurements
        .LongCountAsync(m => m.Sensor.Name == name && m.Sensor.Location == location);

    public async Task<Measurement[]> GetLast3GreatestMeasurementsAsync(string name, string location) =>
      await _dbContext.Measurements
        .Where(m => m.Sensor.Name == name && m.Sensor.Location == location)
        .OrderByDescending(m => m.Value)
        .ThenByDescending(m => m.Time)
        .Take(3)
        .ToArrayAsync();

    public async Task<double> GetAverageOfValidCo2Async(string location) =>
      await _dbContext.Measurements
        .Where(m => m.Sensor.Name == "co2" && m.Sensor.Location == location
                                           && m.Value > 300 && m.Value < 5000)
        .AverageAsync(m => m.Value);
  }
}