using System.Collections.Generic;

namespace FriendlyCards.Backend.Api.Models.ViewModels
{
    public class CollectionContainerViewModel<T>
    {
        public IReadOnlyCollection<T> Items { get; set; }

        public long TotalCount { get; set; }
    }
}