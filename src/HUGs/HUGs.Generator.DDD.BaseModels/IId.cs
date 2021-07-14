namespace HUGs.Generator.DDD.BaseModels
{
    public interface IId<TKey>
    {
        TKey Value { get; }
    }
}