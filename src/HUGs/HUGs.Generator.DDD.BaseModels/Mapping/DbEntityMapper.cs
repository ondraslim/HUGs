using System;
using HUGs.Generator.DDD.Framework.BaseModels;

namespace HUGs.Generator.DDD.Framework.Mapping
{
    public abstract class DbEntityMapper<TDddObject, TDbEntity> : IDbEntityMapper<TDddObject, TDbEntity>
    {
        protected string EnumerationToDbEntity(Enumeration e) => e.ToString();
        //protected string EnumerationToDddObject<TEnumeration>(string s) => TEnumeration.FromString(s);


        public abstract TDbEntity ToDbEntity(TDddObject dddObject);

        public abstract TDddObject ToDddObject(TDbEntity dbEntity);
    }
}