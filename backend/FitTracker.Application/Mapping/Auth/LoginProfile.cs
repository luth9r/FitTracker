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
            .ForCtorParam("Username", opt => opt.MapFrom(src => src.Username))
            .ForCtorParam("Email", opt => opt.MapFrom(src => src.Email))
            .ForCtorParam("JWT", opt => opt.MapFrom(src => string.Empty));
        }
    }
}
