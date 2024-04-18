namespace MotorControl.Api.Entity
{
    public class Motor : Base
    {
        public string ModelYear { get; set; }
        public string Model { get; set; }
        public string MotorPlate { get; set; }
        public bool IsAvalable {  get; set; }
    }
}
