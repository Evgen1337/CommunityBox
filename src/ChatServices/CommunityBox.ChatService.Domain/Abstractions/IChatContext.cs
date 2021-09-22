using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityBox.Common.Core;

namespace CommunityBox.ChatService.Domain.Abstractions
{
    public interface IChatContext
    {
        IQueryable<T> RawQueryAsync<T>(string query, params object[] parameters)
            where T : class, IRawQueryModel;
        
        /// <summary>
        ///		Updates given entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T UpdateEntity<T>(T entity) where T : class, IEntity;

        /// <summary>
        ///     Queries the entity.
        /// </summary>
        /// <typeparam name = "T"> </typeparam>
        /// <returns> </returns>
        IQueryable<T> QueryEntity<T>() where T : class, IEntity;
        
        /// <summary>
        ///     Adds the entity.
        /// </summary>
        /// <typeparam name = "T"> </typeparam>
        /// <param name = "entity"> The entity. </param>
        Task<T> AddEntityAsync<T>(T entity) where T : class, IEntity;

        /// <summary>
        ///     Adds the range.
        /// </summary>
        /// <typeparam name = "T"> </typeparam>
        /// <param name = "entity"> The entity. </param>
        Task AddRangeAsync<T>(IEnumerable<T> entity) where T : class, IEntity;

        /// <summary>
        ///     Removes the entity.
        /// </summary>
        /// <typeparam name = "T"> </typeparam>
        /// <param name = "entity"> The entity. </param>
        void RemoveEntity<T>(T entity) where T : class, IEntity;

        /// <summary>
        ///     Removes the range.
        /// </summary>
        /// <typeparam name = "T"> </typeparam>
        /// <param name = "entity"> The entity. </param>
        void RemoveRange<T>(IEnumerable<T> entity) where T : class, IEntity;

        /// <summary>
        ///     Commits the asynchronous.
        /// </summary>
        /// <returns> </returns>
        Task<int> SaveChangesAsync();
    }
}