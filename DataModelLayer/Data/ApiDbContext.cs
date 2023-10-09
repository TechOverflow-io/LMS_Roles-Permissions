using DataModelLayer.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLayer.Data
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, UserRoles, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
          : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });
            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            builder.Entity<UserRoles>(entity =>
            {
                entity.ToTable("UserRoles");
            });


            builder.Entity<UserRoles>()
             .HasOne(u => u.User)
             .WithMany(s => s.UserRoles)
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserRoles>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.UserRoles)
                .HasForeignKey(t => t.UserId);


            builder.Entity<UserRoles>()
                .HasOne(sc => sc.Role)
                .WithMany(s => s.UserRoles)
                .HasForeignKey(sc => sc.RoleId);

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            builder.HasDefaultSchema("tof");
        }

    }
}
