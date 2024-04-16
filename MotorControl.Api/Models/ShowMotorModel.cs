using System.ComponentModel.DataAnnotations;

namespace MotorControl.Api.Models
{
    public class ShowMotorModel : MotorModel
    {
        [Display(Name = "MotorId")]
        public string? Id { get; set; }
    }
}
