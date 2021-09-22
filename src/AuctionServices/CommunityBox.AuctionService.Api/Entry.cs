using AutoMapper;
using CommunityBox.AuctionService.Api.Mapper.Profiles;
using CommunityBox.AuctionService.DAL;
using CommunityBox.AuctionService.Domain.Abstractions;
using CommunityBox.Common.Kafka.Messages;
using CommunityBox.Common.Kafka.Producer;
using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace CommunityBox.AuctionService.Api
{
    public static class Entry
    {
        public static IServiceCollection ConfigureKafka(this IServiceCollection services, IConfiguration configuration)
        {
            var subsProducerKafkaSection = configuration.GetSection("SubsProducerKafkaConfiguration");
            services.AddKafkaProducer<Null, SubscriberMessageContent>(subsProducerKafkaSection);

            var betProducerSection = configuration.GetSection("BetProducerKafkaConfiguration");
            services.AddKafkaProducer<Null, BetMessageContent>(betProducerSection);

            services.AddKafkaMessageBus();
            
            return services;
        }

        public static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mc =>
                {
                    mc.AddProfiles(new Profile[]
                    {
                        new GrpcMapperProfile(),
                        new EntityMapperProfile()
                    });
                })
                .CreateMapper());

            return services;
        }

        public static IServiceCollection ConfigureAuctionDb(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContextPool<AuctionContext>(opt =>
                opt.UseNpgsql(
                    configuration.GetConnectionString("AuctionDbConnection")
                )
            );

            services.AddScoped<IAuctionContext, AuctionContext>();
            return services;
        }

        public static void ExecuteCardsDbMigrations(this IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<AuctionContext>();

            if (context.Database.IsInMemory())
                return;

            //logger.LogInfoObj($"{nameof(CardsContext)} migrations executing", "MigrationsExecuting");
            context.Database.Migrate();
            //logger.LogInfoObj($"{nameof(CardsContext)} migrations executed", "MigrationsExecuted");
        }
    }
}