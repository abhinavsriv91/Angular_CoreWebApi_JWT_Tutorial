using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Tutorial.Global.DTO;

namespace Tutorial.Api.Infrastructure.JWT
{
    /// <summary>
    /// Manager class for JWT
    /// </summary>
    public class JWTManager
    {
        /// <summary>
        /// Generate JWT Token
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="secretKey"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string GenerateJWTToken(AuthenticatedUserDTO userInfo, string secretKey, double timeOut)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(WebConstants.UserName, userInfo.UserName),
                new Claim(WebConstants.FullName, userInfo.FullName),
                new Claim(WebConstants.EmailAddress, userInfo.EmailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(timeOut),
                    signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Read token from the request header
        /// </summary>
        /// <param name="authorizationToken"></param>
        public static AuthenticatedUserDTO ExtractClaimFromJWT(StringValues authorizationToken)
        {
            var token = authorizationToken.ToString().Split(' ')[1];
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            AuthenticatedUserDTO user = new AuthenticatedUserDTO()
            {
                UserName = jwtToken.Claims.First(claim => claim.Type == WebConstants.UserName).Value,
                FullName = jwtToken.Claims.First(claim => claim.Type == WebConstants.FullName).Value,
                EmailAddress = jwtToken.Claims.First(claim => claim.Type == WebConstants.EmailAddress).Value,
            };
            return user;
        }
    }
}
