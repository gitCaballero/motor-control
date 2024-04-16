using MotorControl.Api.Entity;
using MotorControl.Api.Models;

namespace MotorControl.Api.Services
{
    public interface IMotorService
    {
        IEnumerable<ShowMotorModel> Get();
        ShowMotorModel GetByPlate(string plate);
        void Add(MotorModel motor);
        bool Update(ShowMotorModel motor);
        void Delete(string id);
    }
}
