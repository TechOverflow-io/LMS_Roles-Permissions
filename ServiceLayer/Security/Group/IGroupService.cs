using DataModelLayer.Models.Common;
using DataModelLayer.Models.DTOs.Request.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Security.Group
{
    public interface IGroupService
    {
        Task<LogicResponse> Create(GroupRequest groupItem, ClaimsIdentity claimsIdentity);

        Task<LogicResponse> Update(string id, GroupRequest groupItem, ClaimsIdentity claimsIdentity);
    }
}
