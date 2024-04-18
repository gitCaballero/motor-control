using System.ComponentModel.DataAnnotations;

namespace MotorControl.Api.Models
{
    public class MotorModelResponse : MotorModelRequest
    {
        public string Id { get; set; }
        public bool IsAvalable { get; set; }
    }
}
