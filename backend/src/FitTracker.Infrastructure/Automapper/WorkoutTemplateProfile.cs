using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper;

public class WorkoutTemplateProfile : Profile
{
    public WorkoutTemplateProfile()
    {
        _ = CreateMap<WorkoutTemplateEf, WorkoutTemplate>()
            .ConstructUsing(src => new WorkoutTemplate(
                id: src.Id,
                userId: src.UserId,
                name: src.Name,
                description: src.Description,
                usageCount: src.UsageCount,
                lastUsedAt: src.LastUsedAt,
                createdAt: src.CreatedAt,
                updatedAt: src.UpdatedAt));

        _ = CreateMap<WorkoutTemplate, WorkoutTemplateEf>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.UsageCount, opt => opt.MapFrom(src => src.UsageCount))
            .ForMember(dest => dest.LastUsedAt, opt => opt.MapFrom(src => src.LastUsedAt))
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Exercises, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
