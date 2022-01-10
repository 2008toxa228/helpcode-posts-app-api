using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class Category : EntityBase
    {
        public Guid Id { get; set; }
        public int StatusCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Category() { }

        public Category(string jsonUserModel)
        {
            var category = JsonConvert.DeserializeObject<Category>(jsonUserModel);

            this.Id = category.Id;
            this.StatusCode = category.StatusCode;
            this.Name = category.Name;
            this.Description = category.Description;
        }
    }
}
