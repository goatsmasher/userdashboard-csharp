using dashboard;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace message.Models{
    public class Messages : BaseEntity {
        public int id { get; set; }
        [MinLengthAttribute(1)]
        [RequiredAttribute]
        public string message { get; set; }
        public int user_id { get; set;}
        public int author_id {get; set;}
        public ICollection<Messages> messages { get; set; }
        // public User user { get; set; }
    }
}