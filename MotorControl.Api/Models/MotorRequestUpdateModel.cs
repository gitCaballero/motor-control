using System.ComponentModel.DataAnnotations;

namespace MotorControl.Api.Models
{
    public class MotorRequestUpdateModel
    {
        [Required(ErrorMessage = "Id Required")]
        [Display(Name = "Id")]
        public string Id { get; set; }

        public int IsAvailable { get; set; }

        [Required(ErrorMessage = "Plate Required")]
        [Display(Name = "Plate")]
        public string Plate { get; set; }
    }
}
