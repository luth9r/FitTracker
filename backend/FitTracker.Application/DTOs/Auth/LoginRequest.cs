using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Auth
{
    /// <summary>
    /// DTO for user login request
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User's email address
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
