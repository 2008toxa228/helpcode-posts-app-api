using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class User : EntityBase
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public User() { }

        public User(string jsonUserModel)
        {
            var user = JsonConvert.DeserializeObject<User>(jsonUserModel);

            this.Id = user.Id;
            this.RoleId = user.RoleId;
            this.Email = user.Email;
            this.Username = user.Username;
            this.PasswordHash = user.PasswordHash;
        }
    }
}
