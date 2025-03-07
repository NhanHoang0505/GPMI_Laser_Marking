namespace GPMI_Laser_Marking.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GPMI_Laser_Marking.InputContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GPMI_Laser_Marking.InputContext context)
        {
            //  This method will be called after migrating to the latest version.
            context.accounts.AddOrUpdate(new Models.Account
            {
                FullName = "Admin",
                ID = "Admin",
                FQA = true,
                Manager = true,
                Pakaging = true,
                Password = "1234",
                Shipping = true,
                Marking = true,
                Import = true
            });
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
