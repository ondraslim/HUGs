namespace HUGs.Generator.DDD.Framework.Mapping
{
    public interface IDbEntityMapperFactory
    {
        IDbEntityMapper<TDddObject, TDbEntity> GetMapper<TDddObject, TDbEntity>();
    }
}