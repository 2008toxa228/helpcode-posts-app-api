using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class Post : EntityBase
    {
        public Guid Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public int StatusCode { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int ViewsCount { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ClosedDate { get; set; }

        public Post() { }

        public Post(string jsonUserModel)
        {
            var post = JsonConvert.DeserializeObject<Post>(jsonUserModel);

            this.Id = post.Id;
            this.OwnerUserId = post.OwnerUserId;
            this.StatusCode = post.StatusCode;
            this.Title = post.Title;
            this.Content = post.Content;
            this.ViewsCount = post.ViewsCount;
            this.CreationDate = post.CreationDate;
            this.ClosedDate = post.ClosedDate;
        }
    }
}
