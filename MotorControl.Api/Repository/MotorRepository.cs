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
            _context.motors.Add(motor);
            _context.SaveChanges();
        }

        public void Delete(string plate)
        {
            var user = _context.motors.FirstOrDefault(u => u.MotorPlate == plate);
            if (user != null)
            {
                _context.motors.Remove(user);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Motor> Get()
        {
            return _context.motors;
        }

        public Motor GetByPLate(string plate)
        {
            return _context.motors.Where(u => u.MotorPlate == plate).FirstOrDefault()!;
        }

        public void Update(Motor motor)
        {

            var existingUser = _context.motors.FirstOrDefault(u => u.Id == motor.Id);
            if (existingUser != null)
            {
                _context.ChangeTracker.Clear();
                _context.Update(motor);
                _context.SaveChanges();
            }
        }
    }
}
