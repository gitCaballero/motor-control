using AutoMapper;
using MotorControl.Api.Entity;
using MotorControl.Api.Models;
using MotorControl.Api.Repository;
using RentalMotor.Api.Services.Network.MessageSender;

namespace MotorControl.Api.Services
{
    public class MotorService(IMotorRepository motorRepository, IMapper mapper, IRabbitMQMessageSender rabbitMQMessageSender) : IMotorService
    {
        public readonly IMotorRepository _motorRepository = motorRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IRabbitMQMessageSender _rabbitMQMessageSender = rabbitMQMessageSender;

        public MotorModelResponse Add(MotorModelRequest motorModel)
        {
            var motor = _mapper.Map<Motor>(motorModel);
            motor.IsAvailable = 1;

            var response = _motorRepository.Add(motor);

            var result = _mapper.Map<MotorModelResponse>(response);

            IEnumerable<MotorModelResponse> motors = [result];

            _rabbitMQMessageSender.SendMessage(motors, "motor-registered");

            return result;
        }

        public void Delete(string plate)
        {
            _motorRepository.Delete(plate);
        }

        public IEnumerable<MotorModelResponse> GetMotorsByAvailablesIdAndPlate(bool? available = null, string? id = null, string? plate = null)
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
                if (available == true)
                    _rabbitMQMessageSender.SendMessage(motorsModel, "motors-availabel");

                return motorsModel;
            }
            return motorsModel;
        }

        public MotorModelResponse Update(MotorRequestUpdateModel motorModel)
        {
            var motor = _motorRepository.GetMotorsByAvailablesIdAndPlate(id: motorModel.Id).FirstOrDefault();
            motor.Plate = motorModel.Plate.ToUpper();
            motor.IsAvailable = motorModel.IsAvailable;

            var response = _motorRepository.Update(motor);
            var result = _mapper.Map<MotorModelResponse>(response);
            IEnumerable<MotorModelResponse> motorsResponse = [result];
            _rabbitMQMessageSender.SendMessage(motorsResponse, "motor-updated");

            return result;
        }

        public bool PlateBelongsToAnotherMotor(MotorRequestUpdateModel motorModel)
        {
            if (!string.IsNullOrEmpty(motorModel.Plate))
            {
                var existPlate = GetMotorsByAvailablesIdAndPlate(plate: motorModel.Plate!);
                if (existPlate.Any() && existPlate.FirstOrDefault()!.Id != motorModel.Id)
                    return true;
            }
            return false;
        }
    }
}
