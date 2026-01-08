using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper;

public class ExerciseProfile : Profile
{
    public ExerciseProfile()
    {
        _ = CreateMap<ExerciseEf, Exercise>()
            .ConstructUsing(src => new Exercise(
                id: src.Id,
                name: src.Name,
                description: src.Description,
                imageUrl: src.ImageUrl,
                videoUrl: src.VideoUrl,
                muscleGroup: (MuscleGroup)src.MuscleGroup,
                equipment: (Equipment)src.Equipment,
                createdByUserId: src.CreatedByUserId,
                createdAt: src.CreatedAt,
                updatedAt: src.UpdatedAt));

        _ = CreateMap<Exercise, ExerciseEf>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MuscleGroup, opt => opt.MapFrom(src => (int)src.MuscleGroup))
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src => (int)src.Equipment))
            .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
            .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}