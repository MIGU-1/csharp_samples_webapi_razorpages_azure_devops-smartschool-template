using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;

namespace SmartSchool.Web.Pages.Sensors
{
    public class ListModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<SelectListItem> Sensors { get; set; }

        /// <summary>
        /// In Combobox ausgewählter Sensor und Location
        /// </summary>
        [BindProperty]
        [DisplayName("Sensor")]
        public int SelectedSensorId { get; set; }


        public Sensor Sensor { get; set; }

        public IList<Measurement> Measurements { get; set; }

        public async Task<IActionResult> OnGet(int id) => await CreatePageAsync(id);

        public async Task<IActionResult> OnPost() => await CreatePageAsync(SelectedSensorId);

        private async Task<IActionResult> CreatePageAsync(int id)
        {
            SelectedSensorId = id;
            Sensor = await _unitOfWork.SensorRepository.GetByIdAsync(SelectedSensorId);
            if (Sensor == null)
            {
                return NotFound();
            }

            var sensors = await _unitOfWork
              .SensorRepository
              .GetAllAsync();

            Sensors = sensors
                .Select(sensor => new SelectListItem(
                    $"{sensor.Name}/{sensor.Location}",
                    sensor.Id.ToString()))
                .ToList();

            Measurements = await _unitOfWork.MeasurementRepository.GetLast20BySensorAsync(SelectedSensorId);

            return Page();
        }
    }
}