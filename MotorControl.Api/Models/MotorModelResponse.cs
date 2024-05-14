namespace MotorControl.Api.Models
{
    public class MotorModelResponse : BaseMessage 
    {
        public string Id { get; set; }
        public int IsAvailable { get; set; }
        public string Year { get; set; }
        public string Identifier { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
    }
}
