using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLayer.Models.DTOs.Request.Security
{
    public class GroupRequest
    {

        [Required]
        public string GroupName { get; set; }

        public bool IsActive { get; set; }
    }
}
