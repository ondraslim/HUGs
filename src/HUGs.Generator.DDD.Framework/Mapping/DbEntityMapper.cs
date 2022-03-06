using HUGs.Generator.DDD.Framework.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HUGs.Generator.DDD.Framework.Mapping
{
    public abstract class DbEntityMapper<TDddObject, TDbEntity> : IDbEntityMapper<TDddObject, TDbEntity>
    {
        private readonly IDbEntityMapperFactory factory;

        protected DbEntityMapper(IDbEntityMapperFactory factory)
        {
            this.factory = factory;
        }

        protected string ToDbEntityEnumeration(Enumeration e) => e.ToString();

        protected TEnumeration ToDddObjectEnumeration<TEnumeration>(string e)
            where TEnumeration : Enumeration
        {
            var result = typeof(TEnumeration)
                .GetMethod("FromString", BindingFlags.Static | BindingFlags.Public)!
                .Invoke(null, new object[] { e });
            return (TEnumeration)result;
        } 

        protected Guid ToDbEntityId(IId<Guid> id) => id.Value;

        protected TId ToDddObjectId<TId>(Guid id) => (TId)Activator.CreateInstance(typeof(TId), id);

        protected ICollection<TChildDbEntity> ToDbEntityCollection<TChildDddObject, TChildDbEntity>(IEnumerable<TChildDddObject> collection)
        {
            var mapper = factory.GetMapper<TChildDddObject, TChildDbEntity>();
            return collection.Select(mapper.ToDbEntity).ToList();
        }

        protected ICollection<TChildDddObject> ToDddObjectCollection<TChildDbEntity, TChildDddObject>(IEnumerable<TChildDbEntity> collection)
        {
            var mapper = factory.GetMapper<TChildDddObject, TChildDbEntity>();
            return collection.Select(mapper.ToDddObject).ToList();
        }

        protected TChildDbEntity ToChildDbEntity<TChildDddObject, TChildDbEntity>(TChildDddObject child)
        {
            var mapper = factory.GetMapper<TChildDddObject, TChildDbEntity>();
            return mapper.ToDbEntity(child);
        }

        protected TChildDddObject ToChildDddObject<TChildDbEntity, TChildDddObject>(TChildDbEntity child)
        {
            var mapper = factory.GetMapper<TChildDddObject, TChildDbEntity>();
            return mapper.ToDddObject(child);
        }


        public abstract TDbEntity ToDbEntity(TDddObject dddObject);

        public abstract TDddObject ToDddObject(TDbEntity dbEntity);
    }
}