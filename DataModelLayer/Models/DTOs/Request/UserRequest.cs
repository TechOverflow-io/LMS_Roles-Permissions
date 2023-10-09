using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLayer.Models.DTOs.Request
{
    public class UserRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class UserLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class UserEditRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        //public string StreetAddress { get; set; }
        //public string Suburb { get; set; }
        //public string State { get; set; }
        //public string PostCode { get; set; }

        [Required]
        public string GroupId { get; set; }

        public bool IsActive { get; set; }

    }

}
