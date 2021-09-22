using System.Reflection;
using Autofac;
using CommunityBox.AuctionService.Api.Application.Commands;
using MediatR;

namespace CommunityBox.AuctionService.Api.AutofacModules
{
    public class MediatorModules : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(CreateAuctionCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();

                return t => componentContext.TryResolve(t, out var o)
                    ? o
                    : null;
            });
        }
    }
}