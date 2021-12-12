namespace HUGs.Generator.DDD.Framework.BaseModels
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public IId<TKey> Id { get; protected set; }
    }
}