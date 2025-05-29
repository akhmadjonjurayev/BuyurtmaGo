namespace BuyurtmaGo.Core.Authentications.Models
{
    public record TokenResponse(string JwtToken);

    public record UserInfoModel(long Id,
        string UserName,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string RoleName,
        Guid? AvatarId
        );
}
