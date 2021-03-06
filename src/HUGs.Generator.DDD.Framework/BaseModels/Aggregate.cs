using System;
using System.Collections.Generic;

namespace HUGs.Generator.DDD.Framework.BaseModels
{
    public abstract class Aggregate<TKey> : Entity<TKey>, IAggregate<TKey>
    {
        protected Aggregate()
        {
            Events = new List<object>();
        }

        public string Etag { get; protected set; }

        public void RegenerateEtag()
        {
            Etag = Guid.NewGuid().ToString();
        }

        private List<object> Events { get; set; }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected void AddEvent(object @event)
        {
            Events ??= new List<object>();
            Events.Add(@event);
        }

        public IEnumerable<object> GetEvents()
        {
            return Events ?? new List<object>();
        }
    }
}