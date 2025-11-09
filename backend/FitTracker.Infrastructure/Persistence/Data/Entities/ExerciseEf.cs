using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Enums;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
	public class ExerciseEf : BaseEntityEf
	{
		public string Name { get; set; } = null!;
		public string? Description
		{
			get; set;
		}
		public string? ImageUrl
		{
			get; set;
		}
		public string? VideoUrl
		{
			get; set;
		}
		public int MuscleGroup
		{
			get; set;
		}
		public int Equipment
		{
			get; set;
		}
		public bool IsCustom
		{
			get; set;
		}
		public Guid? UserId
		{
			get; set;
		}

		public UserEf? User
		{
			get; set;
		}
		public ICollection<WorkoutExerciseEf> WorkoutExercises
		{
			get; set;
		}
	}
}
