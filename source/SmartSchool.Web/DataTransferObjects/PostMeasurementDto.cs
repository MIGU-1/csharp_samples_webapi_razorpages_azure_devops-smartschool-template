using System;
using SmartSchool.Core.Validations;

namespace SmartSchool.Web.DataTransferObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class PostMeasurementDto
    {
        public int SensorId { get; set; }

        [NoFutureTimeValidation]
        public DateTime Time { get; set; }
        public double  Value { get; set; }
    }
}
