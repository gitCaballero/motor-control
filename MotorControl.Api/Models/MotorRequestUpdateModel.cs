using System.ComponentModel.DataAnnotations;

namespace MotorControl.Api.Models
{
    public class MotorRequestUpdateModel
    {
        [Required(ErrorMessage = "Id Required")]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "IsAvalable Required")]
        [Display(Name = "IsAvalable")]
        public bool? IsAvalable { get; set; }
        public string? ModelYear { get; set; }
        public string? Model { get; set; }
        public string? MotorPlate { get; set; }
    }
}
