using MotorControl.Api.Models;

namespace MotorControl.Api.Services
{
    public interface IMotorService
    {
        IEnumerable<MotorModelResponse> Get();
        MotorModelResponse GetByPlate(string plate);
        MotorModelResponse GetById(string id);
        void Add(MotorModelRequest motor);
        bool Update(MotorRequestUpdateModel motor);
        void Delete(string id);
        IEnumerable<MotorModelResponse> GetMotorsByAvailablesAndPlate(bool ?available, string ?plate);
        bool PlateBelongsToAnotherMotor(MotorRequestUpdateModel motorModel);

    }
}
