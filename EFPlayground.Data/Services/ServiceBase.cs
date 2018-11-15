using EFPlayground.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFPlayground.Data.Services
{
    public abstract class ServiceBase<TEntity>
        where TEntity : ModelBase
    {
        #region Method - Get
        public static TEntity Get(Guid id, QueryBuilder<TEntity> queryBuilder = null)
        {
            queryBuilder = queryBuilder ?? new QueryBuilder<TEntity>();

            if (queryBuilder.WhereConditions.Count > 0)
            {
                throw new InvalidOperationException();
            }

            queryBuilder.WhereConditions.Add(x => x.Id == id);

            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                dbSet = AddIncludes(dbSet, queryBuilder);
                dbSet = AddWhereConditions(dbSet, queryBuilder);

                return dbSet.FirstOrDefault() ?? default(TEntity);
            }
        }

        public static async Task<TEntity> GetAsync(Guid id, QueryBuilder<TEntity> queryBuilder = null)
        {
            queryBuilder = queryBuilder ?? new QueryBuilder<TEntity>();

            if (queryBuilder.WhereConditions.Count > 0)
            {
                throw new InvalidOperationException();
            }

            queryBuilder.WhereConditions.Add(x => x.Id == id);

            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                dbSet = AddIncludes(dbSet, queryBuilder);
                dbSet = AddWhereConditions(dbSet, queryBuilder);

                return await dbSet.FirstOrDefaultAsync() ?? default(TEntity);
            }
        }
        #endregion

        #region Method - GetCollection
        public static IList<TEntity> GetCollection(QueryBuilder<TEntity> queryBuilder = null)
        {
            queryBuilder = queryBuilder ?? new QueryBuilder<TEntity>();

            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                dbSet = AddIncludes(dbSet, queryBuilder);
                dbSet = BuildCollectionQuery(dbSet, queryBuilder);

                return dbSet.ToList<TEntity>();
            }
        }

        public static async Task<IList<TEntity>> GetCollectionAsync(QueryBuilder<TEntity> queryBuilder = null)
        {
            queryBuilder = queryBuilder ?? new QueryBuilder<TEntity>();

            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                dbSet = AddIncludes(dbSet, queryBuilder);
                dbSet = BuildCollectionQuery(dbSet, queryBuilder);

                return await dbSet.ToListAsync<TEntity>();
            }
        }
        #endregion

        #region Method - Select
        public static dynamic Select(Guid id, Expression<Func<TEntity, object>> select, QueryBuilder<TEntity> queryBuilder = null)
        {
            queryBuilder = queryBuilder ?? new QueryBuilder<TEntity>();

            if (queryBuilder.WhereConditions.Count > 0)
            {
                throw new InvalidOperationException();
            }

            queryBuilder.WhereConditions.Add(x => x.Id == id);

            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                dbSet = AddIncludes(dbSet, queryBuilder);
                dbSet = AddWhereConditions(dbSet, queryBuilder);

                return dbSet.Select(select).FirstOrDefault() ?? default(TEntity);
            }
        }

        public static async Task<dynamic> SelectAsync(Guid id, Expression<Func<TEntity, object>> select, QueryBuilder<TEntity> queryBuilder = null)
        {
            queryBuilder = queryBuilder ?? new QueryBuilder<TEntity>();

            if (queryBuilder.WhereConditions.Count > 0)
            {
                throw new InvalidOperationException();
            }

            queryBuilder.WhereConditions.Add(x => x.Id == id);

            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                dbSet = AddIncludes(dbSet, queryBuilder);
                dbSet = AddWhereConditions(dbSet, queryBuilder);

                return await dbSet.Select(select).FirstOrDefaultAsync() ?? default(TEntity);
            }
        }
        #endregion

        #region Method - SelectCollection
        public static IList<dynamic> SelectCollection(Expression<Func<TEntity, object>> select, QueryBuilder<TEntity> queryBuilder = null)
        {
            queryBuilder = queryBuilder ?? new QueryBuilder<TEntity>();

            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                dbSet = AddIncludes(dbSet, queryBuilder);
                dbSet = BuildCollectionQuery(dbSet, queryBuilder);

                return dbSet.Select(select).ToList();
            }
        }

        public static async Task<IList<dynamic>> SelectCollectionAsync(Expression<Func<TEntity, object>> select, QueryBuilder<TEntity> queryBuilder = null)
        {
            queryBuilder = queryBuilder ?? new QueryBuilder<TEntity>();

            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                dbSet = AddIncludes(dbSet, queryBuilder);
                dbSet = BuildCollectionQuery(dbSet, queryBuilder);

                return await dbSet.Select(select).ToListAsync();
            }
        }
        #endregion

        #region Method - Save
        public static int Save(TEntity entity)
        {
            using (var context = new EFPlaygroundContext())
            {
                AddOrUpdate(context, entity);

                return context.SaveChanges();
            }
        }

        public static async Task<int> SaveAsync(TEntity entity)
        {
            using (var context = new EFPlaygroundContext())
            {
                AddOrUpdate(context, entity);
                
                return await context.SaveChangesAsync();
            }
        }
        #endregion

        #region Method - SaveCollection
        public static int SaveCollection(IEnumerable<TEntity> entities)
        {
            if (entities.Count() == 0)
            {
                return 0;
            }

            using (var context = new EFPlaygroundContext())
            {
                foreach (var entity in entities)
                {
                    AddOrUpdate(context, entity);
                }

                return context.SaveChanges();
            }
        }

        public static async Task<int> SaveCollectionAsync(IEnumerable<TEntity> entities)
        {
            if (entities.Count() == 0)
            {
                return 0;
            }

            using (var context = new EFPlaygroundContext())
            {
                foreach (var entity in entities)
                {
                    AddOrUpdate(context, entity);
                }

                return await context.SaveChangesAsync();
            }
        }
        #endregion

        #region Method - UpdateCollection
        public static int UpdateCollection(QueryBuilder<TEntity> queryBuilder, Func<TEntity, bool> updater)
        {
            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                dbSet = AddIncludes(dbSet, queryBuilder);
                dbSet = BuildCollectionQuery(dbSet, queryBuilder);

                var results = dbSet.ToList();

                return UpdateCollection(context, results, updater);
            }
        }

        public static int UpdateCollection(IEnumerable<TEntity> entities, Func<TEntity, bool> updater)
        {
            using (var context = new EFPlaygroundContext())
            {
                return UpdateCollection(context, entities, updater);
            }
        }

        private static int UpdateCollection(EFPlaygroundContext context, IEnumerable<TEntity> entities, Func<TEntity, bool> updater)
        {
            if (entities.Count() > 0)
            {
                bool hasUpdates = false;
                foreach (var entity in entities)
                {
                    if (updater(entity))
                    {
                        SetDateTrackingFields(entity);
                        context.Entry(entity).State = EntityState.Modified;

                        hasUpdates = true;
                    }
                }
                if (hasUpdates)
                {
                    return context.SaveChanges();
                }
            }

            return 0;
        }

        public static async Task<int> UpdateCollectionAsync(QueryBuilder<TEntity> queryBuilder, Func<TEntity, bool> updater)
        {
            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                dbSet = AddIncludes(dbSet, queryBuilder);
                dbSet = BuildCollectionQuery(dbSet, queryBuilder);

                var results = await dbSet.ToListAsync();

                return await UpdateCollectionAsync(context, results, updater);
            }
        }

        public static async Task<int> UpdateCollectionAsync(IEnumerable<TEntity> entities, Func<TEntity, bool> updater)
        {
            using (var context = new EFPlaygroundContext())
            {
                return await UpdateCollectionAsync(context, entities, updater);
            }
        }

        private static async Task<int> UpdateCollectionAsync(EFPlaygroundContext context, IEnumerable<TEntity> entities, Func<TEntity, bool> updater)
        {
            if (entities.Count() > 0)
            {
                bool hasUpdates = false;
                foreach (var entity in entities)
                {
                    if (updater(entity))
                    {
                        SetDateTrackingFields(entity);
                        context.Entry(entity).State = EntityState.Modified;
                        hasUpdates = true;
                    }
                }
                if (hasUpdates)
                {
                    return await context.SaveChangesAsync();
                }
            }

            return 0;
        }
        #endregion

        #region Method - Delete
        public static int Delete(TEntity entity)
        {
            using (var context = new EFPlaygroundContext())
            {
                context.Entry(entity).State = EntityState.Deleted;
                return context.SaveChanges();
            }
        }

        public static int Delete(QueryBuilder<TEntity> queryBuilder)
        {
            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                dbSet = BuildCollectionQuery(dbSet, queryBuilder);

                var results = dbSet.ToList();

                if (results.Count > 0)
                {
                    context.Set<TEntity>().RemoveRange(results);
                    return context.SaveChanges();
                }

                return 0;
            }
        }

        public static async Task<int> DeleteAsync(TEntity entity)
        {
            using (var context = new EFPlaygroundContext())
            {
                context.Entry(entity).State = EntityState.Deleted;
                return await context.SaveChangesAsync();
            }
        }

        public static async Task<int> DeleteAsync(QueryBuilder<TEntity> queryBuilder)
        {
            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                dbSet = BuildCollectionQuery(dbSet, queryBuilder);

                var results = await dbSet.ToListAsync();

                if (results.Count > 0)
                {
                    context.Set<TEntity>().RemoveRange(results);
                    return await context.SaveChangesAsync();
                }

                return 0;
            }
        }
        #endregion

        #region Method - DeleteCollection
        public static int DeleteCollection(IEnumerable<TEntity> entities)
        {
            using (var context = new EFPlaygroundContext())
            {
                foreach (var entity in entities)
                {
                    context.Entry(entity).State = EntityState.Deleted;
                }
                return context.SaveChanges();
            }
        }

        public static async Task<int> DeleteCollectionAsync(IEnumerable<TEntity> entities)
        {
            using (var context = new EFPlaygroundContext())
            {
                foreach (var entity in entities)
                {
                    context.Entry(entity).State = EntityState.Deleted;
                }
                return await context.SaveChangesAsync();
            }
        }
        #endregion

        #region Method - Count
        public static int Count(QueryBuilder<TEntity> queryBuilder = null)
        {
            queryBuilder = queryBuilder ?? new QueryBuilder<TEntity>();

            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();
                dbSet = AddWhereConditions(dbSet, queryBuilder);
                dbSet = AddPaging(dbSet, queryBuilder);
                return dbSet.Count();
            }
        }

        public static async Task<int> CountAsync(QueryBuilder<TEntity> queryBuilder = null)
        {
            queryBuilder = queryBuilder ?? new QueryBuilder<TEntity>();

            using (var context = new EFPlaygroundContext())
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();
                dbSet = AddWhereConditions(dbSet, queryBuilder);
                dbSet = AddPaging(dbSet, queryBuilder);
                return await dbSet.CountAsync();
            }
        }
        #endregion

        #region Method - Exists
        public static bool Exists(Guid id)
        {
            using (var context = new EFPlaygroundContext())
            {
                return context.Set<TEntity>().Count(x => x.Id == id) > 0;
            }
        }

        public static bool Exists(QueryBuilder<TEntity> queryBuilder)
        {
            return Count(queryBuilder) > 0;
        }

        public static async Task<bool> ExistsAsync(Guid id)
        {
            using (var context = new EFPlaygroundContext())
            {
                return await context.Set<TEntity>().CountAsync(x => x.Id == id) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(QueryBuilder<TEntity> queryBuilder)
        {
            return await CountAsync(queryBuilder) > 0;
        }
        #endregion

        #region Helper Method - AddOrUpdate
        private static void AddOrUpdate(EFPlaygroundContext context, TEntity entity)
        {
            if (!entity.IsNew)
            {
                SetDateTrackingFields(entity);
                context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                SetDateTrackingFields(entity, true);
                context.Set<TEntity>().Add(entity);
            }
        }
        #endregion

        #region Helper Method - AddIncludes
        private static IQueryable<TEntity> AddIncludes(IQueryable<TEntity> dbSet, QueryBuilder<TEntity> queryBuilder)
        {
            foreach (Expression<Func<TEntity, object>> include in queryBuilder.Includes)
            {
                dbSet = dbSet.Include<TEntity, object>(include);
            }
            return dbSet;
        }
        #endregion

        #region Helper Method - AddWhereConditions
        private static IQueryable<TEntity> AddWhereConditions(IQueryable<TEntity> dbSet, QueryBuilder<TEntity> queryBuilder)
        {
            foreach (Expression<Func<TEntity, bool>> whereCondition in queryBuilder.WhereConditions)
            {
                dbSet = dbSet.Where(whereCondition);
            }
            return dbSet;
        }
        #endregion

        #region Helper Method - AddOrderBys
        private static IQueryable<TEntity> AddOrderBys(IOrderedQueryable<TEntity> dbSet, QueryBuilder<TEntity> queryBuilder)
        {
            bool first = true;
            foreach (QuerySortExpression<TEntity> sortExpression in queryBuilder.SortExpressions)
            {
                if (sortExpression.Direction == QuerySortDirections.Ascending)
                {
                    dbSet = first ? dbSet.OrderBy(sortExpression.Expression) : dbSet.ThenBy(sortExpression.Expression);
                }
                else
                {
                    dbSet = first ? dbSet.OrderByDescending(sortExpression.Expression) : dbSet.ThenByDescending(sortExpression.Expression);
                }
                first = false;
            }
            return dbSet;
        }
        #endregion

        #region Helper Method - AddPaging
        private static IQueryable<TEntity> AddPaging(IQueryable<TEntity> dbSet, QueryBuilder<TEntity> queryBuilder)
        {
            if (queryBuilder.UsePaging)
            {
                if (queryBuilder.SortExpressions.Count == 0)
                {
                    dbSet = dbSet.OrderBy(x => x.Id);
                }

                //using the expression, the skip and take values will be parameterized which means the resulting sql query can be cached.
                dbSet = dbSet.Skip(() => queryBuilder.Skip).Take(() => queryBuilder.Take);
            }
            return dbSet;
        }
        #endregion

        #region Helper Method - BuildCollectionQuery
        private static IQueryable<TEntity> BuildCollectionQuery(IQueryable<TEntity> dbSet, QueryBuilder<TEntity> queryBuilder)
        {
            dbSet = AddWhereConditions(dbSet, queryBuilder);
            dbSet = AddOrderBys((IOrderedQueryable<TEntity>)dbSet, queryBuilder);
            dbSet = AddPaging(dbSet, queryBuilder);
            return dbSet;
        }
        #endregion

        #region Helper Method - SetDateTrackingFields
        private static void SetDateTrackingFields(TEntity entity, bool setCreatedDate = false)
        {
            var dateTrackedEntity = entity as DateTrackingModel;
            if (dateTrackedEntity != null)
            {
                dateTrackedEntity.DateModified = DateTime.Now;
                if (setCreatedDate)
                {
                    dateTrackedEntity.DateCreated = dateTrackedEntity.DateModified;
                }
            }
        }
        #endregion
    }
}
