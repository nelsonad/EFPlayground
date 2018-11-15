using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFPlayground.Models;
using EFPlayground.Data.Services;
using System.Threading.Tasks;
using EFPlayground.Data;

namespace EFPlayground.Tests
{
    [TestClass]
    public class TransactionTests
    {
        #region Test - TransactionCommit
        [TestMethod]
        public void TransactionCommit()
        {
            TestUtilities.ResetDatabase();

            using (var scope = TransactionFactory.Create())
            {
                Company companyGwiSoftware = new Company();
                companyGwiSoftware.Name = "GWI Software";
                companyGwiSoftware.Location = "Vancouver";
                CompanyService.Save(companyGwiSoftware);

                Customer customerDanGreen = new Customer();
                customerDanGreen.FirstName = "Dan";
                customerDanGreen.LastName = "Green";
                customerDanGreen.CompanyId = companyGwiSoftware.Id;
                CustomerService.Save(customerDanGreen);

                Assert.AreNotEqual(null, customerDanGreen.Id);

                scope.Complete();
            }

            var companies = CompanyService.GetCollection();
            Assert.AreEqual(2, companies.Count);

            var customers = CustomerService.GetCollection();
            Assert.AreEqual(3, customers.Count);
        }

        [TestMethod]
        public async Task TransactionCommitAsync()
        {
            TestUtilities.ResetDatabase();

            using (var scope = TransactionFactory.CreateAsync())
            {
                Company companyGwiSoftware = new Company();
                companyGwiSoftware.Name = "GWI Software";
                companyGwiSoftware.Location = "Vancouver";
                await CompanyService.SaveAsync(companyGwiSoftware);

                Customer customerDanGreen = new Customer();
                customerDanGreen.FirstName = "Dan";
                customerDanGreen.LastName = "Green";
                customerDanGreen.CompanyId = companyGwiSoftware.Id;
                await CustomerService.SaveAsync(customerDanGreen);

                Assert.AreNotEqual(null, customerDanGreen.Id);

                scope.Complete();
            }

            var companies = await CompanyService.GetCollectionAsync();
            Assert.AreEqual(2, companies.Count);

            var customers = await CustomerService.GetCollectionAsync();
            Assert.AreEqual(3, customers.Count);
        }
        #endregion

        #region Test - TransactionRollback
        [TestMethod]
        public void TransactionRollback()
        {
            TestUtilities.ResetDatabase();

            using (var scope = TransactionFactory.Create())
            {
                Company companyGwiSoftware = new Company();
                companyGwiSoftware.Name = "GWI Software";
                companyGwiSoftware.Location = "Vancouver";
                CompanyService.Save(companyGwiSoftware);

                Customer customerDanGreen = new Customer();
                customerDanGreen.FirstName = "Dan";
                customerDanGreen.LastName = "Green";
                customerDanGreen.CompanyId = companyGwiSoftware.Id;
                CustomerService.Save(customerDanGreen);

                Assert.AreNotEqual(null, customerDanGreen.Id);
            }

            var companies = CompanyService.GetCollection();
            Assert.AreEqual(1, companies.Count);

            var customers = CustomerService.GetCollection();
            Assert.AreEqual(2, customers.Count);
        }

        [TestMethod]
        public async Task TransactionRollbackAsync()
        {
            TestUtilities.ResetDatabase();

            using (var scope = TransactionFactory.CreateAsync())
            {
                Company companyGwiSoftware = new Company();
                companyGwiSoftware.Name = "GWI Software";
                companyGwiSoftware.Location = "Vancouver";
                await CompanyService.SaveAsync(companyGwiSoftware);

                Customer customerDanGreen = new Customer();
                customerDanGreen.FirstName = "Dan";
                customerDanGreen.LastName = "Green";
                customerDanGreen.CompanyId = companyGwiSoftware.Id;
                await CustomerService.SaveAsync(customerDanGreen);

                Assert.AreNotEqual(null, customerDanGreen.Id);
            }

            var companies = await CompanyService.GetCollectionAsync();
            Assert.AreEqual(1, companies.Count);

            var customers = await CustomerService.GetCollectionAsync();
            Assert.AreEqual(2, customers.Count);
        }
        #endregion

        #region Test - TransactionValidationErrorRollback
        [TestMethod]
        public void TransactionValidationErrorRollback()
        {
            TestUtilities.ResetDatabase();

            try
            {
                using (var scope = TransactionFactory.Create())
                {
                    Company companyGwiSoftware = new Company();
                    companyGwiSoftware.Name = "GWI Software";
                    companyGwiSoftware.Location = "Vancouver";
                    CompanyService.Save(companyGwiSoftware);

                    Customer customerDanGreen = new Customer();
                    customerDanGreen.FirstName = "Dan";
                    customerDanGreen.CompanyId = companyGwiSoftware.Id;
                    CustomerService.Save(customerDanGreen);

                    Assert.AreNotEqual(null, customerDanGreen.Id);

                    scope.Complete();
                }

                Assert.Fail("Customer.LastName is required.");
            }
            catch
            {
                
            }

            var companies = CompanyService.GetCollection();
            Assert.AreEqual(1, companies.Count);

            var customers = CustomerService.GetCollection();
            Assert.AreEqual(2, customers.Count);
        }

