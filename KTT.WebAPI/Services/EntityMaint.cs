using KTT.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTT.WebAPI.Services
{
    public class EntityMaint<T> : IEntityMaint<T>
        where T : EntityBase 
    {
        private readonly List<T> entities;

        public EntityMaint ()
        {
            entities = new List<T>();
        }

        public T Get(int id)
        {
            return entities.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return entities;
        }

        public IEnumerable<T> Search(Func<T, bool> searchExpression)
        {
            return entities.Where(searchExpression);
        }

        public T Add(T entity)
        {
            entities.Add(entity);
            return entity;
        }

        public T Update (T entity)
        {
            var foundEntity = entities.FirstOrDefault(p => p.Id == entity.Id);
            if (foundEntity == null) return null;
            foundEntity.Copy<T>(entity);
            return foundEntity;
        }

        public bool Delete(int id)
        {
            var foundEntity = entities.FirstOrDefault(p => p.Id == id);
            if (foundEntity == null) return false;
            return entities.Remove(foundEntity);
        }
    }
}
