using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MessiahSolutionsComFrontEnd.Models
{
    public class ContactUsRequestModel
    {
        
        [Required]
        [StringLength(70)]
        public string FullName;

        [Required]
        [EmailAddress]
        public string Email;

        [Required]
        [StringLength(500)]
        public string Description;

        //public string fromUrl; 
    }
}