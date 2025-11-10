using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
	/// Base class for all database entities with common properties.
	/// </summary>
	public abstract class BaseEntityEf
    {
        /// <summary>
        /// Unique identifier for the entity.
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Timestamp when the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; protected set; }

        /// <summary>
        /// Timestamp when the entity was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; protected set; }

        /// <summary>
        /// Initializes a new instance with auto-generated ID and timestamps.
        /// </summary>
        protected BaseEntityEf()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
