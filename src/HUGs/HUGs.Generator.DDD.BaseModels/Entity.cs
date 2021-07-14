namespace HUGs.Generator.DDD.BaseModels
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public IId<TKey> Id { get; protected set; }
    }
}