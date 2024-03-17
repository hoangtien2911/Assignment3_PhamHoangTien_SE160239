using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace ClockApplicationBO.Models
{
    public partial class ClockApplicationContext : DbContext
    {
        public ClockApplicationContext()
        {
        }

        public ClockApplicationContext(DbContextOptions<ClockApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Alarm> Alarms { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
            var strConn = config["ConnectionStrings:ClockApplication"];

            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alarm>(entity =>
            {
                entity.ToTable("alarm");

                entity.Property(e => e.AlarmId).HasColumnName("alarm_id");

                entity.Property(e => e.AlarmName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("alarm_name");

                entity.Property(e => e.AlarmTime)
                    .HasColumnType("datetime")
                    .HasColumnName("alarm_time");

                entity.Property(e => e.Enabled).HasColumnName("enabled");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
