using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveApp.Models
{
    public class UserLogin
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}