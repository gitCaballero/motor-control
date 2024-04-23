namespace MotorControl.Api.Models
{
    public class MotorModelResponse : MotorModelRequest
    {
        public string Id { get; set; }
        public int IsAvalable { get; set; }
    }
}
