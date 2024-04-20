using MotorControl.Api.Entity;
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
            motor.Plate = motor.Plate.ToUpper();
            motor.Model = motor.Model.ToUpper();

            _context.motors.Add(motor);
            _context.SaveChanges();
        }

        public void Delete(string plate)
        {
            var motor = _context.motors.Where(x => x.Plate.ToUpper() == plate.ToUpper()).FirstOrDefault()!;
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
        
        public IEnumerable<Motor> GetMotorsAvailables()
        {
            return _context.motors.Where(x => x.IsAvalable == 1);
        }

        public Motor GetById(string id)
        {
            return _context.motors.Where(x => x.Id == id).FirstOrDefault()!;
        }

        public Motor GetByPlate(string plate)
        {
            plate = plate.ToUpper();
            return _context.motors.Where(u => u.Plate.ToUpper() == plate.ToUpper()).FirstOrDefault()!;
        }

        public bool Update(Motor motor)
        {

            var existMotor = _context.motors.FirstOrDefault(u => u.Id == motor.Id);
            if (existMotor != null)
            {
                existMotor.Plate = motor.Plate.ToUpper() ?? existMotor.Plate.ToUpper();
                existMotor.IsAvalable = motor.IsAvalable;
                _context.ChangeTracker.Clear();
                _context.Update(existMotor);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
