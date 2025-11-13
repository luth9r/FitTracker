using AutoMapper;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Domain.Entities;

namespace FitTracker.Application.Mapping.Auth
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<User, LoginResponse>()
                .ForMember(dest => dest.JWT, opt => opt.Ignore());
        }
    }
}
