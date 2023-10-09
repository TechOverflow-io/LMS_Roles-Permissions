using DataModelLayer.Models.DTOs.Request;
using DataModelLayer.Models.DTOs.Response.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Security.Auth
{
    public interface IAuthService
    {
        Task<LoginLogicResponse> Login(UserLoginRequest user);
    }
}
