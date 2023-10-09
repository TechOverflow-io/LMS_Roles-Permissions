using DataModelLayer.Data;
using DataModelLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjectTemplate.Auth
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(Category item, PermissionType action, string role)
       : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { item, action, role };
        }
    }


    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        private readonly Category _item;
        private readonly PermissionType _action;
        private readonly string _role;
        private readonly ApiDbContext _dbcontext;

        public AuthorizeActionFilter(Category item, PermissionType action, string role, ApiDbContext dbcontext)
        {
            _item = item;
            _action = action;
            _role = role;
            _dbcontext = dbcontext;
        }



        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = true;

            var claimsIdentity = (System.Security.Claims.ClaimsIdentity)context.HttpContext.User.Identity;

            if (!claimsIdentity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
            }

            var userClaimId = claimsIdentity.FindFirst("id");
            var id = userClaimId.Value;

            var roleIdClaims = claimsIdentity.FindFirst("accval");
            var strRoleIds = roleIdClaims.Value;

            if (strRoleIds.Length > 0)
            {
                var claimId = $"Permissions.{_item}.{_action}";
                var rolesWithClaim = _dbcontext.RoleClaims.Where(x => x.ClaimType == "permission" && x.ClaimValue == claimId).Select(s => s.RoleId).ToList();

                List<string> roleIds = strRoleIds.Split(',').ToList();

                var existClaimRoles = roleIds.Select(i => i.ToString()).Intersect(rolesWithClaim).ToList();

                if (existClaimRoles.Count() == 0)
                {
                    isAuthorized = false;
                }

            }
            else
            {
                isAuthorized = false;
            }


            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
            }

        }
    }
}
