using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WEBA_ASSIGNMENT.Data;

namespace WEBA_ASSIGNMENT.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20160707120727_setupdb")]
    partial class setupdb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.BrandCategory", b =>
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

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.BrandPhoto", b =>
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

                    b.Property<string>("CreatedById")
                        .IsRequired();

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

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

                    b.HasIndex("BrandId")
                        .IsUnique();

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.ToTable("BrandPhoto");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Brands", b =>
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

                    b.Property<string>("CreatedById")
                        .IsRequired();

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<int?>("NoOfProducts")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("NoOfProducts")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("UpdatedById")
                        .IsRequired();

                    b.HasKey("BrandId")
                        .HasName("PrimaryKey_BrandId");

                    b.HasIndex("BrandName")
                        .IsUnique()
                        .HasName("Brand_BrandName_UniqueConstraint");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.BrandSpecials", b =>
                {
                    b.Property<int>("BrandId");

                    b.Property<int>("SpecialId");

                    b.HasKey("BrandId", "SpecialId")
                        .HasName("BrandSpecials_CompositeKey");

                    b.HasIndex("SpecialId");

                    b.ToTable("BrandSpecials");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Category", b =>
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

                    b.Property<string>("CreatedById")
                        .IsRequired();

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnName("EndDate");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnName("StartDate");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("UpdatedById")
                        .IsRequired();

                    b.Property<int>("VisibilityId")
                        .HasColumnName("VisibilityId")
                        .HasColumnType("int");

                    b.HasKey("CatId")
                        .HasName("PrimaryKey_CatId");

                    b.HasIndex("CatName")
                        .IsUnique()
                        .HasName("Category_CatName_UniqueConstraint");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("UpdatedById");

                    b.HasIndex("VisibilityId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.CategorySpecials", b =>
                {
                    b.Property<int>("CatId");

                    b.Property<int>("SpecialId");

                    b.HasKey("CatId", "SpecialId")
                        .HasName("CategorySpecials_CompositeKey");

                    b.HasIndex("SpecialId");

                    b.ToTable("CategorySpecials");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Metrics", b =>
                {
                    b.Property<int>("MetricId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("MetricId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("CreatedById")
                        .IsRequired();

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<string>("MetricName")
                        .IsRequired()
                        .HasColumnName("MetricName")
                        .HasColumnType("VARCHAR(200)");

                    b.Property<int?>("PMetricId")
                        .HasColumnName("PMetricId")
                        .HasColumnType("int");

                    b.Property<int>("PriceId")
                        .HasColumnName("PriceId")
                        .HasColumnType("int");

                    b.Property<int>("ProdId")
                        .HasColumnName("ProdId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnName("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("UpdatedById")
                        .IsRequired();

                    b.HasKey("MetricId")
                        .HasName("PrimaryKey_MetricId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("PMetricId");

                    b.HasIndex("ProdId");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Metrics");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.PresetMetric", b =>
                {
                    b.Property<int>("PMetricId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PMetricId")
                        .HasColumnType("int");

                    b.Property<string>("MetricSubType")
                        .IsRequired()
                        .HasColumnName("MetricSubType")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("MetricType")
                        .IsRequired()
                        .HasColumnName("MetricType")
                        .HasColumnType("VARCHAR(100)");

                    b.HasKey("PMetricId")
                        .HasName("PrimaryKey_PMetricId");

                    b.ToTable("PresetMetric");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Price", b =>
                {
                    b.Property<int>("PriceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PriceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("CreatedById")
                        .IsRequired();

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<int>("MetricId")
                        .HasColumnName("MetricId")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnName("Value")
                        .HasColumnType("DECIMAL(6,2)");

                    b.HasKey("PriceId")
                        .HasName("PrimaryKey_PriceId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("MetricId")
                        .IsUnique();

                    b.ToTable("Price");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Product", b =>
                {
                    b.Property<int>("ProdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ProdId")
                        .HasColumnType("int");

                    b.Property<int>("BrandId");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("CreatedById")
                        .IsRequired();

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("Description")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<string>("ProdName")
                        .IsRequired()
                        .HasColumnName("ProdName")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<int>("Published")
                        .HasColumnName("Published")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnName("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("SpecialId");

                    b.Property<decimal?>("TOPSPrice")
                        .HasColumnName("Price")
                        .HasColumnType("DECIMAL(10, 2)");

                    b.Property<int?>("ThresholdInvertoryQuantity")
                        .IsRequired()
                        .HasColumnName("ThresholdInventoryQuantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("UpdatedById")
                        .IsRequired();

                    b.HasKey("ProdId")
                        .HasName("PrimaryKey_ProdId");

                    b.HasIndex("BrandId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("SpecialId");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.ProductPhoto", b =>
                {
                    b.Property<int>("ProductPhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ProductPhotoId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("CreatedById")
                        .IsRequired();

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

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

                    b.Property<int>("ProdId");

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

                    b.Property<int>("isPrimaryPhoto")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("isPrimaryPhoto")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("ProductPhotoId")
                        .HasName("PrimaryKey_ProductPhotoId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("ProdId");

                    b.ToTable("ProductPhoto");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.ProductSpecials", b =>
                {
                    b.Property<int>("ProdId");

                    b.Property<int>("SpecialId");

                    b.HasKey("ProdId", "SpecialId")
                        .HasName("ProductSpecials_CompositeKey");

                    b.HasIndex("SpecialId");

                    b.ToTable("ProductSpecials");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Specials", b =>
                {
                    b.Property<int>("SpecialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SpecialId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("CreatedById")
                        .IsRequired();

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnName("EndDate");

                    b.Property<string>("SpecialName")
                        .IsRequired()
                        .HasColumnName("SpecialName")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnName("StartDate");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("UpdatedById")
                        .IsRequired();

                    b.Property<double>("numericalDiscount");

                    b.Property<double>("percentageDiscount");

                    b.HasKey("SpecialId")
                        .HasName("PrimaryKey_SpecialId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Specials");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Visibility", b =>
                {
                    b.Property<int>("VisibilityId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("VisibilityName");

                    b.HasKey("VisibilityId")
                        .HasName("PrimaryKey_VisibilityId");

                    b.ToTable("Visibility");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.BrandCategory", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.Brands", "Brand")
                        .WithMany("BrandCategory")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WEBA_ASSIGNMENT.Models.Category", "Category")
                        .WithMany("BrandCategory")
                        .HasForeignKey("CatId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.BrandPhoto", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.Brands", "Brand")
                        .WithOne("BrandPhoto")
                        .HasForeignKey("WEBA_ASSIGNMENT.Models.BrandPhoto", "BrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Brands", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.BrandSpecials", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.Specials")
                        .WithMany("BrandSpecials")
                        .HasForeignKey("SpecialId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Category", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.Visibility", "Visibility")
                        .WithMany("Categories")
                        .HasForeignKey("VisibilityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.CategorySpecials", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.Specials")
                        .WithMany("CategorySpecials")
                        .HasForeignKey("SpecialId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Metrics", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.PresetMetric", "PresetMetric")
                        .WithMany("Metrics")
                        .HasForeignKey("PMetricId");

                    b.HasOne("WEBA_ASSIGNMENT.Models.Product", "Product")
                        .WithMany("Metrics")
                        .HasForeignKey("ProdId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Price", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.Metrics", "Metric")
                        .WithOne("Price")
                        .HasForeignKey("WEBA_ASSIGNMENT.Models.Price", "MetricId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Product", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.Brands", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.Specials", "Special")
                        .WithMany()
                        .HasForeignKey("SpecialId");

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.ProductPhoto", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.Product", "Product")
                        .WithMany("ProductPhotos")
                        .HasForeignKey("ProdId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.ProductSpecials", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.Specials")
                        .WithMany("ProductSpecials")
                        .HasForeignKey("SpecialId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WEBA_ASSIGNMENT.Models.Specials", b =>
                {
                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("WEBA_ASSIGNMENT.Models.ApplicationUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });
        }
    }
}
