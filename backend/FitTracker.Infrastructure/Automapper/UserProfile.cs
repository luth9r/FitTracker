using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserEf>()
                .ForMember(dest => dest.PreferredUnits, opt => opt.MapFrom(src => src.PreferredUnits.ToString()))
                .ForMember(dest => dest.Workouts, opt => opt.Ignore())
                .ForMember(dest => dest.CustomExercises, opt => opt.Ignore())
                .ForMember(dest => dest.WorkoutTemplates, opt => opt.Ignore())
                .ForMember(dest => dest.Achievements, opt => opt.Ignore())
                .ForMember(dest => dest.ExerciseRecords, opt => opt.Ignore());

            CreateMap<UserEf, User>()
                .ConstructUsing(src => new User(
                    src.Username,
                    src.Email,
                    src.PasswordHash,
                    src.FirstName,
                    src.LastName,
                    src.Avatar,
                    src.Bio,
                    UnitSystem.FromString(src.PreferredUnits),
                    src.IsEmailVerified))
                 .ForMember(dest => dest.PreferredUnits, opt => opt.Ignore())
                 .AfterMap((src, dest) =>
                 {
                     dest.SetDatabaseFields(src.Id, src.CreatedAt, src.UpdatedAt);
                 });
        }
    }
}
