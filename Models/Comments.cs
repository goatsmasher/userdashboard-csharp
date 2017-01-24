using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using dashboard;
namespace comment.Models{
    public class Comment : BaseEntity {
        public int id { get; set; }
        [RequiredAttribute]
        [MinLengthAttribute(1)]
        public string comment { get; set; }
        public int messages_id { get; set;}
        public int author_id {get; set;}
        public ICollection<Comment> comments { get; set; }
    }
}  