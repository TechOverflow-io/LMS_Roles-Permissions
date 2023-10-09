using Azure;
using DataModelLayer.Data;
using DataModelLayer.Enums;
using DataModelLayer.Models.Common;
using DataModelLayer.Models.DbModels;
using DataModelLayer.Models.DTOs.Request;
using DataModelLayer.Models.DTOs.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ServiceLayer.Security.User
{

    #region Constructor / Private Members
    public class UserService : IUserService
    {
        private readonly ApiDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager, ApiDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        #endregion

        #region CRUD - C
        public async Task<LogicResponse> Create(UserRequest user, ClaimsIdentity claimsIdentity)
        {
            try
            {
                var userClaimId = claimsIdentity.FindFirst("id");
                var ClaimId = userClaimId.Value;

                var createdId = Guid.NewGuid().ToString();
                var existingUser = await _context.Users.Where(c => c.UserName == user.Email && !c.IsDeleted).AnyAsync();

                if (existingUser)
                {
                    return new LogicResponse()
                    {
                        Message = "Email already exists",
                        IsSuccess = false,
                        MessageType = ResponseMessageType.BadRequest
                    };
                }

                var newUser = new ApplicationUser()
                {
                    Id = createdId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    //StreetAddress = user.StreetAddress,
                    //Suburb = user.Suburb,
                    //PostCode = user.PostCode,
                    //State = user.State,
                    IsActive = user.IsActive,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = Guid.Parse(ClaimId),
                };

                var roleObj = await _context.Roles.Where(x => !x.IsDeleted && x.Name.ToLower() == user.Role.ToLower()).ToListAsync();
                if (roleObj.Count == 0 && roleObj != null)
                {
                    return new LogicResponse()
                    {
                        Message = "Role not allowed",
                        MessageType = ResponseMessageType.BadRequest,
                        IsSuccess = false
                    };
                }

                newUser.UserRoles = roleObj.Select(o => new UserRoles
                {
                    Role = o
                }).ToList();

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    await _context.SaveChangesAsync();

                    return new LogicResponse()
                    {
                        Message = "User was successfully created",
                        IsSuccess = true,
                        MessageType = ResponseMessageType.Ok
                    };
                }
                else
                {
                    return new LogicResponse()
                    {
                        Message = isCreated.Errors.Select(x => x.Description).FirstOrDefault(),
                        MessageType = ResponseMessageType.BadRequest,
                        IsSuccess = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new LogicResponse()
                {
                    Message = "Something went wrong " + ex.Message,
                    MessageType = ResponseMessageType.BadRequest,
                    IsSuccess = false
                };
            }
        }

        #endregion

        #region CRUD - R

        public async Task<IList<UserResponse>> Get(ClaimsIdentity claimsIdentity)
        {
            var userClaimId = claimsIdentity.FindFirst("id");
            var ClaimId = userClaimId.Value;


            var obj = await _context.Users
                   .Where(x => !x.IsDeleted && x.CreatedBy == Guid.Parse(ClaimId))
                   .Include(i => i.UserRoles)
                   .Where(w => w.UserRoles.Any(ur => ur.Role.Name.ToLower() != "superadmin"))
                   .ToListAsync();


            var retObj = obj.Select(o => new UserResponse()
            {
                Id = o.Id,
                FirstName = o.FirstName,
                LastName = o.LastName,
                Email = o.Email,
                Username = o.UserName,
                PhoneNumber = o.PhoneNumber,
                //StreetAddress = o.StreetAddress,
                //Suburb = o.Suburb,
                //PostCode = o.PostCode,
                //State = o.State,
                IsActive = o.IsActive,

            }).OrderBy(c => c.FirstName).ToList();

            return retObj;
        }

        public async Task<UserResponse> GetById(string id, ClaimsIdentity claimsIdentity)
        {
            // do all the dropdowns data here
            //if (Operation.IS_NEW.ToString() == id)
            //{

            //    var newObj = new UserResponse()
            //    {
            //        Id = "",
            //        FirstName = "",
            //        LastName = "",
            //        Email = "",
            //        Username = "",
            //        PhoneNumber = "",
            //        GroupId = "",
            //        //StreetAddress = "",
            //        //Suburb = "",
            //        //PostCode = "",
            //        //State = "",
            //        IsActive = true,
            //        Groups = await GetGroups(claimsIdentity),
            //        ClientUsers = await GetClientUsers("", companyId)
            //    };

            //    return newObj;
            //}

            var obj = await _context.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (obj == null)
                return null;

            var groupId = obj.UserRoles.FirstOrDefault() != null ? obj.UserRoles.FirstOrDefault().RoleId : "";

            var retObj = new UserResponse()
            {
                Id = obj.Id,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                Username = obj.UserName,
                PhoneNumber = obj.PhoneNumber,

                //StreetAddress = obj.StreetAddress,
                //Suburb = obj.Suburb,
                //PostCode = obj.PostCode,
                //State = obj.State,
                IsActive = obj.IsActive,
            };

            return retObj;
        }

        #endregion

        #region CRUD - U
        public async Task<LogicResponse> Update(string id, UserEditRequest item, ClaimsIdentity claimsIdentity)
        {
            try
            {
                var existItem = await _context.Users.Include(i => i.UserRoles).SingleAsync(c => c.Id == id && !c.IsDeleted);

                if (existItem == null)
                {

                    return new LogicResponse()
                    {
                        Message = "Invalid user",
                        IsSuccess = false,
                        MessageType = ResponseMessageType.NotFound
                    };
                }

                existItem.FirstName = item.FirstName;
                existItem.LastName = item.LastName;
                existItem.PhoneNumber = item.PhoneNumber;
                //existItem.Suburb = item.Suburb;
                //existItem.StreetAddress = item.StreetAddress;
                //existItem.State = item.State;
                //existItem.PostCode = item.PostCode;
                existItem.IsActive = item.IsActive;

                if (item.GroupId != null)
                {
                    var roleObj = await _context.Roles.Where(x => x.Id == item.GroupId && !x.IsDeleted).ToListAsync();

                    if (roleObj.Count > 0)
                    {

                        if (existItem.UserRoles.Count > 0)
                        {
                            existItem.UserRoles.RemoveAt(0);
                        }

                        existItem.UserRoles = roleObj.Select(o => new UserRoles
                        {
                            Role = o
                        }).ToList();
                    }

                }

                await _context.SaveChangesAsync();

                return new LogicResponse()
                {
                    Message = "User was successfully updated",
                    IsSuccess = true,
                    MessageType = ResponseMessageType.Ok
                };

            }
            catch (Exception ex)
            {
                return new LogicResponse()
                {
                    Message = "Something went wrong " + ex.Message,
                    MessageType = ResponseMessageType.BadRequest,
                    IsSuccess = false
                };
            }
        }
        #endregion
    }
}
