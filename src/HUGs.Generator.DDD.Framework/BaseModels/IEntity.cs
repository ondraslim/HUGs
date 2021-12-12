namespace HUGs.Generator.DDD.Framework.BaseModels
{
    public interface IEntity<TKey>
    {
        IId<TKey> Id { get; }
    }
}