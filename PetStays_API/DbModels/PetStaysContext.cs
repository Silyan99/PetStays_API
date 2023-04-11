using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PetStays_API.DBModels;

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

    public virtual DbSet<Availability> Availabilities { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         => optionsBuilder.UseSqlServer(_connectionString);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Availability>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__availabi__3213E83F505CB645");

            entity.ToTable("availability");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.FullDay).HasColumnName("full_day");
            entity.Property(e => e.TimeEnd).HasColumnName("time_end");
            entity.Property(e => e.TimeStart).HasColumnName("time_start");

            entity.HasOne(d => d.Admin).WithMany(p => p.Availabilities)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK__availabil__admin__36B12243");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__contacts__3213E83FF38620EA");

            entity.ToTable("contacts");

            entity.HasIndex(e => e.Email, "unique_contact_email").IsUnique();

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

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pets__3213E83FBC19E0C6");

            entity.ToTable("pets");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Breed)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("breed");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("color");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Details)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("details");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Photo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("photo");
            entity.Property(e => e.Uid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("uid");
            entity.Property(e => e.Vaccinated).HasColumnName("vaccinated");

            entity.HasOne(d => d.Owner).WithMany(p => p.Pets)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__pets__owner_id__31EC6D26");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__requests__3213E83FDFBC8C68");

            entity.ToTable("requests");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.IsPaymentDone).HasColumnName("is_payment_done");
            entity.Property(e => e.MadeBy).HasColumnName("made_by");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("remarks");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimeFrom).HasColumnName("time_from");
            entity.Property(e => e.TimeTo).HasColumnName("time_to");

            entity.HasOne(d => d.MadeByNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.MadeBy)
                .HasConstraintName("FK__requests__made_b__3A81B327");

            entity.HasOne(d => d.Pet).WithMany(p => p.Requests)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("FK__requests__pet_id__3B75D760");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F25785F57");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "unique_email").IsUnique();

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
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
