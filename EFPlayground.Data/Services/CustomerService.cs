using EFPlayground.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace EFPlayground.Data.Services
{
    public class CustomerService : CustomerServiceBase
    {
        public static IList<Customer> GetCustomersByCompany(string companyName)
        {
            QueryBuilder<Customer> queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.AddWhereClause(x => x.Company.Name == companyName);
            return CustomerService.GetCollection(queryBuilder);
        }
    }
}
