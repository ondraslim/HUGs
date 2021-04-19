namespace HUGs.Generator.DDD.Common.DDD.Base
{
    public interface IEntity<TKey>
    {
        IId<TKey> Id { get; }
    }
}