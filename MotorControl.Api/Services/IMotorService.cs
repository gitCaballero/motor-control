using MotorControl.Api.Models;

namespace MotorControl.Api.Services
{
    public interface IMotorService
    {
        MotorModelResponse Add(MotorModelRequest motor);
        MotorModelResponse Update(MotorRequestUpdateModel motor);
        void Delete(string id);
        IEnumerable<MotorModelResponse> GetMotorsByAvailablesIdAndPlate(bool? available = null, string? id = null, string? plate = null);
        bool PlateBelongsToAnotherMotor(MotorRequestUpdateModel motorModel);

    }
}
