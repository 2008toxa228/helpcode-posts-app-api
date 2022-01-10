using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class Comment : EntityBase
    {
        public Guid Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public Guid PostId { get; set; }
        public int StatusCode { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }

        public Comment() { }

        public Comment(string jsonUserModel)
        {
            var comment = JsonConvert.DeserializeObject<Comment>(jsonUserModel);

            this.Id = comment.Id;
            this.OwnerUserId = comment.OwnerUserId;
            this.PostId = comment.PostId;
            this.StatusCode = comment.StatusCode;
            this.Content = comment.Content;
            this.CreationDate = comment.CreationDate;
        }
    }
}
