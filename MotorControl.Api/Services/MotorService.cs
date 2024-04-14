using AutoMapper;
using MotorControl.Api.Entity;
using MotorControl.Api.Models;
using MotorControl.Api.Repository;

namespace MotorControl.Api.Services
{
    public class MotorService : IMotorService
    {
        public readonly IMotorRepository _motorRepository;
        private readonly IMapper _mapper;

        public MotorService(IMotorRepository motorRepository, IMapper mapper)
        {
            _motorRepository = motorRepository;
            _mapper = mapper;
        }
        public void Add(MotorModel motorModel)
        {
            var motor = _mapper.Map<Motor>(motorModel);

            _motorRepository.Add(motor);
        }

        public void Delete(string id)
        {
            _motorRepository.Delete(id);
        }

        public IEnumerable<MotorModel> Get()
        {
            var motorsModel = new List<MotorModel>();
            var motors = _motorRepository.Get();
            if (motors != null && motors.Any())
            {
                foreach (var motor in motors)
                {
                    var motorModel = _mapper.Map<MotorModel>(motor);
                    motorsModel.Add(motorModel);
                }
                return motorsModel;
            }
            return motorsModel;
        }

        public MotorModel GetByPlate(string plate)
        {
            var motor = _motorRepository.GetByPLate(plate);

            var motorModel = _mapper.Map<MotorModel>(motor);

            return motorModel;
        }

        public void Update(MotorModel motorModel)
        {
            var motor = _mapper.Map<Motor>(motorModel);

            _motorRepository.Update(motor);
        }
    }
}
