using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EVCS.TriNM.Services.Extensions
{
   
    public class PasswordEncryptionService 
    {
        private readonly IPasswordHasher<object> _passwordHasher;

        public PasswordEncryptionService()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        public string EncryptPassword(string plainPassword)
        {
            return _passwordHasher.HashPassword(null!, plainPassword);
        }

        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
                var result = _passwordHasher.VerifyHashedPassword(null!, hashedPassword, plainPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
