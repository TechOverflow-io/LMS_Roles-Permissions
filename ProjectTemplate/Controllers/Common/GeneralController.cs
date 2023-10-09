using DataModelLayer.Data;
using DataModelLayer.Models.DbModels;
using DataModelLayer.Models.DTOs.Response.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ProjectTemplate.Controllers.Common
{
    [Route("[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GeneralController(UserManager<ApplicationUser> userManager, ApiDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetGeneralData()
        {
            try
            {
                var claimsIdentity = (System.Security.Claims.ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst("id");

                if (claim == null)
                {
                    return Unauthorized(new ErrorResponse()
                    {
                        ErrorMessage = "You are not logged in",
                        Success = false

                    });
                }

                var userId = claim.Value;

                var roleIdClaims = claimsIdentity.FindFirst("accval");
                var roleIds = roleIdClaims.Value;

                    List<string> roleIdList = roleIds.Split(',').ToList();
                    var allClaims = await _context.RoleClaims.Where(c => roleIdList.Contains(c.RoleId) && c.ClaimType == "Permission").Select(s => s.ClaimValue.Replace("Permissions.", "").Replace(".", "_").ToLower()).ToListAsync();

                    var retObj = new GeneralResponse()
                    {
                        Permissions = allClaims
                    };

                    return Ok(retObj);

            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    ErrorMessage = "Something went wrong " + ex.Message,
                    Success = false

                });
            }
        }
    }
}
