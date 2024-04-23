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
        
        public IEnumerable<Motor> GetMotorsByAvailablesAndPlate(bool? available, string? plate)
        {
            if (available != null && plate == null)            
                return _context.motors.Where(x => x.IsAvailable == (available == true ? 1 : 0));

            if (available == null && plate != null)
                return _context.motors.Where(x => x.Plate.ToUpper() == plate.ToUpper());
            
            if (available != null && plate != null)
                return _context.motors.Where(x => x.Plate.ToUpper() == plate.ToUpper() && x.IsAvailable == (available == true ? 1 : 0));

            return _context.motors;

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
                existMotor.IsAvailable = motor.IsAvailable;
                _context.ChangeTracker.Clear();
                _context.Update(existMotor);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
