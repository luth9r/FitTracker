using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Enums;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
	public class SetEf : BaseEntityEf
	{
		public Guid WorkoutExerciseId
		{
			get; set;
		}
		public int SetNumber
		{
			get; set;
		}
		public decimal WeightKg
		{
			get; set;
		}
		public int Reps
		{
			get; set;
		}
		public int? RestSeconds
		{
			get; set;
		}
		public int SetType
		{
			get; set;
		}
		public bool IsCompleted
		{
			get; set;
		}
		public DateTime? CompletedAt
		{
			get; set;
		}

		public WorkoutExerciseEf? WorkoutExercise
		{
			get; set;
		}
	}
}
