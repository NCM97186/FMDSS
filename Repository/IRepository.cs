using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Repository
{
    public interface IRepository<TEntity> where TEntity:class
    {
        IEnumerable<TEntity> GetWithStoredProcedure(string storedprocedure, params object[] parameters);

        IEnumerable<TEntity> GetWithStoredProcedure(string storedprocedure);

         TEntity GetByID(object id);

         void Insert(TEntity entity);

         void Delete(object id);
    }
}