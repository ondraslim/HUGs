namespace HUGs.Generator.DDD.Common.DDD.Base
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public IId<TKey> Id { get; protected set; }
    }
}