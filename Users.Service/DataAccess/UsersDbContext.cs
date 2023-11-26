using Microsoft.EntityFrameworkCore;
using Users.Service.Models.Entity;

namespace Users.Service.DataAccess;

/// <inheritdoc />
public class UsersDbContext : DbContext
{
    /// <inheritdoc />
    public UsersDbContext()
    {
    }

    /// <inheritdoc />
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }

    /// <summary>The Products table.</summary>
    public virtual DbSet<ProductEntity> Products { get; set; } = null!;

    /// <summary>The Users table.</summary>
    public virtual DbSet<UserEntity> Users { get; set; } = null!;

    /// <summary>The User Experience table.</summary>
    public virtual DbSet<UserExperienceEntity> UserExperiences { get; set; } = null!;

    /// <summary>The User History table.</summary>
    public virtual DbSet<UserProductHistoryEntity> UserProductHistories { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Rating).HasColumnName("rating");
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.FirebaseId, "users_firebase_id_index");

            entity.HasIndex(e => e.FirebaseId, "users_firebase_id_key").IsUnique();

            entity.HasIndex(e => e.Id, "users_id_index").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.FirebaseId)
                .HasColumnType("character varying")
                .HasColumnName("firebase_id");
            entity.Property(e => e.LanguagePreferences)
                .HasDefaultValueSql("'English'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("language_preferences");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.Theme)
                .HasDefaultValueSql("'System default'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("theme");
            entity.Property(e => e.Username)
                .HasColumnType("character varying")
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserExperienceEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_experience_pkey");

            entity.ToTable("user_experience");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Level)
                .HasDefaultValueSql("0")
                .HasColumnName("level");
            entity.Property(e => e.Score)
                .HasDefaultValueSql("0")
                .HasColumnName("score");
            entity.Property(e => e.Todos)
                .HasDefaultValueSql("'{0,0,0}'::integer[]")
                .HasColumnName("todos");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.UserEntity).WithMany(p => p.UserExperiences)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_experience_user_id_fkey");
        });

        modelBuilder.Entity<UserProductHistoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_product_history_pkey");

            entity.ToTable("user_product_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ProductEntity).WithMany(p => p.UserProductHistories)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("user_product_history_product_id_fkey");

            entity.HasOne(d => d.UserEntity).WithMany(p => p.UserProductHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_product_history_user_id_fkey");
        });
    }
}