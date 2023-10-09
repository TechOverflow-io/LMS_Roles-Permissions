using DataModelLayer.Models.Common;
using DataModelLayer.Models.DTOs.Request;
using DataModelLayer.Models.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Security.User
{
    public interface IUserService
    {
        public Task<LogicResponse> Create(UserRequest user, ClaimsIdentity claimsIdentity);

        public Task<IList<UserResponse>> Get(ClaimsIdentity claimsIdentity);
        public Task<UserResponse> GetById(string id, ClaimsIdentity claimsIdentity);

        public Task<LogicResponse> Update(string id, UserEditRequest item, ClaimsIdentity claimsIdentity);

    }
}
