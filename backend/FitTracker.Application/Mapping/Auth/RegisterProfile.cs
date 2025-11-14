using AutoMapper;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Domain.Entities;

namespace FitTracker.Application.Mapping
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<User, RegisterResponse>();
        }
    }
}
