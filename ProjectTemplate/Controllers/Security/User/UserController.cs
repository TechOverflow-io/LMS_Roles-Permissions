using DataModelLayer.Enums;
using DataModelLayer.Models.DTOs.Request;
using DataModelLayer.Models.DTOs.Response.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTemplate.Auth;
using ServiceLayer.Security.User;

namespace ProjectTemplate.Controllers.Security.User
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Constructor / Services
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region CRUD - C
        [HttpPost]
        [Authorize(Category.User, PermissionType.Create, "")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var claimsIdentity = (System.Security.Claims.ClaimsIdentity)this.User.Identity;

                    var retObj = await _userService.Create(user, claimsIdentity);
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

        #region CRUD - R

        [HttpGet]
        [Authorize(Category.User, PermissionType.View, "")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var claimsIdentity = (System.Security.Claims.ClaimsIdentity)this.User.Identity;

                var retObj = await _userService.Get(claimsIdentity);

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

        [HttpGet("{id}")]
        [Authorize(Category.User, PermissionType.View, "")]
        public async Task<IActionResult> GetUser(string id)
        {

            try
            {

                var claimsIdentity = (System.Security.Claims.ClaimsIdentity)this.User.Identity;

                var retObj = await _userService.GetById(id, claimsIdentity);

                if (retObj == null)
                {
                    return BadRequest(new ErrorResponse()
                    {
                        ErrorMessage = "Invalid request",
                        Success = false

                    });
                }
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
        #endregion


        #region CRUD - U
        [HttpPut("{id}")]
        [Authorize(Category.User, PermissionType.Create, "")]
        public async Task<IActionResult> UpdateUser(string id, UserEditRequest item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Request.Headers.TryGetValue("accessid", out var companyId);

                    if (String.IsNullOrEmpty(companyId))
                    {
                        return BadRequest(new ErrorResponse()
                        {
                            ErrorMessage = "Invalid request",
                            Success = false

                        });
                    }

                    if (id != item.Id)
                        return BadRequest();


                    var claimsIdentity = (System.Security.Claims.ClaimsIdentity)this.User.Identity;

                    var retObj = await _userService.Update(id, item,claimsIdentity);

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
