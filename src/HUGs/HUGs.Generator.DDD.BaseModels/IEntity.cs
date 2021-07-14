namespace HUGs.Generator.DDD.BaseModels
{
    public interface IEntity<TKey>
    {
        IId<TKey> Id { get; }
    }
}