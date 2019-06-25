using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using BSM.Common.Model;

namespace BSM.Common.DB
{
    public class BSMContext : DbContext 
    {
        public string DbConnection
        {
            get; private set;
        }

        public string DataSourceString => string.Format("{0} - {1}", this.Database.GetDbConnection().DataSource, this.Database.GetDbConnection().Database);

        
        public DbSet<User> Users { get; set; }

        public DbSet<Station> Stations { get; set; }

        public DbSet<Coordinator> Coordinators { get; set; }

        public DbSet<StationData> StationDatas { get; set; }

        public DbSet<StationComment> StationComments { get; set; }

        public DbSet<Alert> Alerts { get; set; }

        public DbSet<UserPermission> UserPermissions { get; set; }

        public DbSet<AlertSetting> AlertSettings { get; set; }


        public BSMContext(string dbconnection)
        {
            this.DbConnection = dbconnection;
        }

        public BSMContext()
        { }

        public BSMContext(DbContextOptions<BSMContext> options) 
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(this.DbConnection))
            {
                optionsBuilder.UseSqlServer(this.DbConnection);
            }            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Station
            modelBuilder.Entity<Station>()
                .Property(st => st.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Station>()
                .HasIndex(st => st.Tag);
            modelBuilder.Entity<Station>()
                .HasIndex(st => st.Name);

            //Coordinator
            modelBuilder.Entity<Coordinator>()
                .HasKey(co => new { co.StationId, co.SeqId });
            modelBuilder.Entity<Coordinator>()
                .HasIndex(co => co.Name);

            //Station Data
            modelBuilder.Entity<StationData>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd();

            //Station Comment
            modelBuilder.Entity<StationComment>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            //User
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.LoginID)
                .IsUnique(true);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(true);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.MobilePhone)
                .IsUnique(true);

            //Alert
            modelBuilder.Entity<Alert>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<AlertSetting>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
