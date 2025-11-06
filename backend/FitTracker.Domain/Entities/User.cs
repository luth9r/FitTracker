using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // User Profile
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Bio { get; set; }

        // Settings
        public bool IsPublic { get; set; }
        public string PreferredUnits { get; set; } // kg, lbs
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
        public ICollection<Program> Programs { get; set; } = new List<Program>();
        public ICollection<Exercise> CustomExercises { get; set; } = new List<Exercise>();
        public ICollection<UserFriend> Friends { get; set; } = new List<UserFriend>();
        public ICollection<UserFriend> FriendOf { get; set; } = new List<UserFriend>();
    }
}
