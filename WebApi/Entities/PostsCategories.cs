using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class PostsCategories : EntityBase
    {
        public Guid PostId { get; set; }
        public Guid CategoryId { get; set; }

        public PostsCategories() { }

        public PostsCategories(string jsonUserModel)
        {
            var postCategory = JsonConvert.DeserializeObject<PostsCategories>(jsonUserModel);

            this.PostId = postCategory.PostId;
            this.CategoryId = postCategory.CategoryId;
        }
    }
}
