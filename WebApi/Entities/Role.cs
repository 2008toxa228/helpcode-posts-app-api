using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class Role : EntityBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Role() { }

        public Role(string jsonUserModel)
        {
            var role = JsonConvert.DeserializeObject<Role>(jsonUserModel);

            this.Id = role.Id;
            this.Name = role.Name;
            this.Description = role.Description;
        }
    }
}
