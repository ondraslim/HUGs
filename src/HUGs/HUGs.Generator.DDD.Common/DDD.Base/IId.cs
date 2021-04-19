namespace HUGs.Generator.DDD.Common.DDD.Base
{
    public interface IId<TKey>
    {
        TKey Value { get; }
    }
}