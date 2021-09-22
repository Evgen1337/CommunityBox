using System;
using CommunityBox.ChatService.DAL;
using Microsoft.EntityFrameworkCore;

namespace CommunityBox.ChatService.IntegrationTests.Infrastructure
{
    public class ChatContextFixture : ChatContext
    {
        public ChatContextFixture(DbContextOptions<ChatContext> options)
            : base(new DbContextOptionsBuilder<ChatContext>()
                .UseInMemoryDatabase(databaseName: $"TestIdentityDb{DateTime.Now}")
                .Options
            )
        {
        }
    }
}