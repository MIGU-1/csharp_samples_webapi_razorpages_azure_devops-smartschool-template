using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;

namespace SmartSchool.Web.Pages.Sensors
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Sensor Sensor { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sensor = await _unitOfWork.SensorRepository.GetByIdAsync(id.Value);

            if (Sensor == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var dbSensor = await _unitOfWork.SensorRepository.GetByIdAsync(Sensor.Id);

            if (dbSensor == null)
            {
                return NotFound();
            }

            dbSensor.Name = Sensor.Name;
            dbSensor.Location = Sensor.Location;
            dbSensor.Unit = Sensor.Unit;
            try
            {
                await _unitOfWork.SaveChangesAsync();
                return RedirectToPage("/Index");
            }
            catch (ValidationException validationException)
            {
                ValidationResult valResult = validationException.ValidationResult;
                ModelState.AddModelError("", valResult.ErrorMessage);
            }

            return Page();
        }
    }
}