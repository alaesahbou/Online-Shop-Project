﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShoppingCart.API.BusinessLogic
{
    /// <summary>
    // Summary:
    //     Provides static methods for generating and validating 'JSON Web Token' (JWT) token.
    /// </summary>
    public class JwtTokenManager
    {
        /*
         * Usually this key is not even known to the developer.
         * It is usually stored in a file that is encrypted in a network location and only the reference is available to our project.
         * But for the sake of simplicity, I am mentioning it here directly.
         */
        private const string JWT_SECRET_KEY = @"z2pyJ8V4FpqU8Nhu96AhN6VwEdW9dKbRXzGWCfynFNcBQEceUnCGBg8AcrM2PdGx";

        /// <summary>
        /// Generates a new JWT token based on the username.
        /// </summary>
        /// <param name="usernameParam">Username from which the JWT token is generated.</param>
        /// <returns>A valid 'JSON Web Token' (JWT) token</returns>
        /// <exception cref="ArgumentNullException">Exception is thrown if the username is null</exception>
        public static string GenerateToken(string usernameParam)
        {
            if (usernameParam.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(usernameParam));

            byte[] jwtBinaryKey = Convert.FromBase64String(JWT_SECRET_KEY);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(jwtBinaryKey); //Requires the NuGet package - Microsoft.IdentityModel.Tokens
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usernameParam)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler(); //Requires the NuGet package - System.IdentityModel.Tokens.Jwt
            JwtSecurityToken jwtToken = jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(jwtToken);
        }

        /// <summary>
        /// Validates a 'JSON Web Token' (JWT) token.
        /// </summary>
        /// <param name="jwtTokenParam">'JSON Web Token' (JWT)</param>
        /// <returns>Returns the username if the JWT token is valid and an empty string otherwise.</returns>
        public static string ValidateToken(string jwtTokenParam)
        {
            string username = String.Empty;
            ClaimsIdentity claimIdentity = null;

            ClaimsPrincipal claimsPrincipal = GetPrincipal(jwtTokenParam);
            if (claimsPrincipal == null)
                return username;
            try
            {
                //If the cast is not valid, then it is an invalid JWT.
                claimIdentity = (ClaimsIdentity)claimsPrincipal.Identity;
            }
            catch (Exception ex)
            {
                //ToDo - Log this information.
                string errorDetails = string.Format("Exception occurred at {0}.ValidateToken(). Error message is - {1}", typeof(JwtTokenManager), ex);
                System.Diagnostics.Debug.WriteLine(errorDetails);
                return username;
            }
            Claim usernameClaim = claimIdentity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }

        /// <summary>
        /// Returns a validation reference 'ClaimsPrincipal' after processing a 'JSON Web Token' (JWT)
        /// </summary>
        /// <param name="jwtTokenParam">'JSON Web Token' (JWT)</param>
        /// <returns>A System.Security.Claims.ClaimsPrincipal</returns>
        private static ClaimsPrincipal GetPrincipal(string jwtTokenParam)
        {
            ClaimsPrincipal claimsPrincipal = null;

            try
            {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                if (jwtSecurityTokenHandler.CanReadToken(jwtTokenParam))
                {
                    /*
                     * NOTE: Difference between ReadJwtToken and ReadToken.
                     * ReadToken will need an explicit cast to JwtSecurityToken
                     * ReadJwtToken will NOT need an explicit cast to JwtSecurityToken
                     */
                    JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwtTokenParam);
                    TokenValidationParameters tokenValidationParameters = getTokenValidationParameters();
                    SecurityToken securityToken;
                    claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(jwtTokenParam, tokenValidationParameters, out securityToken);
                }
            }
            catch (Exception ex)
            {
                //ToDo - Log this information.
                string errorDetails = string.Format("Exception occurred at {0}.GetPrincipal(). Error message is - {1}", typeof(JwtTokenManager), ex);
                System.Diagnostics.Debug.WriteLine(errorDetails);
            }

            return claimsPrincipal;
        }

        /// <summary>
        /// Returns the token validation parameters to perform JWT token validation.
        /// </summary>
        /// <returns>A Microsoft.IdentityModel.Tokens.TokenValidationParameters</returns>
        public static TokenValidationParameters getTokenValidationParameters()
        {
            byte[] jwtBinaryKey = Convert.FromBase64String(JWT_SECRET_KEY);
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true, //We have set the expire time to 30 minutes, hence we check for this token configuration.
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(jwtBinaryKey)
            };
            return tokenValidationParameters;
        }
    }
}
