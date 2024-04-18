using MotorControl.Api.Entity;

namespace MotorControl.Api.Repository
{
    public interface IMotorRepository
    {
        IEnumerable<Motor> Get();
        Motor GetByPlate(string plate);
        Motor GetById(string id);
        void Add(Motor motor);
        bool Update(Motor motor);
        void Delete(string id);
    }
}
