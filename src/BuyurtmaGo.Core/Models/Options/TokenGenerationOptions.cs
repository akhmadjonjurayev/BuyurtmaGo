namespace BuyurtmaGo.Core.Models.Options
{
    public class TokenGenerationOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        /// <summary>
        /// From minutes
        /// </summary>
        public int Expiration { get; set; }

        public int RefreshTokenExpireInDays { get; set; }

        public string Secret { get; set; }
    }
}
