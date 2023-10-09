using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLayer.Models.DTOs.Response.Common
{
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public IList<string> ErrorList { get; set; }
    }
}
