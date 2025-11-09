using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
	public class WorkoutExerciseProfile : Profile
	{
		public WorkoutExerciseProfile()
		{
			CreateMap<WorkoutExercise, WorkoutExerciseEf>()
				.ForMember(dest => dest.Workout, opt => opt.Ignore())
				.ForMember(dest => dest.Exercise, opt => opt.Ignore())
				.ForMember(dest => dest.Sets, opt => opt.Ignore());

			CreateMap<WorkoutExerciseEf, WorkoutExercise>()
				.ConstructUsing(src => new WorkoutExercise(
					src.WorkoutId,
					src.ExerciseId,
					src.OrderIndex,
					src.Notes
				))
				.AfterMap(
					(src, dest) =>
					{
                        dest.SetDatabaseFields(src.Id, src.CreatedAt, src.UpdatedAt);
					}
				);
		}
	}
}
