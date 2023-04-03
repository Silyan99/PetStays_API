using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PetStays.Domain.Entities
{
    public partial class PetStaysContext : DbContext
    {
        private string _connectionString = String.Empty;
        public PetStaysContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PetStaysContext(DbContextOptions<PetStaysContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", "pet");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .HasColumnName("email");
                entity.Property(e => e.Password)
                    .HasMaxLength(128)
                    .HasColumnName("password");
                entity.Property(e => e.UserName)
                    .HasMaxLength(256)
                    .HasColumnName("user_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
