using AutoMapper;
using MotorControl.Api.Entity;
using MotorControl.Api.Models;

namespace MotorControl.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MotorModel, Motor>();
            CreateMap<MotorModel, MotorBase>();
            CreateMap<Motor, ShowMotorModel>();
        }

    }
}
