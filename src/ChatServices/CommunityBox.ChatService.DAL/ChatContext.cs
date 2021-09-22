using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityBox.ChatService.DAL.Configurations;
using CommunityBox.ChatService.Domain.Abstractions;
using CommunityBox.ChatService.Domain.Entities;
using CommunityBox.ChatService.Domain.Queries;
using CommunityBox.Common.Core;
using Microsoft.EntityFrameworkCore;

namespace CommunityBox.ChatService.DAL
{
    public class ChatContext : DbContext, IChatContext
    {
        public DbSet<Chat> Chats { get; set; }

        public DbSet<ChatUser> ChatUsers { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<GetChatsQueryResult> GetChatsQueryResults { get; set; }

        public ChatContext(DbContextOptions<ChatContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ChatUserConfiguration());
            builder.ApplyConfiguration(new MessageConfiguration());

            builder.Entity<GetChatsQueryResult>().HasNoKey();

            base.OnModelCreating(builder);
        }

        public IQueryable<T> QueryEntity<T>() where T : class, IEntity => Set<T>();

        public IQueryable<T> RawQueryAsync<T>(string query, params object[] parameters)
            where T : class, IRawQueryModel =>
            Set<T>().FromSqlRaw(query, parameters ?? Array.Empty<object>()).AsNoTracking();

        public T UpdateEntity<T>(T entity) where T : class, IEntity => Update(entity).Entity;

        public async Task<T> AddEntityAsync<T>(T entity) where T : class, IEntity => (await AddAsync(entity)).Entity;

        public async Task AddRangeAsync<T>(IEnumerable<T> entity) where T : class, IEntity =>
            await Set<T>().AddRangeAsync(entity);

        public void RemoveEntity<T>(T entity) where T : class, IEntity => Remove(entity);

        public void RemoveRange<T>(IEnumerable<T> entity) where T : class, IEntity => Set<T>().RemoveRange(entity);

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
    }
}