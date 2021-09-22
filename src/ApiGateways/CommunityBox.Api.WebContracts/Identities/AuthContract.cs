namespace CommunityBox.Api.WebContracts.Identities
{
    public class AuthContract
    {
        public string UserId { get; set; }
        
        public string Email { get; set; }
        
        public string Token { get; set; }
    }
}