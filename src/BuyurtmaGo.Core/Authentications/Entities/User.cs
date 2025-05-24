using Microsoft.AspNetCore.Identity;

namespace BuyurtmaGo.Core.Authentications.Entities
{
    public class User : IdentityUser<long>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid? AvatarId { get; set; }
    }
}
