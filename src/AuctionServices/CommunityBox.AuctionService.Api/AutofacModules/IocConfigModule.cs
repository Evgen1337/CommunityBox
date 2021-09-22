using Autofac;

namespace CommunityBox.AuctionService.Api.AutofacModules
{
    public class IocConfigModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<MediatorModules>();
        }
    }
}