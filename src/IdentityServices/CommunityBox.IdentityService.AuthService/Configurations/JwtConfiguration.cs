using System;

namespace CommunityBox.IdentityService.AuthService.Configurations
{
    public class JwtConfiguration
    {
        public TimeSpan LifeTime { get; set; }

        public string Key { get; set; }
    }
}