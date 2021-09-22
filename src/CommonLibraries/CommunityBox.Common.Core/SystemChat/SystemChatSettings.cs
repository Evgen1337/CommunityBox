using System;

namespace CommunityBox.Common.Core.SystemChat
{
    public static class SystemChatSettings
    {
        public static string SystemUserName => "Система";
        public static string SystemUserId => Guid.Parse("4571AC6E-E461-422A-90A2-5A4C55DE7567").ToString();
    }
}