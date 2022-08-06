using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MessenceComFrontEnd.Models
{
    public class ContactUsRequestModel
    {
        [StringLength(70)]
        public string FullName;

        [StringLength(70)]
        public string FirstName;

        [StringLength(70)]
        public string LastName;

        [Required]
        [EmailAddress]
        public string Email;

        [StringLength(50)]
        public string Phone;

        [Required]
        [StringLength(500)]
        public string Description;

        [Required]
        [StringLength(500)]
        public string Message;
    }
}