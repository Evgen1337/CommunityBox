using System;

namespace CommunityBox.IdentityService.AuthService.Configurations
{
    public interface IJwtConfiguration
    {
        public TimeSpan LifeTime { get; }

        public string Key { get; }
    }
}