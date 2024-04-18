using AutoMapper;
using MotorControl.Api.Entity;
using MotorControl.Api.Models;

namespace MotorControl.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MotorModelRequest, Motor>();
            CreateMap<MotorRequestUpdateModel, Motor>();
            CreateMap<MotorModelRequest, Base>();
            CreateMap<Motor, MotorModelResponse>();
        }
    }
}
