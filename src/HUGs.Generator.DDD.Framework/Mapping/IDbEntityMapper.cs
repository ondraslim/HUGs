namespace HUGs.Generator.DDD.Framework.Mapping
{
    public interface IDbEntityMapper<TDddObject, TDbEntity>
    {
        /// <summary>
        /// Maps a DDD object to corresponding Db entity.
        /// </summary>
        TDbEntity ToDbEntity(TDddObject dddObject);

        /// <summary>
        /// Maps a Db entity to corresponding DDD object.
        /// </summary>
        TDddObject ToDddObject(TDbEntity dbEntity);
    }
}