using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Application.Interfaces;
using Crypt = BCrypt.Net.BCrypt;

namespace FitTracker.Infrastructure.Services
{
	public class PasswordHasher : IPasswordHasher
	{
		private const int WorkFactor = 11;

		public string HashPassword(string password)
		{
			if (string.IsNullOrWhiteSpace(password))
				throw new ArgumentException("Password cannot be empty");

			return Crypt.HashPassword(password, WorkFactor);
		}

		public bool VerifyPassword(string password, string hash)
		{
			if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
				return false;

			try
			{
				return Crypt.Verify(password, hash);
			}
			catch
			{
				return false;
			}
		}

		public bool NeedsRehash(string hash)
		{
			return Crypt.PasswordNeedsRehash(hash, WorkFactor);
		}
	}
}
