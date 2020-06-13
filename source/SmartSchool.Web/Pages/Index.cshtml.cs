using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartSchool.Core.Contracts;
using SmartSchool.Core.DataTransferObjects;

namespace SmartSchool.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public SensorOverview[] SensorsOverview { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGet()
        {
          var measurements = await _unitOfWork
            .SensorRepository
            .GetAllWithMeasurementsCounterAsync();

            SensorsOverview = measurements
                .Select(s => new SensorOverview
                {
                    Name = s.Sensor.Name,
                    Location = s.Sensor.Location,
                    CountMeasurements = s.Count,
                    Unit = s.Sensor.Unit,
                    Id = s.Sensor.Id
                })
                .ToArray();

            return Page();
        }
    }
}
