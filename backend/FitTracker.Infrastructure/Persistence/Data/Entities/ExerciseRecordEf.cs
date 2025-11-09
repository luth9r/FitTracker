using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
	public class ExerciseRecordEf : BaseEntityEf
	{
		public Guid UserId
		{
			get; set;
		}
		public Guid ExerciseId
		{
			get; set;
		}

		public decimal MaxWeight_Kilograms
		{
			get; set;
		}

		public int MaxReps
		{
			get; set;
		}
		public decimal MaxVolume
		{
			get; set;
		}
		public decimal MaxTotalVolume
		{
			get; set;
		}

		public DateTime MaxWeightDate
		{
			get; set;
		}
		public DateTime MaxRepsDate
		{
			get; set;
		}
		public DateTime MaxVolumeDate
		{
			get; set;
		}
		public DateTime MaxTotalVolumeDate
		{
			get; set;
		}

		public int TotalWorkouts
		{
			get; set;
		}
		public int TotalSets
		{
			get; set;
		}
		public int TotalReps
		{
			get; set;
		}
		public decimal TotalLifted
		{
			get; set;
		}
		public DateTime LastPerformed
		{
			get; set;
		}

		public UserEf? User
		{
			get; set;
		}
		public ExerciseEf? Exercise
		{
			get; set;
		}
	}
}
