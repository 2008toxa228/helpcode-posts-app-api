using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class RefreshAccessTokenRequest
    {
        public Guid UserId { get; set; }

        public string RefreshToken { get; set; }
    }
}
