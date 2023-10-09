using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLayer.Models.DbModels
{
    public class UserRoles : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; } = new ApplicationUser();
        public virtual ApplicationRole Role { get; set; } = new ApplicationRole();
    }
}
