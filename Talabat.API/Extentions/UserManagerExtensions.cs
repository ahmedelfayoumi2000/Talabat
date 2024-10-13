using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.DAL.Entities.Identity;

namespace Talabat.API.Extentions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUserWithAddressByEmailAsync(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            var User = await userManager.Users.Include(U => U.Address).SingleOrDefaultAsync(U => U.Email == email);
            return User;
        }
    }
}
