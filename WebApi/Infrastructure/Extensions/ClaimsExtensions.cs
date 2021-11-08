using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetValueByType(this IEnumerable<Claim> claims, string type)
        {
            return claims
                .Where(x => x.Type == type)
                .FirstOrDefault()
                ?.Value;
        }
    }
}
