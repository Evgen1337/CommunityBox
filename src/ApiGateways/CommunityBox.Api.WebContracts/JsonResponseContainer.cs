using System.Collections.Generic;
using System.Linq;

namespace FriendlyCards.Backend.Api.Models.ViewModels
{
    public class JsonResponseContainer<T>
    {
        public T Data { get; set; }
        public IReadOnlyCollection<JsonResponseError> Errors { get; set; } = new JsonResponseError[] { };

        public bool Success => !Errors.Any();
    }
    
    public class JsonResponseContainer
    {
        public object Data { get; set; } = new object();
        public IReadOnlyCollection<JsonResponseError> Errors { get; set; } = new JsonResponseError[] { };

        public bool Success => !Errors.Any();
    }
}