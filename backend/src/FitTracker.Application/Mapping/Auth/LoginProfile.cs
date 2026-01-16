using AutoMapper;
using FitTracker.Application.Features.User.Common;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.Mapping.Auth;

public class LoginProfile : Profile
{
    public LoginProfile()
    {
        _ = CreateMap<UserEntity, LoginResponse>()
            .ConstructUsing(src => new LoginResponse(
                Username: src.Username,
                Email: src.Email,
                Jwt: string.Empty));
    }
}
