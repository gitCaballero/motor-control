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
        public void Add(MotorModelRequest motorModel)
        {
            var motor = _mapper.Map<Motor>(motorModel);
            motor.IsAvalable = 1;

            _motorRepository.Add(motor);
        }

        public void Delete(string plate)
        {
            _motorRepository.Delete(plate);
        }

        public IEnumerable<MotorModelResponse> Get()
        {
            var motorsModel = new List<MotorModelResponse>();
            var motors = _motorRepository.Get();
            if (motors != null && motors.Any())
            {
                foreach (var motor in motors)
                {
                    var motorModel = _mapper.Map<MotorModelResponse>(motor);
                    motorsModel.Add(motorModel);
                }
                return motorsModel;
            }
            return motorsModel;
        }
        
        public IEnumerable<MotorModelResponse> GetMotorsByAvailablesAndPlate(bool ?available, string ?plate)
        {
            var motorsModel = new List<MotorModelResponse>();
            var motors = _motorRepository.GetMotorsByAvailablesAndPlate(available, plate);
            if (motors.Any())
            {
                foreach (var motor in motors)
                {
                    var motorModel = _mapper.Map<MotorModelResponse>(motor);
                    motorsModel.Add(motorModel);
                }
                return motorsModel;
            }
            return motorsModel;
        }

        public MotorModelResponse GetById(string id)
        {
            var motor = _motorRepository.GetById(id);
            var motorResponse = _mapper.Map<MotorModelResponse>(motor);
            return motorResponse;
        }

        public MotorModelResponse GetByPlate(string plate)
        {
            var motor = _motorRepository.GetByPlate(plate);

            var motorModel = _mapper.Map<MotorModelResponse>(motor);

            return motorModel;
        }
        public bool Update(MotorRequestUpdateModel motorModel)
        {
            var motor = _mapper.Map<Motor>(motorModel);
            motor.IsAvalable = motorModel.IsAvailable;
            return _motorRepository.Update(motor);
        }

        public bool PlateBelongsToAnotherMotor(MotorRequestUpdateModel motorModel)
        {
            if (!string.IsNullOrEmpty(motorModel.Plate))
            {
                var existPlate = GetByPlate(motorModel.Plate!);
                if (existPlate != null && existPlate.Id != motorModel.Id)
                    return true;
            }
            return false;
        }
    }
}
