using EFPlayground.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFPlayground.Data
{
    public class QueryBuilder<TEntity> where TEntity : ModelBase
    {
        public QueryBuilder()
        {
            this.Includes = new List<Expression<Func<TEntity, object>>>();
            this.WhereConditions = new List<Expression<Func<TEntity, bool>>>();
            this.SortExpressions = new List<QuerySortExpression<TEntity>>();
        }

        public bool UsePaging { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }

        public void AddWhereClause(Expression<Func<TEntity, bool>> expression)
        {
            this.WhereConditions.Add(expression);
        }

        public void AddInclude(Expression<Func<TEntity, object>> include)
        {
            this.Includes.Add(include);
        }

        public void AddSortExpression(Expression<Func<TEntity, object>> expression)
        {
            this.SortExpressions.Add(new QuerySortExpression<TEntity>(expression));
        }

        public void AddSortExpression(Expression<Func<TEntity, object>> expression, QuerySortDirections direction)
        {
            this.SortExpressions.Add(new QuerySortExpression<TEntity>(expression, direction));
        }

        internal List<Expression<Func<TEntity, object>>> Includes { get; set; }
        internal List<Expression<Func<TEntity, bool>>> WhereConditions { get; set; }
        internal List<QuerySortExpression<TEntity>> SortExpressions { get; set; }
    }

    internal class QuerySortExpression<TEntity> where TEntity : ModelBase
    {
        public QuerySortExpression(Expression<Func<TEntity, object>> expression) : this(expression, QuerySortDirections.Ascending) { }
        public QuerySortExpression(Expression<Func<TEntity, object>> expression, QuerySortDirections direction)
        {
            this.Expression = expression;
            this.Direction = direction;
        }

        public Expression<Func<TEntity, object>> Expression { get; private set; }
        public QuerySortDirections Direction { get; private set; }
    }

    public enum QuerySortDirections
    {
        Ascending,
        Descending
    }
}
