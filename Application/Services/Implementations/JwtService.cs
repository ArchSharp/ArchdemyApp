using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    internal sealed class JwtService : IJwtService
    {
        private readonly JwtParameters _jwtParameters;

        public JwtService(IOptions<JwtParameters> jwtParameters)
        {
            _jwtParameters = jwtParameters.Value;
        }

        public string CreateJwtToken(User user)
        {
            var claims = new Claim[]
            {
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Email, user.Email)
            };
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtParameters.SecretKey)),
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: _jwtParameters.Issuer,
                audience: _jwtParameters.Audience,
                claims: claims,
                null,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
