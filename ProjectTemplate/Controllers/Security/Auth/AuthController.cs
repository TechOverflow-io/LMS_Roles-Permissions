using DataModelLayer.Models.DTOs.Request;
using DataModelLayer.Models.DTOs.Response.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Security.Auth;

namespace ProjectTemplate.Controllers.Security.Auth
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    var retObj = await _authService.Login(user);

                    if (retObj.IsSuccess)
                    {
                        return Ok(retObj.LoginInfo);
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
    }
}
