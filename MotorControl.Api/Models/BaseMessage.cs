namespace MotorControl.Api.Models
{
    public class BaseMessage : IMessageBus
    {
        public string Id { get; set; }
        public string MessageCreated { get; set; }
    }
}
