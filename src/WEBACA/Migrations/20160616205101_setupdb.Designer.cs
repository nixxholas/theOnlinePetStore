using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WEBACA.Models;

namespace WEBACA.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20160616205101_setupdb")]
    partial class setupdb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20901")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WEBACA.Models.BrandCategory", b =>
                {
                    b.Property<int>("BrandId");

                    b.Property<int>("CatId");

                    b.Property<DateTime?>("DeletedAt");

                    b.HasKey("BrandId", "CatId")
                        .HasName("BrandsOfCategories_CompositeKey");

                    b.HasIndex("BrandId");

                    b.HasIndex("CatId");

                    b.ToTable("BrandCategory");
                });

            modelBuilder.Entity("WEBACA.Models.BrandPhoto", b =>
                {
                    b.Property<int>("BrandPhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("BrandPhotoId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrandId");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("Format")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Format")
                        .HasColumnType("VARCHAR(14)")
                        .HasDefaultValue("");

                    b.Property<int>("Height")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Height")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("ImageSize")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ImageSize")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("PublicCloudinaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PublicCloudinaryId")
                        .HasColumnType("VARCHAR(100)")
                        .HasDefaultValue("");

                    b.Property<string>("SecureUrl")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SecureUrl")
                        .HasColumnType("VARCHAR(300)")
                        .HasDefaultValue("");

                    b.Property<string>("Url")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Url")
                        .HasColumnType("VARCHAR(300)")
                        .HasDefaultValue("");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Version")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Width")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Width")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("BrandPhotoId")
                        .HasName("PrimaryKey_BrandPhotoId");

                    b.HasIndex("BrandId");

                    b.ToTable("BrandPhoto");
                });

            modelBuilder.Entity("WEBACA.Models.Brands", b =>
                {
                    b.Property<int>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("BrandId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnName("BrandName")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<int>("NoOfProducts")
                        .HasColumnName("NoOfProducts")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("BrandId")
                        .HasName("PrimaryKey_BrandId");

                    b.HasIndex("BrandName")
                        .IsUnique()
                        .HasName("Brand_BrandName_UniqueConstraint");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("WEBACA.Models.Category", b =>
                {
                    b.Property<int>("CatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CatId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CatName")
                        .IsRequired()
                        .HasColumnName("CatName")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<DateTime?>("EndDate");

                    b.Property<DateTime?>("StartDate");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<int>("VisibilityId")
                        .HasColumnName("VisibilityId")
                        .HasColumnType("int");

                    b.HasKey("CatId")
                        .HasName("PrimaryKey_CatId");

                    b.HasIndex("CatName")
                        .IsUnique()
                        .HasName("Category_CatName_UniqueConstraint");

                    b.HasIndex("VisibilityId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("WEBACA.Models.Visibility", b =>
                {
                    b.Property<int>("VisibilityId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("VisibilityName");

                    b.HasKey("VisibilityId")
                        .HasName("PrimaryKey_VisibilityId");

                    b.ToTable("Visibility");
                });

            modelBuilder.Entity("WEBACA.Models.BrandCategory", b =>
                {
                    b.HasOne("WEBACA.Models.Brands")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WEBACA.Models.Category")
                        .WithMany()
                        .HasForeignKey("CatId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WEBACA.Models.BrandPhoto", b =>
                {
                    b.HasOne("WEBACA.Models.Brands")
                        .WithOne()
                        .HasForeignKey("WEBACA.Models.BrandPhoto", "BrandId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WEBACA.Models.Category", b =>
                {
                    b.HasOne("WEBACA.Models.Visibility")
                        .WithMany()
                        .HasForeignKey("VisibilityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
