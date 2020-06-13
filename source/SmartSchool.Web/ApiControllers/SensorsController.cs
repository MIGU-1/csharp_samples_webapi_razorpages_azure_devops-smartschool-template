using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;

namespace SmartSchool.Web.ApiControllers
{
    /// <summary>
    /// API-Controller für die Abfrage von Mitgliedern
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor mit DI
        /// </summary>
        /// <param name="unitOfWork"></param>
        public SensorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Liefert alle Namen der Sensoren
        /// </summary>
        /// <response code="200">Die Abfrage war erfolgreich.</response>
        // GET: api/Categories
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string[]>> GetSensorNames()
        {
            Sensor[] sensors = await _unitOfWork.SensorRepository.GetAllAsync();
            return sensors.Select(s => s.Name).ToArray();
        }

        /// <summary>
        /// Liefert den Sensor mit der Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSensor(int id)
        {
            Sensor sensor = await _unitOfWork.SensorRepository.GetByIdAsync(id);
            if (sensor == null)
            {
                return NotFound();
            }
            return Ok(new {sensor.Id, sensor.Name, sensor.Location});
        }


        /// <summary>
        /// Liefert die Sensoren und die Anzahl ihrer Messwerte
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet()]
        [Route("countofmeasurements")]
        public async Task<IActionResult> GetSensorsWithCountOfMeasurements()
        {
            var sensors = await _unitOfWork.SensorRepository.GetAllWithMeasurementsCounterAsync();
            var sensorsDto = sensors
                .Select(s => new { s.Sensor.Id, s.Sensor.Location, s.Sensor.Name, s.Sensor.Unit, MeasurementsCount = s.Count })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Location)
                .ToArray();
            return Ok(sensorsDto);
        }


        /// <summary>
        /// Liefert Sensoren mit den Mittelwerten ihrer Messwerte
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("averagevalues")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSensorsWithAverageValues()
        {
            var resultTuples = await _unitOfWork.SensorRepository.GetSensorsWithAverageValuesAsync();
            var sensorsWithAverageValues = resultTuples
                .Select(rt => new { rt.Sensor.Name, rt.Sensor.Location, rt.Average, rt.Sensor.Unit })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Location)
                .ToArray();
            return Ok(sensorsWithAverageValues);
        }
    }
}
