namespace CommunityBox.Web.Mvc.Configurations
{
    public class GatewayClientConfig
    {
        public string BaseUrl { get; set; }

        public AuctionRoutes AuctionRoutes { get; set; }
        
        public IdentityRoutes IdentityRoutes { get; set; }
    }
}