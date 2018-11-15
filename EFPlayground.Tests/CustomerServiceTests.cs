using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFPlayground.Data;
using EFPlayground.Data.Services;
using System.Data.Entity;
using EFPlayground.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFPlayground.Tests
{
    [TestClass]
    public class CustomerServiceTests
    {
        #region Test - Get
        [TestMethod]
        public async Task CustomerGetAsync()
        {
            TestUtilities.ResetDatabase();

            Customer customer = new Customer();
            customer.FirstName = "Daren";
            customer.LastName = "Nelson";

            await CustomerService.SaveAsync(customer);

            var savedCustomer = await CustomerService.GetAsync(customer.Id);

            Assert.IsFalse(savedCustomer.IsNew);
            Assert.AreEqual(customer.Id, savedCustomer.Id);
            Assert.AreEqual("Daren", savedCustomer.FirstName);
            Assert.AreEqual("Daren Nelson", savedCustomer.FullName);
        }

        [TestMethod]
        public void CustomerGet()
        {
            TestUtilities.ResetDatabase();

            Customer customer = new Customer();
            customer.FirstName = "Daren";
            customer.LastName = "Nelson";

            CustomerService.Save(customer);

            var savedCustomer = CustomerService.Get(customer.Id);

            Assert.IsFalse(savedCustomer.IsNew);
            Assert.AreEqual(customer.Id, savedCustomer.Id);
            Assert.AreEqual("Daren", savedCustomer.FirstName);
        }
        #endregion

        #region Test - GetByCompany
        [TestMethod]
        public void CustomerGetByCompanyAsync()
        {
            TestUtilities.ResetDatabase();

            var customers = CustomerService.GetCustomersByCompany("EFPlayground Software");

            Assert.AreEqual(2, customers.Count);
        }
        #endregion

        #region Test - GetCollection
        [TestMethod]
        public async Task CustomerGetCollectionAsync()
        {
            TestUtilities.ResetDatabase();

            var customers = await CustomerService.GetCollectionAsync();
            Assert.AreEqual(2, customers.Count);
            Assert.AreEqual(null, customers[0].Company);

            QueryBuilder<Customer> queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.AddWhereClause(x => x.FirstName == "Adam");
            queryBuilder.AddInclude(x => x.Company);
            queryBuilder.AddInclude(x => x.Groups);
            customers = await CustomerService.GetCollectionAsync(queryBuilder);
            Assert.AreEqual(1, customers.Count);
            Assert.AreEqual("EFPlayground Software", customers[0].Company.Name);
            Assert.IsNotNull(customers[0].Company);
            Assert.IsNotNull(customers[0].Groups);
            Assert.AreEqual(2, customers[0].Groups.Count);
        }

        [TestMethod]
        public void CustomerGetCollection()
        {
            TestUtilities.ResetDatabase();

            var customers = CustomerService.GetCollection();
            Assert.AreEqual(2, customers.Count);
            Assert.AreEqual(null, customers[0].Company);

            QueryBuilder<Customer> queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.AddWhereClause(x => x.FirstName == "Adam");
            queryBuilder.AddInclude(x => x.Company);
            customers = CustomerService.GetCollection(queryBuilder);
            Assert.AreEqual(1, customers.Count);
            Assert.AreNotEqual(null, customers[0].Company);
        }
        #endregion

        #region Test - Select
        [TestMethod]
        public async Task CustomerSelectAsync()
        {
            TestUtilities.ResetDatabase();

            Customer customer = new Customer();
            customer.FirstName = "Daren";
            customer.LastName = "Nelson";

            await CustomerService.SaveAsync(customer);

            var savedCustomer = await CustomerService.SelectAsync(customer.Id, (x => new { Id = x.Id, First = x.FirstName, CompName = x.Company.Name }));

            Assert.AreEqual(customer.Id, savedCustomer.Id);
            Assert.AreEqual("Daren", savedCustomer.First);
        }

        [TestMethod]
        public void CustomerSelect()
        {
            TestUtilities.ResetDatabase();

            Customer customer = new Customer();
            customer.FirstName = "Daren";
            customer.LastName = "Nelson";

            CustomerService.Save(customer);

            var savedCustomer = CustomerService.Select(customer.Id, (x => new { Id = x.Id, First = x.FirstName, CompName = x.Company.Name }));

            Assert.AreEqual(customer.Id, savedCustomer.Id);
            Assert.AreEqual("Daren", savedCustomer.First);
        }
        #endregion

        #region Test - SelectCollection
        [TestMethod]
        public async Task CustomerSelectCollectionAsync()
        {
            TestUtilities.ResetDatabase();

            QueryBuilder<Customer> queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.AddSortExpression(x => x.FirstName);

            var customers = await CustomerService.SelectCollectionAsync((x => new { Id = x.Id, First = x.FirstName, CompName = x.Company.Name }), queryBuilder);

            Assert.AreEqual(2, customers.Count);
            Assert.AreEqual("Adam", customers[0].First);
            Assert.AreEqual("EFPlayground Software", customers[0].CompName);
        }

        [TestMethod]
        public void CustomerSelectCollection()
        {
            TestUtilities.ResetDatabase();

            QueryBuilder<Customer> queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.AddSortExpression(x => x.FirstName);

            var customers = CustomerService.SelectCollection((x => new { Id = x.Id, First = x.FirstName, CompName = x.Company.Name }), queryBuilder);

            Assert.AreEqual(2, customers.Count);
            Assert.AreEqual("Adam", customers[0].First);
            Assert.AreEqual("EFPlayground Software", customers[0].CompName);
        }
        #endregion

        #region Test - Save
        [TestMethod]
        public async Task CustomerSaveAsync()
        {
            TestUtilities.ResetDatabase();

            Company companyGwiSoftware = new Company();
            companyGwiSoftware.Name = "GWI Software";
            companyGwiSoftware.Location = "Vancouver";
            await CompanyService.SaveAsync(companyGwiSoftware);

            Customer customerDanGreen = new Customer();
            customerDanGreen.FirstName = "Dan";
            customerDanGreen.LastName = "Green";
            customerDanGreen.CompanyId = companyGwiSoftware.Id;
            await CustomerService.SaveAsync(customerDanGreen);

            Assert.IsNotNull(customerDanGreen.Id);
            Assert.IsNotNull(customerDanGreen.DateModified);
            Assert.IsNotNull(customerDanGreen.DateCreated);

            var companies = await CompanyService.GetCollectionAsync();
            Assert.AreEqual(2, companies.Count);

            var customers = await CustomerService.GetCollectionAsync();
            Assert.AreEqual(3, customers.Count);
        }

        [TestMethod]
        public void CustomerSave()
        {
            TestUtilities.ResetDatabase();

            Company companyGwiSoftware = new Company();
            companyGwiSoftware.Name = "GWI Software";
            companyGwiSoftware.Location = "Vancouver";
            CompanyService.Save(companyGwiSoftware);

            Customer customerDanGreen = new Customer();
            customerDanGreen.FirstName = "Dan";
            customerDanGreen.LastName = "Green";
            customerDanGreen.CompanyId = companyGwiSoftware.Id;
            CustomerService.Save(customerDanGreen);

            Assert.IsNotNull(customerDanGreen.Id);
            Assert.IsNotNull(customerDanGreen.DateModified);
            Assert.IsNotNull(customerDanGreen.DateCreated);

            var companies = CompanyService.GetCollection();
            Assert.AreEqual(2, companies.Count);

            var customers = CustomerService.GetCollection();
            Assert.AreEqual(3, customers.Count);
        }
        #endregion

        #region Test - SaveCollection
        [TestMethod]
        public async Task CustomerSaveCollectionAsync()
        {
            TestUtilities.ResetDatabase();

            await CustomerService.SaveCollectionAsync(new List<Customer>()
            {
                new Customer { FirstName = "Daren", LastName = "Nelson" },
                new Customer { FirstName = "Darren", LastName = "Grigg" },
                new Customer { FirstName = "Ryan", LastName = "Terrell" },
                new Customer { FirstName = "Lisa", LastName = "Kimery" },
            });

            var customers = await CustomerService.GetCollectionAsync();
            Assert.AreEqual(6, customers.Count);
        }

        [TestMethod]
        public void CustomerSaveCollection()
        {
            TestUtilities.ResetDatabase();

            CustomerService.SaveCollection(new List<Customer>()
            {
                new Customer { FirstName = "Daren", LastName = "Nelson" },
                new Customer { FirstName = "Darren", LastName = "Grigg" },
                new Customer { FirstName = "Ryan", LastName = "Terrell" },
                new Customer { FirstName = "Lisa", LastName = "Kimery" },
            });

            var customers = CustomerService.GetCollection();
            Assert.AreEqual(6, customers.Count);
        }
        #endregion

        #region Test - UpdateCollection
        [TestMethod]
        public async Task CustomerUpdateCollectionAsync()
        {
            TestUtilities.ResetDatabase();
        
            var customers = await CustomerService.GetCollectionAsync();
            Assert.AreEqual(2, customers.Count);
            Assert.AreEqual("Nelson", customers[0].LastName);

            await CustomerService.UpdateCollectionAsync(customers, (x => { x.LastName = "Updated"; return true; }));

            customers = await CustomerService.GetCollectionAsync();

            Assert.AreEqual("Updated", customers[0].LastName);
            Assert.AreNotEqual(customers[0].DateCreated, customers[0].DateModified);
        }

        [TestMethod]
        public void CustomerUpdateCollection()
        {
            TestUtilities.ResetDatabase();

            var customers = CustomerService.GetCollection();
            Assert.AreEqual(2, customers.Count);
            Assert.AreEqual("Nelson", customers[0].LastName);

            CustomerService.UpdateCollection(customers, (x => { x.LastName = "Updated"; return true; }));

            customers = CustomerService.GetCollection();

            Assert.AreEqual("Updated", customers[0].LastName);
            Assert.AreNotEqual(customers[0].DateCreated, customers[0].DateModified);
        }
        #endregion

        #region Test - Delete
        [TestMethod]
        public async Task CustomerDeleteAsync()
        {
            TestUtilities.ResetDatabase();

            QueryBuilder<Customer> query = new QueryBuilder<Customer>();
            query.AddWhereClause(x => x.FirstName == "Adam");
            var customers = await CustomerService.GetCollectionAsync(query);

            await CustomerService.DeleteAsync(customers[0]);

            int count = await CustomerService.CountAsync();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void CustomerDelete()
        {
            TestUtilities.ResetDatabase();

            QueryBuilder<Customer> query = new QueryBuilder<Customer>();
            query.AddWhereClause(x => x.FirstName == "Adam");
            var customers = CustomerService.GetCollection(query);

            CustomerService.Delete(customers[0]);

            int count = CustomerService.Count();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public async Task CustomerDeleteQueryAsync()
        {
            TestUtilities.ResetDatabase();

            QueryBuilder<Customer> query = new QueryBuilder<Customer>();
            query.AddWhereClause(x => x.FirstName == "Adam");

            await CustomerService.DeleteAsync(query);

            int count = await CustomerService.CountAsync();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void CustomerDeleteQuery()
        {
            TestUtilities.ResetDatabase();

            QueryBuilder<Customer> query = new QueryBuilder<Customer>();
            query.AddWhereClause(x => x.FirstName == "Adam");

            CustomerService.Delete(query);

            int count = CustomerService.Count();

            Assert.AreEqual(1, count);
        }
        #endregion

        #region Test - DeleteCollection
        [TestMethod]
        public async Task CustomerDeleteCollectionAsync()
        {
            TestUtilities.ResetDatabase();

            QueryBuilder<Customer> query = new QueryBuilder<Customer>();
            query.AddWhereClause(x => x.FirstName == "Adam");
            var customers = await CustomerService.GetCollectionAsync(query);

            await CustomerService.DeleteCollectionAsync(customers);

            int count = await CustomerService.CountAsync();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void CustomerDeleteCollection()
        {
            TestUtilities.ResetDatabase();

            QueryBuilder<Customer> query = new QueryBuilder<Customer>();
            query.AddWhereClause(x => x.FirstName == "Adam");
            var customers = CustomerService.GetCollection(query);

            CustomerService.DeleteCollection(customers);

            int count = CustomerService.Count();

            Assert.AreEqual(1, count);
        }
        #endregion

        #region Test - Count
        [TestMethod]
        public async Task CustomerCountAsync()
        {
            TestUtilities.ResetDatabase();

            var count = await CustomerService.CountAsync();
            Assert.AreEqual(2, count);

            QueryBuilder<Customer> queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.AddWhereClause(x => x.FirstName == "Adam");
            count = await CustomerService.CountAsync(queryBuilder);
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void CustomerCount()
        {
            TestUtilities.ResetDatabase();

            var count = CustomerService.Count();
            Assert.AreEqual(2, count);

            QueryBuilder<Customer> queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.AddWhereClause(x => x.FirstName == "Adam");
            count = CustomerService.Count(queryBuilder);
            Assert.AreEqual(1, count);
        }
        #endregion

        #region Test - Exists
        [TestMethod]
        public async Task CustomerExistsAsync()
        {
            TestUtilities.ResetDatabase();

            QueryBuilder<Customer> queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.AddWhereClause(x => x.FirstName == "Adam");
            var customerAdam = await CustomerService.GetCollectionAsync(queryBuilder);
            var knownId = customerAdam[0].Id;

            var exists = await CustomerService.ExistsAsync(knownId);
            Assert.AreEqual(true, exists);

            queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.AddWhereClause(x => x.FirstName == "Adam");
            exists = await CustomerService.ExistsAsync(queryBuilder);
            Assert.AreEqual(true, exists);
        }

        [TestMethod]
        public void CustomerExists()
        {
            TestUtilities.ResetDatabase();

            QueryBuilder<Customer> queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.AddWhereClause(x => x.FirstName == "Adam");
            var customerAdam = CustomerService.GetCollection(queryBuilder);
            var knownId = customerAdam[0].Id;

            var exists = CustomerService.Exists(knownId);
            Assert.AreEqual(true, exists);

            queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.AddWhereClause(x => x.FirstName == "Adam");
            exists = CustomerService.Exists(queryBuilder);
            Assert.AreEqual(true, exists);
        }
        #endregion

        #region Test - Paging
        [TestMethod]
        public async Task CustomerPagedQueryAsync()
        {
            TestUtilities.ResetDatabase();

            var queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.UsePaging = true;
            queryBuilder.Skip = 0;
            queryBuilder.Take = 1;
            queryBuilder.AddSortExpression(x => x.FirstName, QuerySortDirections.Descending);
            var customers = await CustomerService.GetCollectionAsync(queryBuilder);
            Assert.AreEqual(1, customers.Count);
            Assert.AreEqual("Caleb", customers[0].FirstName);
        }

        [TestMethod]
        public void CustomerPagedQuery()
        {
            TestUtilities.ResetDatabase();

            var queryBuilder = new QueryBuilder<Customer>();
            queryBuilder.UsePaging = true;
            queryBuilder.Skip = 0;
            queryBuilder.Take = 1;
            queryBuilder.AddSortExpression(x => x.FirstName);
            var customers = CustomerService.GetCollection(queryBuilder);
            Assert.AreEqual(1, customers.Count);
            Assert.AreEqual("Adam", customers[0].FirstName);
        }
        #endregion
    }
}
