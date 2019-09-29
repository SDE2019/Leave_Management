using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveApp.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }
    public class UserMetaData
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage ="Min 8 characters")]
        public string Password { get; set; }

        [CompareAttribute("Password", ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]

        public string ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}