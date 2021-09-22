using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityBox.AuctionService.DAL.Configurations;
using CommunityBox.AuctionService.Domain.Abstractions;
using CommunityBox.AuctionService.Domain.Entities;
using CommunityBox.Common.Core;
using Microsoft.EntityFrameworkCore;

namespace CommunityBox.AuctionService.DAL
{
    public class AuctionContext : DbContext, IAuctionContext
    {
        public DbSet<Auction> Auctions { get; set; }

        public DbSet<Auctioneer> Auctioneers { get; set; }

        public DbSet<Lot> Lots { get; set; }

        public AuctionContext(DbContextOptions<AuctionContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AuctionConfiguration());
            builder.ApplyConfiguration(new AuctioneerConfiguration());
            builder.ApplyConfiguration(new LotConfiguration());
        }

        public IQueryable<T> QueryEntity<T>() where T : class, IEntity => Set<T>();

        public T UpdateEntity<T>(T entity) where T : class, IEntity => Update(entity).Entity;

        public async Task<T> AddEntityAsync<T>(T entity) where T : class, IEntity => (await AddAsync(entity)).Entity;

        public async Task AddRangeAsync<T>(IEnumerable<T> entity) where T : class, IEntity =>
            await Set<T>().AddRangeAsync(entity);

        public void RemoveEntity<T>(T entity) where T : class, IEntity => Remove(entity);

        public void RemoveRange<T>(IEnumerable<T> entity) where T : class, IEntity => Set<T>().RemoveRange(entity);

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
    }
}