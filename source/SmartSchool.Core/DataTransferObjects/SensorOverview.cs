namespace SmartSchool.Core.DataTransferObjects
{
    public class SensorOverview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int CountMeasurements { get; set; }
        public string Unit { get; set; }
    }
}
