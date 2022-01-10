using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class RefreshToken : EntityBase
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }

        public RefreshToken() { }

        public RefreshToken(string jsonUserModel)
        {
            var token = JsonConvert.DeserializeObject<RefreshToken>(jsonUserModel);

            this.Id = token.Id;
            this.Token = token.Token;
            this.ExpirationDate = token.ExpirationDate;
        }
    }
}
