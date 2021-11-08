using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApi.Models
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int Reputation { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public AuthenticateResponse() { }

        public AuthenticateResponse(User user, string accessToken, string refreshToken)
        {
            this.Id = user.Id;
            this.RoleId = user.RoleId;
            this.Email = user.Email;
            this.Reputation = user.Reputation;
            this.UserName = user.Username;

            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
        }
    }
}
