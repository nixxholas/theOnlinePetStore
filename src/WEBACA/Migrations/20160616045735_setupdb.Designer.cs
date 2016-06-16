using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using WEBACA.Models;

namespace WEBACA.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20160616045735_setupdb")]
    partial class setupdb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WEBACA.Models.BrandCategory", b =>
                {
                    b.Property<int>("BrandId");

                    b.Property<int>("CatId");

                    b.Property<DateTime?>("DeletedAt");

                    b.HasKey("BrandId", "CatId")
                        .HasAnnotation("Relational:Name", "BrandsOfCategories_CompositeKey");
                });

            modelBuilder.Entity("WEBACA.Models.BrandPhoto", b =>
                {
                    b.Property<int>("BrandPhotoId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "BrandPhotoId")
                        .HasAnnotation("Relational:ColumnType", "int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrandId");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:GeneratedValueSql", "GetDate()");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasAnnotation("Relational:ColumnName", "Format")
                        .HasAnnotation("Relational:ColumnType", "VARCHAR(14)")
                        .HasAnnotation("Relational:DefaultValue", "")
                        .HasAnnotation("Relational:DefaultValueType", "System.String");

                    b.Property<int>("Height")
                        .HasAnnotation("Relational:ColumnName", "Height")
                        .HasAnnotation("Relational:ColumnType", "int")
                        .HasAnnotation("Relational:DefaultValue", "0")
                        .HasAnnotation("Relational:DefaultValueType", "System.Int32");

                    b.Property<int>("ImageSize")
                        .HasAnnotation("Relational:ColumnName", "ImageSize")
                        .HasAnnotation("Relational:ColumnType", "int")
                        .HasAnnotation("Relational:DefaultValue", "0")
                        .HasAnnotation("Relational:DefaultValueType", "System.Int32");

                    b.Property<string>("PublicCloudinaryId")
                        .HasAnnotation("Relational:ColumnName", "PublicCloudinaryId")
                        .HasAnnotation("Relational:ColumnType", "VARCHAR(100)")
                        .HasAnnotation("Relational:DefaultValue", "")
                        .HasAnnotation("Relational:DefaultValueType", "System.String");

                    b.Property<string>("SecureUrl")
                        .HasAnnotation("Relational:ColumnName", "SecureUrl")
                        .HasAnnotation("Relational:ColumnType", "VARCHAR(300)")
                        .HasAnnotation("Relational:DefaultValue", "")
                        .HasAnnotation("Relational:DefaultValueType", "System.String");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasAnnotation("Relational:ColumnName", "Url")
                        .HasAnnotation("Relational:ColumnType", "VARCHAR(300)")
                        .HasAnnotation("Relational:DefaultValue", "")
                        .HasAnnotation("Relational:DefaultValueType", "System.String");

                    b.Property<int>("Version")
                        .HasAnnotation("Relational:ColumnName", "Version")
                        .HasAnnotation("Relational:ColumnType", "int")
                        .HasAnnotation("Relational:DefaultValue", "0")
                        .HasAnnotation("Relational:DefaultValueType", "System.Int32");

                    b.Property<int>("Width")
                        .HasAnnotation("Relational:ColumnName", "Width")
                        .HasAnnotation("Relational:ColumnType", "int")
                        .HasAnnotation("Relational:DefaultValue", "0")
                        .HasAnnotation("Relational:DefaultValueType", "System.Int32");

                    b.HasKey("BrandPhotoId")
                        .HasAnnotation("Relational:Name", "PrimaryKey_BrandPhotoId");
                });

            modelBuilder.Entity("WEBACA.Models.Brands", b =>
                {
                    b.Property<int>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "BrandId")
                        .HasAnnotation("Relational:ColumnType", "int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasAnnotation("Relational:ColumnName", "BrandName")
                        .HasAnnotation("Relational:ColumnType", "VARCHAR(100)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:GeneratedValueSql", "GETDATE()");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<int>("NoOfProducts")
                        .HasAnnotation("Relational:ColumnName", "NoOfProducts")
                        .HasAnnotation("Relational:ColumnType", "int");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:GeneratedValueSql", "GETDATE()");

                    b.HasKey("BrandId")
                        .HasAnnotation("Relational:Name", "PrimaryKey_BrandId");

                    b.HasIndex("BrandName")
                        .IsUnique()
                        .HasAnnotation("Relational:Name", "Brand_BrandName_UniqueConstraint");
                });

            modelBuilder.Entity("WEBACA.Models.Category", b =>
                {
                    b.Property<int>("CatId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "CatId")
                        .HasAnnotation("Relational:ColumnType", "int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CatName")
                        .IsRequired()
                        .HasAnnotation("Relational:ColumnName", "CatName")
                        .HasAnnotation("Relational:ColumnType", "VARCHAR(100)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:GeneratedValueSql", "GetDate()");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<DateTime?>("EndDate");

                    b.Property<DateTime?>("StartDate");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:GeneratedValueSql", "GetDate()");

                    b.Property<int>("VisibilityId")
                        .HasAnnotation("Relational:ColumnName", "VisibilityId")
                        .HasAnnotation("Relational:ColumnType", "int");

                    b.HasKey("CatId")
                        .HasAnnotation("Relational:Name", "PrimaryKey_CatId");

                    b.HasIndex("CatName")
                        .IsUnique()
                        .HasAnnotation("Relational:Name", "Category_CatName_UniqueConstraint");
                });

            modelBuilder.Entity("WEBACA.Models.Visibility", b =>
                {
                    b.Property<int>("VisibilityId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("VisibilityName");

                    b.HasKey("VisibilityId")
                        .HasAnnotation("Relational:Name", "PrimaryKey_VisibilityId");
                });

            modelBuilder.Entity("WEBACA.Models.BrandCategory", b =>
                {
                    b.HasOne("WEBACA.Models.Brands")
                        .WithMany()
                        .HasForeignKey("BrandId");

                    b.HasOne("WEBACA.Models.Category")
                        .WithMany()
                        .HasForeignKey("CatId");
                });

            modelBuilder.Entity("WEBACA.Models.BrandPhoto", b =>
                {
                    b.HasOne("WEBACA.Models.Brands")
                        .WithOne()
                        .HasForeignKey("WEBACA.Models.BrandPhoto", "BrandId");
                });

            modelBuilder.Entity("WEBACA.Models.Category", b =>
                {
                    b.HasOne("WEBACA.Models.Visibility")
                        .WithMany()
                        .HasForeignKey("VisibilityId");
                });
        }
    }
}
