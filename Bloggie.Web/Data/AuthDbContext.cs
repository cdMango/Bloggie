using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        //static id generated using uuidgen | tr "[A-Z]" "[a-z]"
        var superAdminRoleId = "7a958259-c1cc-45de-b4cf-ecd6593fb050";
        var adminRoleId = "0d7163ff-0bc4-444f-a19d-58326852e38f";
        var userRoleId = "fe0836d8-61c8-4471-a6cb-f938bd84a285"; 
        
        // Seed Roles (User, Admin, Super admin)
        var roles = new List<IdentityRole>
        {
            new IdentityRole()
            {
                Name = "SuperAdmin",
                NormalizedName = "SuperAdmin",
                Id = superAdminRoleId,
                ConcurrencyStamp = superAdminRoleId
            },

            new IdentityRole()
            {
                Name = "Admin",
                NormalizedName = "Admin",
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId
            },

            new IdentityRole()
            {
                Name = "User",
                NormalizedName = "User",
                Id = userRoleId,
                ConcurrencyStamp = userRoleId
            },

        };
        builder.Entity<IdentityRole>().HasData(roles);
        
        // Seed Super Admin user
        var superAdminId = "9635a733-64b2-42e3-9e13-7d14007f7928";
        var superAdminUser = new IdentityUser()
        {
            Id = superAdminId,
            UserName = "superadmin@bloggie.com",
            Email = "superadmin@bloggie.com"
        };

        superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
            .HashPassword(superAdminUser, "superadmin123");

        builder.Entity<IdentityUser>().HasData(superAdminUser); 


        // Add All Roles To Super Admin User
        var superAdminRoles = new List<IdentityUserRole<string>>()
        {
            new IdentityUserRole<string>
            {
                RoleId = superAdminRoleId,
                UserId = superAdminId
            },

            new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = superAdminId
            },

            new IdentityUserRole<string>
            {
                RoleId = userRoleId,
                UserId = superAdminId 
            },
        };

        builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
    }
}