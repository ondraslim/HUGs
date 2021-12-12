namespace HUGs.Generator.DDD.Framework.Mapping
{
    public interface IDbEntityMapper<TDddObject, TDbEntity>
    {
        TDbEntity ToDbEntity(TDddObject dddObject);
        TDddObject ToDddObject(TDbEntity dbEntity);
    }
}