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

        public Motor Add(Motor motor)
        {
            motor.Plate = motor.Plate.ToUpper();
            motor.Model = motor.Model.ToUpper();

            _context.motors.Add(motor);
            _ = _context.SaveChangesAsync().Result;
            return motor;
        }

        public void Delete(string plate)
        {
            var motor = _context.motors.Where(x => x.Plate.ToUpper().Equals(plate.ToUpper())).FirstOrDefault()!;
            if (motor != null)
            {
                _context.motors.Remove(motor);
                _context.SaveChanges();
            }            
        }

        public IEnumerable<Motor> GetMotorsByAvailablesIdAndPlate(bool? available = null, string? id = null, string? plate = null)
        {
            if (available != null && id == null && plate == null)
                return _context.motors.Where(x => x.IsAvailable == (available == true ? 1 : 0));

            if (available == null && id != null && plate == null)
                return _context.motors.Where(x => x.Id.Equals(id));

            if (available == null && id == null && plate != null)
                return _context.motors.Where(x => x.Plate.ToUpper().Equals(plate.ToUpper()));

            if (available != null && id != null && plate != null)
                return _context.motors.Where(x => x.Plate.ToUpper().Equals(plate.ToUpper()) && x.IsAvailable == (available == true ? 1 : 0) && x.Id.Equals(id));

            return _context.motors;

        }

        public Motor Update(Motor motor)
        {
            _context.ChangeTracker.Clear();
            _context.Update(motor);
            _ = _context.SaveChangesAsync().Result;
            return motor;
        }
    }
}
