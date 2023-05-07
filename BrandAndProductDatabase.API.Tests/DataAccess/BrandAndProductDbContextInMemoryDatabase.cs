using System.Text.Json;
using BrandAndProductDatabase.API.DataAccess;
using BrandAndProductDatabase.API.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BrandAndProductDatabase.API.Tests.DataAccess;

public class BrandAndProductDbContextInMemoryDatabase : BrandAndProductDbContext
{
    public BrandAndProductDbContextInMemoryDatabase(DbContextOptions<BrandAndProductDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;
        optionsBuilder.UseInMemoryDatabase("test_database");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var options = new JsonSerializerOptions();


        // Configure the Items property to use a ValueConverter
        var itemsConverter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, options),
            v => JsonSerializer.Deserialize<List<string>>(v, options)!
        );

        modelBuilder.Entity<BrandEntity>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("brands_pkey");

            entity.ToTable("brands");

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.Property(e => e.Country)
                .HasColumnType("character varying")
                .HasColumnName("country");

            entity.Property(e => e.EnvironmentRating)
                .HasColumnName("environment_rating");

            entity.Property(e => e.PeopleRating)
                .HasColumnName("people_rating");

            entity.Property(e => e.AnimalRating)
                .HasColumnName("animal_rating");

            entity.Property(e => e.RatingDescription)
                .HasColumnType("character varying")
                .HasColumnName("rating_description");

            entity.Property(e => e.Categories)
                .HasColumnType("character varying")
                .HasConversion(itemsConverter);

            entity.Property(e => e.Ranges)
                .HasColumnType("character varying")
                .HasColumnName("ranges")
                .HasConversion(itemsConverter);
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");
            entity.ToTable("products");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.BrandId)
                .HasColumnName("brand_id");

            entity.Property(e => e.UpcCode)
                .HasColumnType("character varying")
                .HasColumnName("upc_code");

            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.Property(e => e.Category)
                .HasColumnType("character varying")
                .HasColumnName("category");

            entity.Property(e => e.Ranges)
                .HasColumnType("character varying")
                .HasColumnName("ranges")
                .HasConversion(itemsConverter!);

            entity.HasOne(d => d.BrandEntity).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_brand_id_fkey");
        });
    }
}