using System.Collections.Generic;

namespace HUGs.Generator.DDD.BaseModels
{
    public interface IAggregate<TKey> : IEntity<TKey>
    {
        public string Etag { get; }
        void RegenerateEtag();
        bool Equals(object other);
        int GetHashCode();
        public IEnumerable<object> GetEvents();
    }
}