using BrandAndProductDatabase.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProductDatabase.API.DataAccess;

/// <inheritdoc />
public class BrandAndProductDbContext : DbContext
{
    
    /// <summary>The Brands table.</summary>
    public virtual DbSet<BrandEntity> Brands { get; set; } = null!;

    /// <summary>The Products table.</summary>
    public virtual DbSet<ProductEntity> Products { get; set; } = null!;

    /// <inheritdoc />
    public BrandAndProductDbContext() { }
    
    /// <inheritdoc />
    public BrandAndProductDbContext(DbContextOptions<BrandAndProductDbContext> options) : base(options) { }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID=fairwear;Password=fairwear;Host=localhost;Port=5432;Database=fairwear_brand_and_product_database;");

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BrandEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("brands_pkey");

            entity.ToTable("brands");

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_brand_id_fkey");
        });
    }
}
