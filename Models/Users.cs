using dashboard;
using message.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace user.Models{
    public class User : BaseEntity {
        public int id { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; } 
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [MinLengthAttribute(2)]
        public string first_name { get; set; } 
        [Required]

        [RegularExpression(@"^[a-zA-Z]+$")]
        [MinLengthAttribute(2, ErrorMessage = "Name must be at least 2 letters")]
        public string last_name { get; set; } 
        [Required]

        [DataTypeAttribute(DataType.Password)]
        public string password { get; set; } 
        [Required]
        [CompareAttribute("password", ErrorMessage = "Password's must match")]
        public string password_confirmation { get; set; } 
        public int admin { get; set; }
        public string description { get; set;}
        public ICollection<Messages> messages { get; set; }
    } 
}