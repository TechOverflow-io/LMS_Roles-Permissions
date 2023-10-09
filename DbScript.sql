-- Inserting a new role 'SuperAdmin' into the Roles table
INSERT INTO [ProjectTemplateDb].[tof].[Roles] ([Id],[IsActive], [IsDeleted], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES ('259CFE21-92AA-4E04-88FF-E4ED4E98AAE8',1, 0, 'SuperAdmin', 'SUPERADMIN', NEWID());

INSERT INTO [ProjectTemplateDb].[tof].[Roles] ([Id],[IsActive], [IsDeleted], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES (NEWID(),1, 0, 'Admin', 'ADMIN', NEWID());

INSERT INTO [ProjectTemplateDb].[tof].[Roles] ([Id],[IsActive], [IsDeleted], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES (NEWID(),1, 0, 'User', 'USER', NEWID());


INSERT INTO [ProjectTemplateDb].[tof].[RoleClaims] ([RoleId],[ClaimType], [ClaimValue])
VALUES 
		('259CFE21-92AA-4E04-88FF-E4ED4E98AAE8','Permission','Permissions.User.View'),
		('259CFE21-92AA-4E04-88FF-E4ED4E98AAE8','Permission','Permissions.User.Create'),
		('259CFE21-92AA-4E04-88FF-E4ED4E98AAE8','Permission','Permissions.Groups.Update'),
		('259CFE21-92AA-4E04-88FF-E4ED4E98AAE8','Permission','Permissions.Groups.Create');