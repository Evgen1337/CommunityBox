using System;
using CommunityBox.IdentityService.Api.Databases;
using Microsoft.EntityFrameworkCore;

namespace CommunityBox.IdentityService.IntegrationTests.Infrastructure
{
    public class IdentityContextFixture : IdentityContext
    {
        public IdentityContextFixture(DbContextOptions<IdentityContext> options)
            : base(new DbContextOptionsBuilder<IdentityContext>()
                .UseInMemoryDatabase(databaseName: $"TestIdentityDb{DateTime.Now}")
                .Options
            )
        {
        }
    }
}