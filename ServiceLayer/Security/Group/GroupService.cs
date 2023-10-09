using DataModelLayer.Data;
using DataModelLayer.Enums;
using DataModelLayer.Models.Common;
using DataModelLayer.Models.DbModels;
using DataModelLayer.Models.DTOs.Request.Security;
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

namespace ServiceLayer.Security.Group
{
    public class GroupService : IGroupService
    {
        #region Private Variables / Constructor

        private readonly ApiDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        /*rivate readonly DBLogger _dbLogger;*/
        public GroupService(RoleManager<ApplicationRole> roleManager, ApiDbContext context)
        {
            _context = context;
            _roleManager = roleManager;
        }

        #endregion


        #region CRUD - C

        public async Task<LogicResponse> Create(GroupRequest groupItem, ClaimsIdentity claimsIdentity)
        {
            try
            {
                if (groupItem.GroupName == "" || groupItem.GroupName.ToLower() == Roles.SuperAdmin.ToString().ToLower())
                {

                    return new LogicResponse()
                    {
                        Message = "Invalid request",
                        IsSuccess = false,
                        MessageType = ResponseMessageType.BadRequest
                    };
                }

                var existingRecord = await _context.Roles.Where(c => c.Name.ToLower() == groupItem.GroupName.ToLower()  && !c.IsDeleted).FirstOrDefaultAsync();

                if (existingRecord != null)
                {
                    return new LogicResponse()
                    {
                        Message = "Group name already exists",
                        IsSuccess = false,
                        MessageType = ResponseMessageType.BadRequest
                    };
                }
                var createId = Guid.NewGuid();
                var group = new ApplicationRole()
                {
                    Id = createId.ToString(),
                    Name = groupItem.GroupName,
                    IsActive = groupItem.IsActive,
                    NormalizedName = groupItem.GroupName.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString().ToUpper(),
                    IsDeleted = false,
                };

                await _context.Roles.AddAsync(group);

                await _context.SaveChangesAsync();


                return new LogicResponse()
                {
                    Message = "Group was successfully created",
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

        #region CRUD - U
        public async Task<LogicResponse> Update(string id, GroupRequest groupItem, ClaimsIdentity claimsIdentity)
        {
            try
            {

                var existingRecord = await _context.Roles.Where(c => c.Name == groupItem.GroupName && !c.IsDeleted).FirstOrDefaultAsync();

                if (existingRecord != null && existingRecord.Id != id)
                {
                    return new LogicResponse()
                    {
                        Message = "Group name already exists",
                        IsSuccess = false,
                        MessageType = ResponseMessageType.BadRequest
                    };
                }

                var saveRecord = await _context.Roles.Where(c => c.Id == id && !c.IsDeleted).FirstOrDefaultAsync();

                if (saveRecord == null)
                    return null;

                saveRecord.Name = groupItem.GroupName;
                saveRecord.NormalizedName = groupItem.GroupName.ToUpper();
                saveRecord.IsActive = groupItem.IsActive;

                await _context.SaveChangesAsync();


                return new LogicResponse()
                {
                    Message = "Group was successfully updated",
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
