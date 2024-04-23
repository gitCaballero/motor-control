namespace MotorControl.Api.Entity
{
    public class Motor : Base
    {
        public string Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
        public int IsAvailable { get; set; }
        public string Identifier { get; set; }

    }
}
