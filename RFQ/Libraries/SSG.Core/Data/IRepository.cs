using System.Linq;
using System.Data.Common;

namespace SSG.Core.Data
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T> where T : BaseEntity
    {
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Table { get; }
        DbDataRecord GetEntityOriginalValues(T entity);
    }
}
