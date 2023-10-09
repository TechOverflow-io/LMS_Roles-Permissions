using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLayer.Models.DTOs.Response.Common
{
    public class GeneralSuccessResponse
    {
        public bool Success { get; set; }
        public string CreatedId { get; set; }
        public string Message { get; set; }
    }
}
