using MotorControl.Api.Entity;
using MotorControl.Api.Models;

namespace MotorControl.Api.Services
{
    public interface IMotorService
    {
        IEnumerable<MotorModel> Get();
        MotorModel GetByPlate(string id);
        void Add(MotorModel motor);
        void Update(MotorModel motor);
        void Delete(string id);
    }
}
