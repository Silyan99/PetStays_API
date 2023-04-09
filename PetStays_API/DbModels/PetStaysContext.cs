using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PetStays_API.DBModels;

public partial class PetStaysContext : DbContext
{
    public PetStaysContext()
    {
    }

    public PetStaysContext(DbContextOptions<PetStaysContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Integrated Security=True;Initial Catalog=PetStays;Data Source=KISHANAUTOBOTS\\SQLEXPRESS; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__contacts__3213E83FF38620EA");

            entity.ToTable("contacts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("message");
            entity.Property(e => e.Mobile)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("mobile");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F382C6684");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.Mobile)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("mobile");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
