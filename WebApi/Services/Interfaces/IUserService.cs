using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IUserService
    {
        public AuthenticateResponse Authenticate(AuthenticateRequest request);

        public Task<HttpStatusCode> Register(RegisterRequest request);

        AuthenticateResponse RefreshAccessToken(RefreshAccessTokenRequest request);
    }
}
