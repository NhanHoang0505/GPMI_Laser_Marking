using GPMI_Laser_Marking.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace GPMI_Laser_Marking
{
    public  class InputContext : DbContext
    {
        public InputContext(): base("GPMIconnect")
        {
            //InputConnectionString
            //GPMIconnect
        }
        public DbSet<Account> accounts { get; set; }
        public DbSet<OrderInput> orderInputs { get; set; }
        public DbSet<OutputMarking> outputMarkings { get; set; }

        public DbSet<NGMarking> nGMarkings { get; set; }
        public DbSet<PackagingManager> packagingManagers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<FQA>()
            //  .HasRequired(s => s.OutputMarking)
            //  .WithRequiredPrincipal(ad => ad.FQA);

            //modelBuilder.Entity<FQA>()
            //   .HasOptional(s => s.OutputMarking) // Mark Address property optional in Student entity
            //   .WithRequired(ad => ad.FQA);
        }
    }
}
