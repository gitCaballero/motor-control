namespace MotorControl.Api.Models
{
    public class MotorModelResponse : MotorModelRequest 
    {
        public string Id { get; set; }
        public int IsAvailable { get; set; }
    }
}
