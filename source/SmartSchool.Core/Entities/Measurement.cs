using System;
using System.ComponentModel.DataAnnotations.Schema;
using SmartSchool.Core.Validations;


namespace SmartSchool.Core.Entities
{
    public class Measurement : EntityObject
    {

        public int SensorId { get; set; }

        [ForeignKey(nameof(SensorId))]
        public Sensor Sensor { get; set; }

        [NoFutureTimeValidation]
        public DateTime Time { get; set; }
        public double Value { get; set; }

    }
}
