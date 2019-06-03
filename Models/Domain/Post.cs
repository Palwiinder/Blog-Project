using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAssignment.Models.Domain
{
    public class Post
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public bool Published { get; set; }
        public DateTime DateCreated { get; set; }
        public Post()
        {
            DateCreated = DateTime.Now;
        }
        public DateTime DateUpdated { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int PostId { get; set; }
        public string MediaUrl { get; set; }

    }
}