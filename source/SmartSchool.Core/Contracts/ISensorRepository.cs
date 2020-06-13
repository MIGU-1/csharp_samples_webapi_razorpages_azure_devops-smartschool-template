using System;
using System.Threading.Tasks;
using SmartSchool.Core.Entities;

namespace SmartSchool.Core.Contracts
{
  public interface ISensorRepository
  {
    Task<Sensor[]> GetAllAsync();
    Task<(Sensor Sensor, int Count)[]> GetAllWithMeasurementsCounterAsync();

    Task<Sensor> GetByIdAsync(int catId);

    Task<(Sensor Sensor, Double Average)[]> GetSensorsWithAverageValuesAsync();
    Task<bool> HasDuplicateAsync(Sensor sensor);

    Task InsertAsync(Sensor sensor);
    void Update(Sensor sensor);
    void Remove(Sensor sensor);
  }
}
