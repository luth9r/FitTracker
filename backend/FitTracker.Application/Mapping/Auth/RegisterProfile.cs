using AutoMapper;
using FitTracker.Application.DTOs.Auth;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.Mapping
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<UserEntity, RegisterResponse>()
                .ForMember(dest => dest.JWT, opt => opt.Ignore());
        }
    }
}
