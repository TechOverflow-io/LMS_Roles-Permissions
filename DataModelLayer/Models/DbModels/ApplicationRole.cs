using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataModelLayer.Models.DbModels
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string roleName) : base(roleName)
        {

        }

        public ApplicationRole(string roleName, bool isActive) : base(roleName)
        {
            IsActive = isActive;

        }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public IList<UserRoles> UserRoles { get; set; }
    }

}
