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
    public interface IEntity<T>
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        T Id { get; set; }
    }
}