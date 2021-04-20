using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DbBicyclesLab.Models;

namespace DbBicyclesLab.Utils
{
    public class CustomUserValidator : UserValidator<User>, IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (user.UserName.Contains("admin"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Нікнейм не може містити слово 'admin'"
                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}