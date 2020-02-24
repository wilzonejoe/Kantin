using Core.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Core.Helpers
{
    public class JWTHelper
    {
        private const string SecretKey = "TW9zaGVFcmV6UHJpdmF0ZUtleQ==";

        public string GenerateToken(JWTContainer model)
        {
            if (model == null || model.Claims == null || !model.Claims.Any())
                throw new UnauthorizedAccessException("Arguments to create token are not valid.");

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(model.Claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(model.ExpireMinutes)),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), model.SecurityAlgorithm)
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("Given token is null or empty.");

            var tokenValidationParameters = GetTokenValidationParameters();
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return tokenValid.Claims;
            }
            catch
            {
                throw new UnauthorizedAccessException("Given token is not valid");
            }
        }

        public bool IsTokenValid(string token)
        {
            try
            {
                GetTokenClaims(token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private SecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Convert.FromBase64String(SecretKey));
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }
    }
}
