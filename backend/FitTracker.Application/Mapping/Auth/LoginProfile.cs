using AutoMapper;
using FitTracker.Application.DTOs.Auth;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.Mapping.Auth
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            _ = CreateMap<UserEntity, LoginResponse>()
                .ForMember(dest => dest.JWT, opt => opt.Ignore())
                .ForMember(dest => dest.PreferredUnits, opt => opt.MapFrom(src => src.PreferredUnits.ToString()));
        }
    }
}
