using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FitTracker.Application.DTOs.Auth;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.Mapping.Auth
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            _ = CreateMap<UserEntity, RegisterResponse>()
                .ConstructUsing(src => new RegisterResponse(
                    Username: src.Username,
                    Email: src.Email));
        }
    }
}
