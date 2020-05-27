using System;
using System.Collections.Generic;

namespace dotnetfundaGenericRepositoryEF.Models
{
    public partial class Post 
    {
        public Post()
        {
            this.Tags = new HashSet<Tag>();
        }
        public Int64 PostId { get; set; }

        public string Author { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastmodifiedDate { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}