using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Common
{
    public class CrudService<TResource, TEntity> : ICrudService<TResource>
        where TResource : IBaseResource
        where TEntity : BaseEntity
    {
        protected readonly IAppDbContext DbContext;
        protected readonly IMapper Mapper;

        public CrudService(IAppDbContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        public TResource GetById(int id)
        {
            var resource = DbContext.GetDbSet<TEntity>()
                .Where(e => e.Id == id)
                .ProjectTo<TResource>(Mapper.ConfigurationProvider)
                .FirstOrDefault();

            if (resource == null) throw new Exception(nameof(TEntity));

            return resource;
        }

        public async Task<IEnumerable<TResource>> GetAll()
        {
            return await DbContext.GetDbSet<TEntity>()
                .ProjectTo<TResource>(Mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<UpsertReplyResource> Upsert(TResource resource)
        {
            TEntity entity;

            if (resource.Id.HasValue && resource.Id.Value > 0)
            {
                entity = DbContext.GetDbSet<TEntity>().Find(resource.Id.Value);
                if (entity == null) throw new Exception(nameof(TEntity));
            }
            else
            {
                entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
                if (entity == null) throw new Exception($"{nameof(TEntity)} couldn't be created");

                DbContext.GetDbSet<TEntity>().Add(entity);
            }

            Mapper.Map(resource, entity);

            await DbContext.SaveChangesAsync();

            return new UpsertReplyResource(entity.Id);
        }

        public async Task Delete(int id)
        {
            var entity = DbContext.GetDbSet<TEntity>().Find(id);

            if (entity == null) throw new Exception(nameof(TEntity));

            entity.SoftDeleted = true;

            await DbContext.SaveChangesAsync();
        }
    }
}
