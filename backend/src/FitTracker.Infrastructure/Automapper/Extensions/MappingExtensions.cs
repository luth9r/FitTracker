using AutoMapper;
using FitTracker.Domain.Abstract;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper.Extensions;

public static class MappingExtensions
{
    public static IMappingExpression<TSource, TDest> IgnoreDomainEventsAndAudit<TSource, TDest>(
        this IMappingExpression<TSource, TDest> mapping)
        where TSource : BaseEntity
        where TDest : BaseEntityEf
    {
        return mapping
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                if (src.DomainEvents == null || src.DomainEvents.Count == 0)
                {
                    return;
                }

                foreach (var domainEvent in src.DomainEvents)
                {
                    dest.AddDomainEvent(domainEvent);
                }
            });
    }
}
