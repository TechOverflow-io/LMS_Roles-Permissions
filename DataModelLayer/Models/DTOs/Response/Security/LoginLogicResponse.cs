using DataModelLayer.Configuration;
using DataModelLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLayer.Models.DTOs.Response.Security
{
    public class LoginLogicResponse
    {

        public LoginResponse LoginInfo { get; set; }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public IList<string> ErrorList { get; set; }

        public string CreatedId { get; set; }
        public ResponseMessageType MessageType { get; set; }


    }

    public class LoginResponse : AuthResult
    {
        public string Username { get; set; }

        public string Name { get; set; }
        public string UserId { get; set; }

    }
}
