using System.ComponentModel.DataAnnotations;

namespace MotorControl.Api.Models
{
    public class MotorModel
    {
        
        [Display(Name = "MotorId")]
        public string ?Id { get; set; } 

        [Required(ErrorMessage = "ModelYear Required")]
        [Display(Name = "ModelYear")]
        public string ModelYear { get; set; }

        [Required(ErrorMessage = "Model Required")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required(ErrorMessage = "MotorPlate Required")]
        [Display(Name = "MotorPlate")]
        public string MotorPlate { get; set; }
    }
}
