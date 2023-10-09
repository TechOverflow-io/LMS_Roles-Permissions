using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DataModelLayer.Models.DbModels
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(100)")]
        public string StreetAddress { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(50)")]
        public string Suburb { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(50)")]
        public string State { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(20)")]
        public string PostCode { get; set; } = string.Empty;

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }


        [Column(TypeName = "uniqueidentifier")]
        public Guid? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }


        [Column(TypeName = "uniqueidentifier")]
        public Guid? Updatedby { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }

        public IList<UserRoles> UserRoles { get; set; } = new List<UserRoles>();

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        public Guid? ResetToken { get; set; }

    }
}
