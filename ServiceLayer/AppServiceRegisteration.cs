using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Security.Auth;
using ServiceLayer.Security.Group;
using ServiceLayer.Security.User;

namespace ServiceLayer
{
    public static class AppServiceRegisteration
    {
        public static void AddAppService(this IServiceCollection services)
        {
            #region User Management / Roles / Permissions
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGroupService, GroupService>();
            #endregion



        }
    }
}
