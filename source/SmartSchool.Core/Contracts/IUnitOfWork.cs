using System;
using System.Threading.Tasks;

namespace SmartSchool.Core.Contracts
{
  public interface IUnitOfWork: IDisposable
    {
        IMeasurementRepository MeasurementRepository { get; }
        ISensorRepository SensorRepository { get; }

        Task<int> SaveChangesAsync();

        Task DeleteDatabaseAsync();
        Task MigrateDatabaseAsync();
    }
}
