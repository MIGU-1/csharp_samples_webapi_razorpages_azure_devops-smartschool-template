using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;
using SmartSchool.Web.DataTransferObjects;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.Web.ApiControllers
{
    /// <summary>
    /// API-Controller für die Abfrage von Messwerten
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor mit DI
        /// </summary>
        /// <param name="unitOfWork"></param>
        public MeasurementsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Liefert den Messwert per Id und den zugehörigen Sensorinformationen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var measurement = await _unitOfWork.MeasurementRepository.GetByIdAsync(id);
            if (measurement == null) return NotFound();
            var result = new { measurement.Sensor.Name, measurement.Sensor.Location, measurement.Time, measurement.Value, measurement.Sensor.Unit };
            return Ok(result);
        }

        /// <summary>
        /// Liefert den nächsten Messwert ab einer vorgegebenen Zeit
        /// </summary>
        /// <param name="sensorId"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns>Messwert oder NotFound, falls es keinen Messwert ab der Zeit gab</returns>
        [HttpGet]
        [Route("measurementat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMeasurementForSensorAt(int sensorId, string date, string time)
        {
            DateTime dateTime = DateTime.Parse(date + " " + time);
            var measurement = await _unitOfWork.MeasurementRepository.GetBySensorAndDateTimeAsync(sensorId, dateTime);
            if (measurement == null) return NotFound();
            var result = new { measurement.Sensor.Name, measurement.Sensor.Location, measurement.Time, measurement.Value, measurement.Sensor.Unit };
            return Ok(result);
        }


        /// <summary>
        /// Fügt für den Sensor mit der Id einen Messwert hinzu
        /// </summary>
        /// <param name="measurementDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostMeasurement([FromBody] PostMeasurementDto measurementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var sensor = await _unitOfWork.SensorRepository.GetByIdAsync(measurementDto.SensorId);
                if (sensor == null)
                {
                    return NotFound();
                }

                Measurement newMeasurement = new Measurement
                {
                    SensorId = measurementDto.SensorId,
                    Time = measurementDto.Time,
                    Value = measurementDto.Value
                };
                
                await _unitOfWork.MeasurementRepository.AddAsync(newMeasurement);
                await _unitOfWork.SaveChangesAsync();
                return CreatedAtAction(
                    nameof(Get),
                    new { id = newMeasurement.Id },
                    measurementDto);
            }
            catch (ValidationException validationException)
            {
                ValidationResult valResult = validationException.ValidationResult;
                ModelState.AddModelError(valResult.MemberNames.First(), valResult.ErrorMessage);
                return BadRequest(ModelState);
            }

        }

        private async Task<bool> MeasurementExists(int id) =>
          await _unitOfWork.MeasurementRepository.GetByIdAsync(id) != null;

    }
}
