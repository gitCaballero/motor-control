using MotorControl.Api.Entity;

namespace MotorControl.Api.Repository
{
    public interface IMotorRepository
    {
        Motor Add(Motor motor);
        Motor Update(Motor motor);
        void Delete(string id);
        IEnumerable<Motor> GetMotorsByAvailablesIdAndPlate(bool? available = null, string? id = null, string? plate = null);
    }
}
