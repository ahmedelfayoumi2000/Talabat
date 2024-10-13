using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Interfaces;
using Talabat.DAL.Entities.Identity;

namespace Talabat.BLL.Services
{
    public class TokenService : ITokenService
    {
        //hold the configuration settings
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        //create a JWT token for a given user
        public async Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager)
        {

            // Create a list of claims containing user's email and display name
            var authClaims = new List<Claim>()
            {
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.GivenName , user.DisplayName )
            }; // Privvate Claims(UserDefinded)

            // Get the roles assigned to the user
            var userRoles = await userManager.GetRolesAsync(user);

            // Add each role as a claim
            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));

            // Create a security key from the configuration
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

            // Define the token(Generate Token)
            var token = new JwtSecurityToken(

                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:DurationInDayes"])),
                claims: authClaims,
                 //Determine how to generate hashing result
                 signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                 );

            // Write the token to a string and return it
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
