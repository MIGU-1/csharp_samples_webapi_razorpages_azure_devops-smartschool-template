using System;
using System.Linq;
using System.Threading.Tasks;
using SmartSchool.Core.Contracts;
using SmartSchool.Persistence;
using SmartSchool.TestConsole;

namespace SmartSchool.ImportConsole
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine(">> Import der Measurements und Sensors in die Datenbank");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine();
            using (IUnitOfWork unitOfWorkImport = new UnitOfWork())
            {
                Console.WriteLine("Datenbank löschen");
                await unitOfWorkImport.DeleteDatabaseAsync();
                Console.WriteLine("Datenbank migrieren");
                await unitOfWorkImport.MigrateDatabaseAsync();
                Console.WriteLine("Messwerte werden von measurements.csv eingelesen");

                var measurements = ImportController.ReadFromCsv().ToArray();

                if (measurements.Length == 0)
                {
                    Console.WriteLine("!!! Es wurden keine Messwerte eingelesen");
                    return;
                }

                Console.WriteLine(
                    $"  Es wurden {measurements.Count()} Messwerte eingelesen, werden in Datenbank gespeichert ...");
                await unitOfWorkImport.MeasurementRepository.AddRangeAsync(measurements);
                int countSensors = measurements.GroupBy(m => m.Sensor).Count();
                int savedRows = await unitOfWorkImport.SaveChangesAsync();
                Console.WriteLine
                (
                    $"{countSensors} Sensoren und {savedRows - countSensors} Messwerte wurden in Datenbank gespeichert!");
            }

            Console.Write("Beenden mit Eingabetaste ...");
            Console.ReadLine();
        }

    }
}
