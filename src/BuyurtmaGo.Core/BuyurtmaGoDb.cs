using BuyurtmaGo.Core.Authentications.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BuyurtmaGo.Core
{
    public class BuyurtmaGoDb : IdentityDbContext<User, Role, long>
    {
        public BuyurtmaGoDb(DbContextOptions<BuyurtmaGoDb> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("users");
            builder.Entity<IdentityUserToken<long>>().ToTable("user_tokens");
            builder.Entity<IdentityUserLogin<long>>().ToTable("user_logins");
            builder.Entity<IdentityUserClaim<long>>().ToTable("user_claims");
            builder.Entity<Role>().ToTable("roles");
            builder.Entity<IdentityUserRole<long>>().ToTable("user_roles");
            builder.Entity<IdentityRoleClaim<long>>().ToTable("role_claims");
        }
    }
}
