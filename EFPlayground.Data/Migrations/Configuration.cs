namespace EFPlayground.Data.Migrations
{
    using EFPlayground.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public class Configuration : DbMigrationsConfiguration<EFPlayground.Data.EFPlaygroundContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EFPlayground.Data.EFPlaygroundContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var companyEFPlayground = new Company { Name = "EFPlayground Software", Location = "Vancouver", DateCreated = DateTime.Now, DateModified = DateTime.Now };
            context.Companies.AddOrUpdate(
                c => c.Name,
                companyEFPlayground
            );

            var adminGroup = new Group { Name = "Administrators", DateCreated = DateTime.Now, DateModified = DateTime.Now };
            var supportGroup = new Group { Name = "Support", DateCreated = DateTime.Now, DateModified = DateTime.Now };

            context.Groups.AddOrUpdate(
                g => g.Name,
                adminGroup,
                supportGroup
            );

            var adamCustomer = new Customer { FirstName = "Adam", LastName = "Nelson", CompanyId = companyEFPlayground.Id, DateCreated = DateTime.Now, DateModified = DateTime.Now, Source = CustomerSource.DirectEntry };
            adamCustomer.Groups.Add(adminGroup);
            adamCustomer.Groups.Add(supportGroup);

            var calebCustomer = new Customer { FirstName = "Caleb", LastName = "Sandfort", CompanyId = companyEFPlayground.Id, DateCreated = DateTime.Now, DateModified = DateTime.Now, Source = CustomerSource.Email };
            calebCustomer.Groups.Add(supportGroup);

            context.Customers.AddOrUpdate(
                c => c.FirstName,
                adamCustomer,
                calebCustomer
            );
        }
    }
}
