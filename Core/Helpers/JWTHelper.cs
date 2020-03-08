using Core.Exceptions;
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
        public static readonly string SecretKey = "TW9zaGVFcmV6UHJpdmF0ZUtleQ==";

        private static JWTHelper _instance;
        public static JWTHelper Instance => _instance ?? (_instance = new JWTHelper());

        private JWTHelper() { }

        public string GenerateToken(JWTContainer model)
        {
            if (model == null || model.Claims == null || !model.Claims.Any())
                throw new UnauthorizedException("Arguments to create token are not valid.");

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
                throw new UnauthorizedException("Given token is null or empty.");

            var tokenValidationParameters = GetTokenValidationParameters();
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return tokenValid.Claims;
            }
            catch
            {
                throw new UnauthorizedException("Given token is not valid");
            }
        }

        public string GetValueFromClaims(IEnumerable<Claim> claims, string key)
        {
            if (claims != null && claims.Any())
            {
                var claim = claims.FirstOrDefault(c => c.Type == key);

                if (claim != null)
                    return claim.Value;
            }

            throw new UnauthorizedException("Given token is incomplete or invalid");
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

        public SecurityKey GetSymmetricSecurityKey()
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
