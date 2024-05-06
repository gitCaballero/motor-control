using AutoMapper;
using MotorControl.Api.Entity;
using MotorControl.Api.Models;
using MotorControl.Api.Repository;

namespace MotorControl.Api.Services
{
    public class MotorService(IMotorRepository motorRepository, IMapper mapper) : IMotorService
    {
        public readonly IMotorRepository _motorRepository = motorRepository;
        private readonly IMapper _mapper = mapper;

        public MotorModelResponse Add(MotorModelRequest motorModel)
        {
            var motor = _mapper.Map<Motor>(motorModel);
            motor.IsAvailable = 1;

            var response = _motorRepository.Add(motor);

            var result = _mapper.Map<MotorModelResponse>(response);

            return result;

        }

        public void Delete(string plate)
        {
            _motorRepository.Delete(plate);
        }

        public IEnumerable<MotorModelResponse> GetMotorsByAvailablesIdAndPlate(bool ?available = null, string ?id = null, string ?plate = null)
        {
            var motorsModel = new List<MotorModelResponse>();
            var motors = _motorRepository.GetMotorsByAvailablesIdAndPlate(available, id, plate);
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

        public MotorModelResponse Update(MotorRequestUpdateModel motorModel)
        {
            var motor = _mapper.Map<Motor>(motorModel);
            motor.IsAvailable = motorModel.IsAvailable;
            var response =  _motorRepository.Update(motor);
            var result = _mapper.Map<MotorModelResponse>(response);
            return result;
        }

        public bool PlateBelongsToAnotherMotor(MotorRequestUpdateModel motorModel)
        {
            if (!string.IsNullOrEmpty(motorModel.Plate))
            {
                var existPlate = GetMotorsByAvailablesIdAndPlate(plate: motorModel.Plate!);
                if (!existPlate.Any() && existPlate.FirstOrDefault()!.Id != motorModel.Id)
                    return true;
            }
            return false;
        }
    }
}
