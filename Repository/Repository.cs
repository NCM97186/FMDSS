using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using FMDSS.Models.FmdssContext;
using FMDSS.Repository;

namespace FMDSS.Repository
{
  public class Repository<TEntity>:IRepository<TEntity> where TEntity : class
    //public class Repository<TEntity> where TEntity : class
    {
        private FmdssContext dbContext;
        private DbSet<TEntity> dbSet;
        public Repository()
        {
           dbContext = new FmdssContext();
           this.dbSet = dbContext.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetWithStoredProcedure(string storedprocedure, params object[] parameters)
        {

            return dbContext.Database.SqlQuery<TEntity>(storedprocedure, parameters);        
        }

        public virtual IEnumerable<TEntity> GetWithStoredProcedure(string storedprocedure)
        {
            return dbContext.Database.SqlQuery<TEntity>(storedprocedure).ToList(); 
        }
      
        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);           
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }      
    }
}