using DataModelLayer.Enums;
using DataModelLayer.Models.DTOs.Request.Security;
using DataModelLayer.Models.DTOs.Response.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTemplate.Auth;
using ServiceLayer.Security.Group;

namespace ProjectTemplate.Controllers.Security.Group
{
    [Route("[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        #region private variables / Constructor
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        #endregion

        #region CRUD - C
        [HttpPost]
        [Authorize(Category.Groups, PermissionType.Create, "")]
        public async Task<IActionResult> CreateGroup([FromBody] GroupRequest groupItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var claimsIdentity = (System.Security.Claims.ClaimsIdentity)this.User.Identity;

                    var retObj = await _groupService.Create(groupItem, claimsIdentity);

                    if (retObj.IsSuccess)
                    {
                        return Ok(new GeneralSuccessResponse()
                        {
                            Success = true,
                            Message = retObj.Message
                        });
                    }
                    else
                    {
                        return BadRequest(new ErrorResponse()
                        {
                            ErrorMessage = retObj.Message,
                            Success = false
                        });
                    }
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

            return BadRequest(new ErrorResponse()
            {
                ErrorMessage = "Invalid request",
                Success = false
            });
        }
        #endregion

        #region CRUD - U 
        [HttpPut("{id}")]
        [Authorize(Category.Groups, PermissionType.Update, "")]
        public async Task<IActionResult> UpdateGroup(string id, GroupRequest groupItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var claimsIdentity = (System.Security.Claims.ClaimsIdentity)this.User.Identity;

                    var retObj = await _groupService.Update(id, groupItem, claimsIdentity);

                    if (retObj.IsSuccess)
                    {
                        return Ok(new GeneralSuccessResponse()
                        {
                            Success = true,
                            Message = retObj.Message
                        });
                    }
                    else
                    {
                        return BadRequest(new ErrorResponse()
                        {
                            ErrorMessage = retObj.Message,
                            Success = false
                        });
                    }
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

            return BadRequest(new ErrorResponse()
            {
                ErrorMessage = "Invalid request",
                Success = false
            });

        }
        #endregion

    }
}
