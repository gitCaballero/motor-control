namespace MotorControl.Api.Models
{
    public class MotorModelRequest
    {
        public required string Year { get; set; }
        public required string Identifier { get; set; }
        public required string Model { get; set; }
        public required string Plate { get; set; }
    }
}
