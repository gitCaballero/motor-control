using System.ComponentModel.DataAnnotations;

namespace MotorControl.Api.Models
{
    public class MotorRequestUpdateModel
    {
        public required string Id { get; set; }

        public int IsAvailable { get; set; }

        public required string Plate { get; set; }
    }
}
