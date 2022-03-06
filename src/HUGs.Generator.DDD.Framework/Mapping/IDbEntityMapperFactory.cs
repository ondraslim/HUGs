using System;

namespace HUGs.Generator.DDD.Framework.Mapping
{
    public interface IDbEntityMapperFactory
    {
        IDbEntityMapper<TDddObject, TDbEntity> GetMapper<TDddObject, TDbEntity>();
    }

    public class DbEntityMapperFactory : IDbEntityMapperFactory
    {
        private readonly IServiceProvider serviceProvider;

        public DbEntityMapperFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IDbEntityMapper<TDddObject, TDbEntity> GetMapper<TDddObject, TDbEntity>()
            => (IDbEntityMapper<TDddObject, TDbEntity>) serviceProvider.GetService(typeof(IDbEntityMapper<TDddObject, TDbEntity>));
    }
}