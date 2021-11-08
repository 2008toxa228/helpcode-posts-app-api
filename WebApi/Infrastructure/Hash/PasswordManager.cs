using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.Hash
{
    public static class PasswordManager
    {
        private static string _salt;

        private static int _iterationCount;

        /// <summary>
        /// Инициализация переменных.
        /// </summary>
        /// <param name="configuration"></param>
        public static void SetConfig(IConfiguration configuration)
        {
            //var secretKeyName = configuration?.GetSection("Config:App:JwtToken:SecretKeyName")?.Value;
            //Argument.NotNullOrEmpty(secretKeyName, nameof(secretKeyName));
            //int.TryParse(configuration?.GetSection("Config:App:JwtToken:TokenLifeTime")?.Value, out _lifeTime);

            _salt = "=8VJeif^biu0KtO4h?LlJGZBx&WhKlcf";
            _iterationCount = 50000;

            // Если Dev mode, берем значение ключа шифрования из переменных окружения.
            //_secretKey = Environment.GetEnvironmentVariable(secretKeyName);

#if (!DEBUG)
            //var credentialManager = new CredentialManager(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            //_secretKey = credentialManager.GetPassword(secretKeyName);
#endif
            //Argument.NotNullOrEmpty(_secretKey, nameof(_secretKey));

        }

        /// <summary>
        /// Получить свежий токен доступа.
        /// </summary>
        /// <returns></returns>
        public static string GetHash(string password)
        {
            var hashedPassword = string.Empty;
            using (SHA512 sha512 = new SHA512Managed())
            {
                var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(password, _salt)));

                for (int i = 0; i < _iterationCount; i++)
                {
                    hash = sha512.ComputeHash(hash);
                }

                hashedPassword = BitConverter.ToString(hash).Replace("-", string.Empty);
            }

            return hashedPassword;
        }

        public static bool Verify(string password, string hashedPassword)
        {
            return hashedPassword == GetHash(password);
        }
    }
}
