using CommunityBox.ChatService.Api.Clients;
using CommunityBox.ChatService.Api.Services.Kafka;
using CommunityBox.ChatService.DAL;
using CommunityBox.ChatService.Domain.Abstractions;
using CommunityBox.Common.Kafka.Consumer;
using CommunityBox.Common.Kafka.Messages;
using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBox.ChatService.Api
{
    public static class Entry
    {
        public static IServiceCollection ConfigureKafka(this IServiceCollection services, IConfiguration configuration)
        {
            var subsConsumerKafkaSection = configuration.GetSection("SubsConsumerKafkaConfiguration");
            services.AddKafkaConsumer<Null, SubscriberMessageContent, SubscriberMessageHandler>(subsConsumerKafkaSection);

            var betProducerSection = configuration.GetSection("BetConsumerKafkaConfiguration");
            services.AddKafkaConsumer<Null, BetMessageContent, BetMessageHandler>(betProducerSection);
            
            return services;
        }
        
        public static IServiceCollection ConfigureAuctionDb(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContextPool<ChatContext>(opt =>
                opt.UseNpgsql(
                    configuration.GetConnectionString("ChatDbConnection")
                )
            );

            services.AddScoped<IChatContext, ChatContext>();
            return services;
        }

        public static IServiceCollection ConfigureClients(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection(nameof(IdentityServiceConfig)).Get<IdentityServiceConfig>());
            services.AddSingleton<IIdentityClient, IdentityClient>();
            
            return services;
        }

        public static void ExecuteCardsDbMigrations(this IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<ChatContext>();

            if (context.Database.IsInMemory())
                return;

            //logger.LogInfoObj($"{nameof(CardsContext)} migrations executing", "MigrationsExecuting");
            context.Database.Migrate();
            //logger.LogInfoObj($"{nameof(CardsContext)} migrations executed", "MigrationsExecuted");
        }
    }
}