using SmartSchool.Core.Entities;
using System;
using System.Threading.Tasks;

namespace SmartSchool.Core.Contracts
{
  public interface IMeasurementRepository
  {
    Task AddAsync(Measurement measurement);
    Task AddRangeAsync(Measurement[] measurements);

    Task<int> GetCountAsync();
    Task<Measurement[]> GetAllBySensorIdAsync(int sensorId);
    Task<Measurement> GetByIdAsync(int id);
    Task<Measurement[]> GetAllAsync();
    Task<Measurement[]> GetLast20BySensorAsync(int sensorId);
    Task<Measurement> GetBySensorAndDateTimeAsync(int sensorId, DateTime dateTime);
    Task<long> GetCountOfMeasurementsAsync(string name, string location);
    Task<Measurement[]> GetLast3GreatestMeasurementsAsync(string name, string location);
    Task<double> GetAverageOfValidCo2Async(string location);
  }
}
