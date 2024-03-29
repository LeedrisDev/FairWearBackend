﻿using BackOffice.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.DataAccess;

/// <inheritdoc />
public class BrandAndProductDbContext : DbContext
{
    /// <inheritdoc />
    public BrandAndProductDbContext()
    {
    }

    /// <inheritdoc />
    public BrandAndProductDbContext(DbContextOptions<BrandAndProductDbContext> options) : base(options)
    {
    }

    /// <summary>The Brands table.</summary>
    public virtual DbSet<BrandEntity> Brands { get; set; } = null!;

    /// <summary>The Products table.</summary>
    public virtual DbSet<ProductEntity> Products { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BrandEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("brands_pkey");

            entity.ToTable("brands");

            entity.HasIndex(e => e.Name, "brands_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnimalRating).HasColumnName("animal_rating");
            entity.Property(e => e.Categories)
                .HasColumnType("character varying[]")
                .HasColumnName("categories");
            entity.Property(e => e.Country)
                .HasColumnType("character varying")
                .HasColumnName("country");
            entity.Property(e => e.EnvironmentRating).HasColumnName("environment_rating");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.PeopleRating).HasColumnName("people_rating");
            entity.Property(e => e.Ranges)
                .HasColumnType("character varying[]")
                .HasColumnName("ranges");
            entity.Property(e => e.RatingDescription)
                .HasColumnType("character varying")
                .HasColumnName("rating_description");
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Category)
                .HasColumnType("character varying")
                .HasColumnName("category");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Ranges)
                .HasColumnType("character varying[]")
                .HasColumnName("ranges");
            entity.Property(e => e.UpcCode)
                .HasColumnType("character varying")
                .HasColumnName("upc_code");

            entity.HasOne(d => d.BrandEntity).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("products_brand_id_fkey");
        });
    }
}