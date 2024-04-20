using System.ComponentModel.DataAnnotations;

namespace MotorControl.Api.Models
{
    public class MotorModelRequest
    {
        [Required(ErrorMessage = "Year Required")]
        [Display(Name = "Year")]
        public string Year { get; set; }

        [Required(ErrorMessage = "Identifier Required")]
        [Display(Name = "Identifier")]
        public string Identifier { get; set; }

        [Required(ErrorMessage = "Model Required")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Plate Required")]
        [Display(Name = "Plate")]
        public string Plate { get; set; }
    }
}
