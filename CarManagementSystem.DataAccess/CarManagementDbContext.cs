using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CarManagementSystem.DataAccess;

public partial class CarManagementDbContext : DbContext
{
    public CarManagementDbContext()
    {
    }

    public CarManagementDbContext(DbContextOptions<CarManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CarCompany> CarCompanies { get; set; }

    public virtual DbSet<ElectricVehicle> ElectricVehicles { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VehicleCategory> VehicleCategories { get; set; }

    //private string GetConnectionString()
    //{
    //    IConfiguration configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json", true, true).Build();
    //    return configuration["ConnectionStrings:DefaultConnectionString"];
    //}

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer(GetConnectionString());
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarCompa__3214EC0773ED471F");

            entity.ToTable("CarCompany");

            entity.Property(e => e.CatalogName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ElectricVehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Electric__3214EC07789989CA");

            entity.ToTable("ElectricVehicle");

            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.ImageUrl).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Specification).HasMaxLength(500);
            entity.Property(e => e.Version).HasMaxLength(50);

            entity.HasOne(d => d.CarCompany).WithMany(p => p.ElectricVehicles)
                .HasForeignKey(d => d.CarCompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ElectricV__CarCo__4E88ABD4");

            entity.HasOne(d => d.VehicleCategory).WithMany(p => p.ElectricVehicles)
                .HasForeignKey(d => d.VehicleCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ElectricV__Vehic__4F7CD00D");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC07F4F1FE40");

            entity.ToTable("Feedback");

            entity.Property(e => e.Content).HasMaxLength(500);
            entity.Property(e => e.Datetime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FeedbackType).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__UserId__3C69FB99");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC0753CA7955");

            entity.ToTable("Order");

            entity.Property(e => e.Datetime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK__Order__Promotion__44FF419A");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__UserId__440B1D61");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC075E2EA7CE");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ElectricVehicle).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ElectricVehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Elect__534D60F1");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__52593CB8");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC07A2B4F4FF");

            entity.ToTable("Promotion");

            entity.HasIndex(e => e.Code, "UQ__Promotio__A25C5AA717F097BD").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0721186B37");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534D847EF79").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<VehicleCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VehicleC__3214EC0778FAF3EC");

            entity.ToTable("VehicleCategory");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
