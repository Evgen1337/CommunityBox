using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityBox.Common.Core
{
    /// <inheritdoc/>
    public interface IEntity : IEntity<long>
    {
    }
    
    /// <summary>
    ///     Базовая cущность
    /// </summary>
    public interface IEntity<TId>
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        TId Id { get; set; }
    }
}