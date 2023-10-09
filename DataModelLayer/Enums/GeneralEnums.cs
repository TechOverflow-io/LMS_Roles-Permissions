using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLayer.Enums
{
    public enum Category
    {
        User,
        Groups
    }

    public enum PermissionType
    {
        Create,
        View,
        Update
    }

    public enum ResponseMessageType
    {
        BadRequest,
        NotFound,
        Ok
    }

    public enum Roles
    {
        SuperAdmin
    }

}
