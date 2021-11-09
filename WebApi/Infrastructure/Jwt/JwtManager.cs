using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
//using Norbit.Crm;
//using CredentialManagerWrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;
using WebApi.Entities;
using System.Collections.Generic;
//using CredentialManagerWrapper;
//using ClaimsIdentity = Microsoft.IdentityModel.Claims.ClaimsIdentity;
//using ClaimTypes = Microsoft.IdentityModel.Claims.ClaimTypes;

namespace WebApi.Infrastructure.Jwt
{
    /// <summary>
    /// Опции механизма работы jwt токенов.
    /// </summary>
    public static class JwtManager
    {
        /// <summary>
        /// Ключ шифрования. HMAC-SHA256. Min 64-bytes recommended.
        /// </summary>
        private static string _secretKey;

        /// <summary>
        /// Издатель (любое значение.).
        /// </summary>
        public static string Issuer = "HelpCodeServer";

        /// <summary>
        /// Потребитель (любое значение).
        /// </summary>
        public static string Audience = "HelpCodeClient";

        /// <summary>
        /// Время действия токена доступа.
        /// </summary>
        private static int _accessLifeTime;

        /// <summary>
        /// Время действия токена обновления.
        /// </summary>
        private static int _refreshLifeTime;

        private static List<JwtSecurityToken> _refreshTokens;

        /// <summary>
        /// Инициализация переменных.
        /// </summary>
        /// <param name="configuration"></param>
        public static void SetConfig(IConfiguration configuration)
        {
            _refreshTokens = new List<JwtSecurityToken>();

            //var secretKeyName = configuration?.GetSection("Config:App:JwtToken:SecretKeyName")?.Value;
            //Argument.NotNullOrEmpty(secretKeyName, nameof(secretKeyName));
            //int.TryParse(configuration?.GetSection("Config:App:JwtToken:TokenLifeTime")?.Value, out _lifeTime);
            _accessLifeTime = _accessLifeTime != default 
                ? _accessLifeTime 
                : 10;

            _refreshLifeTime = _accessLifeTime != default
                ? _accessLifeTime
                : 1200;

            // Если Dev mode, берем значение ключа шифрования из переменных окружения.
            //_secretKey = Environment.GetEnvironmentVariable(secretKeyName);
            _secretKey = "mysupersecret_secretkey!123";
#if (!DEBUG)
            //var credentialManager = new CredentialManager(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            //_secretKey = credentialManager.GetPassword(secretKeyName);
#endif
            //Argument.NotNullOrEmpty(_secretKey, nameof(_secretKey));

        }

        /// <summary>
        /// Получить ключ.
        /// </summary>
        /// <returns></returns>
        public static Microsoft.IdentityModel.Tokens.SymmetricSecurityKey GetKey() =>
            new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));

        /// <summary>
        /// Проверить дату окончания действия токена.
        /// </summary>
        /// <param name="before"></param>
        /// <param name="exp"></param>
        /// <param name="token"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool ValidateTime(DateTime? before, DateTime? exp,
            Microsoft.IdentityModel.Tokens.SecurityToken token,
            Microsoft.IdentityModel.Tokens.TokenValidationParameters parameters) => exp != null && exp > DateTime.UtcNow;

        /// <summary>
        /// Валидирует передаваемый токен и возвращает результат.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool ValidateToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;

            // These need to match the values used to generate the token
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidIssuer = Issuer;
            validationParameters.ValidAudience = Audience;
            validationParameters.IssuerSigningKey = GetKey();
            validationParameters.ValidateIssuerSigningKey = true;
            validationParameters.ValidateAudience = true;
            validationParameters.ValidateLifetime = true;

            if (jwtHandler.CanReadToken(token))
            {
                try
                {
                    jwtHandler.ValidateToken(token, validationParameters, out validatedToken);

                    return true;
                }
                catch (Exception e)
                {
                    // ToDo write to logs
                }
            }

            return false;
        }

        public static IEnumerable<Claim> GetClaimsFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims;
        }

        /// <summary>
        /// Получить свежий токен доступа.
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken(User user)
        {
            // ToDo add validation
            var claimsIdentity = new[]
            {
                new System.Security.Claims.Claim("Id", user.Id.ToString()),
                new System.Security.Claims.Claim("Role", user.RoleId.ToString()),
            };

            var jwt = GenerateToken(_accessLifeTime, claimsIdentity);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        /// <summary>
        /// Получить новую сущность токена обновления.
        /// </summary>
        /// <returns></returns>
        public static RefreshToken GetRefreshToken(User user)
        {
            // ToDo add validation
            var claimsIdentity = new[]
            {
                new System.Security.Claims.Claim("Id", user.Id.ToString()),
                new System.Security.Claims.Claim("Name", "RefreshToken"),
            };

            var jwt = GenerateToken(_refreshLifeTime, claimsIdentity);

            var refreshToken = new RefreshToken()
            {
                UserId = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                ExpirationDate = jwt.ValidTo
            };
            return refreshToken; 
        }

        private static JwtSecurityToken GenerateToken(int minutesLifeTime, Claim[] claimsIdentity)
        {
            // ToDo add validation
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(minutesLifeTime);

            var jwt = new JwtSecurityToken(
                notBefore: now,
                expires: expires,
                audience: Audience,
                issuer: Issuer,
                claims: claimsIdentity,
                signingCredentials: new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256));

            return jwt;
        }
    }
}