        [TestMethod]
        public async Task TransactionValidationErrorRollbackAsync()
        {
            TestUtilities.ResetDatabase();

            try
            {
                using (var scope = TransactionFactory.CreateAsync())
                {
                    Company companyGwiSoftware = new Company();
                    companyGwiSoftware.Name = "GWI Software";
                    companyGwiSoftware.Location = "Vancouver";
                    await CompanyService.SaveAsync(companyGwiSoftware);

                    Customer customerDanGreen = new Customer();
                    customerDanGreen.FirstName = "Dan";
                    customerDanGreen.CompanyId = companyGwiSoftware.Id;
                    await CustomerService.SaveAsync(customerDanGreen);

                    Assert.AreNotEqual(null, customerDanGreen.Id);

                    scope.Complete();
                }

                Assert.Fail("Customer.LastName is required.");
            }
            catch
            {

            }

            var companies = await CompanyService.GetCollectionAsync();
            Assert.AreEqual(1, companies.Count);

            var customers = await CustomerService.GetCollectionAsync();
            Assert.AreEqual(2, customers.Count);
        }
        #endregion

        #region Test - TransactionNestedCommit
        [TestMethod]
        public void TransactionNestedCommit()
        {
            TestUtilities.ResetDatabase();

            using (var scope = TransactionFactory.Create())
            {
                Company companyGwiSoftware = new Company();
                companyGwiSoftware.Name = "GWI Software";
                companyGwiSoftware.Location = "Vancouver";
                CompanyService.Save(companyGwiSoftware);

                Customer customerDanGreen = new Customer();
                customerDanGreen.FirstName = "Dan";
                customerDanGreen.LastName = "Green";
                customerDanGreen.CompanyId = companyGwiSoftware.Id;
                CustomerService.Save(customerDanGreen);

                using (var scope2 = TransactionFactory.Create())
                {
                    Company companyLblSoft = new Company();
                    companyLblSoft.Name = "LblSoft";
                    companyLblSoft.Location = "Vancouver";
                    CompanyService.Save(companyLblSoft);

                    Customer customerLisaKimery = new Customer();
                    customerLisaKimery.FirstName = "Lisa";
                    customerLisaKimery.LastName = "Kimery";
                    customerLisaKimery.CompanyId = companyLblSoft.Id;
                    CustomerService.Save(customerLisaKimery);

                    Assert.AreNotEqual(null, customerLisaKimery.Id);

                    scope2.Complete();
                }

                Assert.AreNotEqual(null, customerDanGreen.Id);

                scope.Complete();
            }

            var companies = CompanyService.GetCollection();
            Assert.AreEqual(3, companies.Count);

            var customers = CustomerService.GetCollection();
            Assert.AreEqual(4, customers.Count);
        }
        #endregion

        #region Test - TransactionNestedRollback
        [TestMethod]
        public void TransactionNestedRollback()
        {
            TestUtilities.ResetDatabase();

            using (var scope = TransactionFactory.Create())
            {
                Company companyGwiSoftware = new Company();
                companyGwiSoftware.Name = "GWI Software";
                companyGwiSoftware.Location = "Vancouver";
                CompanyService.Save(companyGwiSoftware);

                Customer customerDanGreen = new Customer();
                customerDanGreen.FirstName = "Dan";
                customerDanGreen.LastName = "Green";
                customerDanGreen.CompanyId = companyGwiSoftware.Id;
                CustomerService.Save(customerDanGreen);

                using (var scope2 = TransactionFactory.Create())
                {
                    Company companyLblSoft = new Company();
                    companyLblSoft.Name = "LblSoft";
                    companyLblSoft.Location = "Vancouver";
                    CompanyService.Save(companyLblSoft);

                    Customer customerLisaKimery = new Customer();
                    customerLisaKimery.FirstName = "Lisa";
                    customerLisaKimery.LastName = "Kimery";
                    customerLisaKimery.CompanyId = companyLblSoft.Id;
                    CustomerService.Save(customerLisaKimery);

                    Assert.AreNotEqual(null, customerLisaKimery.Id);

                    scope2.Complete();
                }

                Assert.AreNotEqual(null, customerDanGreen.Id);
            }

            var companies = CompanyService.GetCollection();
            Assert.AreEqual(1, companies.Count);

            var customers = CustomerService.GetCollection();
            Assert.AreEqual(2, customers.Count);
        }
        #endregion
    }
}
