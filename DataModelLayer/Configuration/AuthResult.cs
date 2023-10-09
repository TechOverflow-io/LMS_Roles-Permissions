using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLayer.Configuration
{
    public class AuthResult
    {
        public string? AccessToken { get; set; }
        public bool Success { get; set; }
    }
}
