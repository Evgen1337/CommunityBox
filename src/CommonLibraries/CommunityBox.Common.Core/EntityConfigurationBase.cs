using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityBox.Common.Core
{
    public abstract class EntityConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity> 
        where TEntity : class, IEntity 
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}