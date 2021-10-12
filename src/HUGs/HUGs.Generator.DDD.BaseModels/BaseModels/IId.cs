namespace HUGs.Generator.DDD.Framework.BaseModels
{
    public interface IId<TKey>
    {
        TKey Value { get; }
    }
}