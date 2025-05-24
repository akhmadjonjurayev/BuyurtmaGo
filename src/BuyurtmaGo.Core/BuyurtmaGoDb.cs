using BuyurtmaGo.Core.Authentications.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BuyurtmaGo.Core
{
    public class BuyurtmaGoDb : IdentityDbContext<User, Role, long>
    {
        public BuyurtmaGoDb(DbContextOptions<BuyurtmaGoDb> options) : base(options)
        {
        }
    }
}
