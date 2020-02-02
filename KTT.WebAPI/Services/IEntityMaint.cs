using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTT.WebAPI.Services
{
    public interface IEntityMaint<T>
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Search(Func<T, bool> searchExpression);
        T Add(T entity);
        T Update(T entity);
        bool Delete(int id);
    }
}
