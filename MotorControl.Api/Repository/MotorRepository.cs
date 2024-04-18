using MotorControl.Api.Entity;
using MotorControl.Api.Models;
using MotorControl.Api.Repository.Context;

namespace MotorControl.Api.Repository
{
    public class MotorRepository : IMotorRepository
    {
        private readonly MotorControlDbContext _context;

        public MotorRepository(MotorControlDbContext context)
        {
            _context = context;
        }

        public void Add(Motor motor)
        {
            _context.motors.Add(motor);
            _context.SaveChanges();
        }

        public void Delete(string plate)
        {
            var motor = _context.motors.Where(x => x.MotorPlate == plate).FirstOrDefault()!;
            if (motor != null)
            {
                _context.motors.Remove(motor);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Motor> Get()
        {
            return _context.motors;
        }

        public Motor GetById(string id)
        {
            return _context.motors.Where(x => x.Id == id).FirstOrDefault()!;
        }

        public Motor GetByPlate(string plate)
        {
            return _context.motors.Where(u => u.MotorPlate == plate).FirstOrDefault()!;
        }

        public bool Update(Motor motor)
        {

            var existMotor = _context.motors.FirstOrDefault(u => u.Id == motor.Id);
            if (existMotor != null)
            {
                existMotor.Model = motor.Model ?? existMotor.Model;
                existMotor.MotorPlate = motor.MotorPlate ?? existMotor.MotorPlate;
                existMotor.IsAvalable = motor.IsAvalable;
                existMotor.ModelYear = motor.ModelYear ?? existMotor.ModelYear;
                _context.ChangeTracker.Clear();
                _context.Update(existMotor);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
