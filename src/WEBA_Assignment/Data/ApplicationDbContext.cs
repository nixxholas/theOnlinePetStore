using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

/* Additional using statements besides the defaults (Start) */
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
/* Additional using statements besides the defaults (End) */
using WEBA_ASSIGNMENT.Models;

namespace WEBA_ASSIGNMENT.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Metrics> Metrics { get; set; }
        public DbSet<PresetMetric> PresetMetrics { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<BrandCategory> BrandCategory { get; set; }
        public DbSet<Specials> Specials { get; set; }
        public DbSet<ProductSpecials> ProductSpecials { get; set; }
        public DbSet<BrandSpecials> BrandSpecials { get; set; }
        public DbSet<CategorySpecials> CategorySpecials { get; set; }
        public DbSet<Visibility> Visibilities { get; set; }
        public DbSet<BrandPhoto> BrandPhotos { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=nixh\SQLEXPRESS;database=WEBA_ASSIGNMENTDB;Trusted_Connection=True;MultipleActiveResultSets=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }

            // Unique Constraints Enforcement undone

            // ---------------- Defining Price Entity ----------------- //

            modelBuilder.Entity<Price>()
                .HasKey(input => input.PriceId)
                .HasName("PrimaryKey_Price_PriceId");

            // Provide the properties of the PriceId column
            modelBuilder.Entity<Price>()
                .Property(input => input.PriceId)
                .HasColumnName("PriceId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<Price>()
                .Property(input => input.MetricId)
                .HasColumnName("MetricId")
                .HasColumnType("int")
                .IsRequired();

            modelBuilder.Entity<Price>()
                .Property(input => input.Value)
                .HasColumnName("Value")
                .HasColumnType("money")
                .IsRequired();

            modelBuilder.Entity<Price>()
                .Property(input => input.RRP)
                .HasColumnName("RRP")
                .HasColumnType("money")
                .IsRequired(false);

            // Two sets of Many to One relationship between User and ApplicationUser  entity (Start)
            modelBuilder.Entity<Price>()
             .HasOne(userClass => userClass.CreatedBy)
             .WithMany()
             .HasForeignKey(userClass => userClass.CreatedById)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();

            modelBuilder.Entity<Price>()
                .HasOne(userClass => userClass.DeletedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------- Defining Price Entity --------------- //
            // END.

            // ---------------- Defining Status Entity ----------------- //

            modelBuilder.Entity<Status>()
                .HasKey(input => input.StatusId)
                .HasName("PrimaryKey_Status_StatusId");

            // Provide the properties of the StatusId column
            modelBuilder.Entity<Status>()
                .Property(input => input.StatusId)
                .HasColumnName("StatusId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<Status>()
                .Property(input => input.StatusName)
                .HasColumnName("StatusName")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            // -------------- Defining Status Entity --------------- //
            // END.

            // -------------- Defining Visibility Entity --------------- //
            modelBuilder.Entity<Visibility>()
                .HasKey(input => input.VisibilityId)
                .HasName("PrimaryKey_Visibility_VisibilityId");

            // -------------- Defining Visibility Entity --------------- //
            // END.

            // -------------- Defining Category Entity --------------- //

            // Make CatId a primary key and an identity column
            modelBuilder.Entity<Category>()
                .HasKey(input => input.CatId)
                .HasName("PrimaryKey_Category_CatId");

            // Provide the properties of the CatId column
            modelBuilder.Entity<Category>()
                .Property(input => input.CatId)
                .HasColumnName("CatId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            // Provide the properties of the CatName Column
            modelBuilder.Entity<Category>()
                .Property(input => input.CatName)
                .HasColumnName("CatName")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            // Provide the properties of the NoOfSubCategories Column
            //modelBuilder.Entity<Category>()
            //    .Property(input => input.NoOfSubCategories)
            //    .HasColumnName("NoOfSubCategories")
            //    .HasColumnType("int")
            //    .IsRequired();

            // Make sure this is required, we don't want to have
            // a nullpointer exception.
            modelBuilder.Entity<Category>()
                .Property(input => input.VisibilityId)
                .HasColumnName("VisibilityId")
                .HasColumnType("int")
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(input => input.StartDate)
                .HasColumnName("StartDate")
                .IsRequired(false);

            modelBuilder.Entity<Category>()
                .Property(input => input.EndDate)
                .HasColumnName("EndDate")
                .IsRequired(false);

            modelBuilder.Entity<Category>()
                .Property(input => input.CreatedAt)
                .HasDefaultValueSql("GetDate()");

            modelBuilder.Entity<Category>()
                .Property(input => input.UpdatedAt)
                .HasDefaultValueSql("GetDate()");

            modelBuilder.Entity<Category>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            // Enforce unique constraint on Category Name
            modelBuilder.Entity<Category>()
                .HasIndex(input => input.CatName).IsUnique()
                .HasName("Category_CatName_UniqueConstraint");

            // Foreign Keys
            modelBuilder.Entity<Category>()
                .HasOne(input => input.Visibility)
                .WithMany(input => input.Categories)
                .HasForeignKey(input => input.VisibilityId);

            //Three sets of Many to One relationship between User and ApplicationUser  entity (Start)
            modelBuilder.Entity<Category>()
             .HasOne(userClass => userClass.CreatedBy)
             .WithMany()
             .HasForeignKey(userClass => userClass.CreatedById)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();
            modelBuilder.Entity<Category>()
                .HasOne(userClass => userClass.UpdatedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            modelBuilder.Entity<Category>()
                .HasOne(userClass => userClass.DeletedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------- Defining Category Entity --------------- //
            // END.

            // -------------- Defining Brand Entity --------------- //

            // Make BrandId the primary key and an Identity Column
            modelBuilder.Entity<Brands>()
                .HasKey(input => input.BrandId)
                .HasName("PrimaryKey_BrandId");

            modelBuilder.Entity<Brands>()
                .Property(input => input.BrandId)
                .HasColumnName("BrandId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<Brands>()
                .Property(input => input.BrandName)
                .HasColumnName("BrandName")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            modelBuilder.Entity<Brands>()
                .Property(input => input.NoOfProducts)
                .HasColumnName("NoOfProducts")
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            modelBuilder.Entity<Brands>()
                .Property(input => input.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Brands>()
                .Property(input => input.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Brands>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            // Unique Constraint

            modelBuilder.Entity<Brands>()
                .HasIndex(input => input.BrandName).IsUnique()
                .HasName("Brand_BrandName_UniqueConstraint");

            // Enforce One to Many relationship between products

            modelBuilder.Entity<Brands>()
                .HasMany(input => input.Products)
                .WithOne(input => input.Brand)
                .HasForeignKey(input => input.BrandId);
            
            //Three sets of Many to One relationship between User and ApplicationUser  entity (Start)
            modelBuilder.Entity<Brands>()
             .HasOne(userClass => userClass.CreatedBy)
             .WithMany()
             .HasForeignKey(userClass => userClass.CreatedById)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();

            modelBuilder.Entity<Brands>()
                .HasOne(userClass => userClass.UpdatedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Brands>()
                .HasOne(userClass => userClass.DeletedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------- Defining Brand Entity --------------- //
            // END.

            // -------------- Defining BrandPhoto Entity --------------- //

            modelBuilder.Entity<BrandPhoto>()
            .HasKey(BrandPhotoObject => BrandPhotoObject.BrandPhotoId)
            .HasName("PrimaryKey_BrandPhotoId");

            modelBuilder.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.BrandPhotoId)
            .HasColumnName("BrandPhotoId")
            .HasColumnType("int")
            .UseSqlServerIdentityColumn()
            .ValueGeneratedOnAdd()
            .IsRequired(true);

            // Created At
            modelBuilder.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.CreatedAt)
            .HasDefaultValueSql("GetDate()");

            // Deleted At
            modelBuilder.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.DeletedAt)
            .IsRequired(false);

            // Public Cloudinary ID
            modelBuilder.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.PublicCloudinaryId)
            .HasColumnName("PublicCloudinaryId")
            .HasColumnType("VARCHAR(100)")
            .HasDefaultValue("")
            .IsRequired(false);

            // Version
            modelBuilder.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.Version)
            .HasColumnName("Version")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Width
            modelBuilder.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.Width)
            .HasColumnName("Width")
            .HasColumnType("int")
            .HasDefaultValue(0);
            // Height
            modelBuilder.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.Height)
            .HasColumnName("Height")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Format
            modelBuilder.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.Format)
            .HasColumnName("Format")
            .HasColumnType("VARCHAR(14)")
            .HasDefaultValue("")
            .IsRequired(true);

            // ImageSize
            modelBuilder.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.ImageSize)
            .HasColumnName("ImageSize")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Image URL
            modelBuilder.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.Url)
            .HasColumnName("Url")
            .HasColumnType("VARCHAR(300)")
            .HasDefaultValue("")
            .IsRequired(true);

            // Secure Image URL
            modelBuilder.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.SecureUrl)
            .HasColumnName("SecureUrl")
            .HasColumnType("VARCHAR(300)")
            .HasDefaultValue("")
            .IsRequired(false);

            // Two sets of Many to One relationship between User and ApplicationUser  entity (Start)
            modelBuilder.Entity<BrandPhoto>()
             .HasOne(userClass => userClass.CreatedBy)
             .WithMany()
             .HasForeignKey(userClass => userClass.CreatedById)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();

            modelBuilder.Entity<BrandPhoto>()
                .HasOne(userClass => userClass.DeletedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            //----------------------------------------------------------------------------
            //Define One to One relationship
            //----------------------------------------------------------------------------
            //Reference: https://readthedocs.com/projects/aspnet-ef/downloads/pdf/latest/
            modelBuilder.Entity<Brands>()
            .HasOne(brandsObject => brandsObject.BrandPhoto)
            .WithOne(brandsPhotoObject => brandsPhotoObject.Brand)
            .HasForeignKey<BrandPhoto>(brandsPhotoObject => brandsPhotoObject.BrandId);

            // -------------- Defining BrandPhoto Entity --------------- //
            // END.

            // -------------- Defining BrandsOfCategories Entity --------------- //

            modelBuilder.Entity<BrandCategory>()
                .HasKey(input => new { input.BrandId, input.CatId })
                .HasName("BrandsOfCategories_CompositeKey");

            modelBuilder.Entity<BrandCategory>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            // -------------- Defining BrandsOfCategories Entity --------------- //
            // END.

            // -------------- Defining Specials Entity ----------------- //

            modelBuilder.Entity<Specials>()
                .HasKey(input => input.SpecialId)
                .HasName("PrimaryKey_SpecialId");

            modelBuilder.Entity<Specials>()
                .Property(input => input.SpecialId)
                .HasColumnName("SpecialId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<Specials>()
                .Property(input => input.SpecialName)
                .HasColumnName("SpecialName")
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();

            modelBuilder.Entity<Specials>()
                .Property(input => input.StartDate)
                .HasColumnName("StartDate")
                .IsRequired(false);

            modelBuilder.Entity<Specials>()
                .Property(input => input.EndDate)
                .HasColumnName("EndDate")
                .IsRequired(false);

            modelBuilder.Entity<Specials>()
                .Property(input => input.CreatedAt)
                .HasDefaultValueSql("GetDate()");

            modelBuilder.Entity<Specials>()
                .Property(input => input.UpdatedAt)
                .HasDefaultValueSql("GetDate()");

            modelBuilder.Entity<Specials>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);
            
            //Three sets of Many to One relationship between User and ApplicationUser  entity (Start)
            modelBuilder.Entity<Specials>()
             .HasOne(userClass => userClass.CreatedBy)
             .WithMany()
             .HasForeignKey(userClass => userClass.CreatedById)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();
            modelBuilder.Entity<Specials>()
                .HasOne(userClass => userClass.UpdatedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            modelBuilder.Entity<Specials>()
                .HasOne(userClass => userClass.DeletedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            // ------------ End of Defining Specials Entity ------------ //

            // -------------- Defining CategorySpecials Entity ----------------- //

            modelBuilder.Entity<CategorySpecials>()
                .HasKey(input => new { input.CatId, input.SpecialId })
                .HasName("CategorySpecials_CompositeKey");
            
            // ------------ End of Defining CategorySpecials Entity ------------ //

            // -------------- Defining BrandSpecials Entity ----------------- //

            modelBuilder.Entity<BrandSpecials>()
                .HasKey(input => new { input.BrandId, input.SpecialId })
                .HasName("BrandSpecials_CompositeKey");

            // ------------ End of Defining BrandSpecials Entity ------------ //

            // -------------- Defining ProductSpecials Entity ----------------- //

            modelBuilder.Entity<ProductSpecials>()
                .HasKey(input => new { input.ProdId, input.SpecialId })
                .HasName("ProductSpecials_CompositeKey");

            // ------------ End of Defining ProductSpecials Entity ------------ //

            // -------------- Defining Price Entity ----------------- //

            modelBuilder.Entity<Price>()
                .HasKey(input => input.PriceId)
                .HasName("PrimaryKey_PriceId");

            modelBuilder.Entity<Price>()
                .Property(input => input.PriceId)
                .HasColumnName("PriceId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<Price>()
                .Property(input => input.MetricId)
                .HasColumnName("MetricId")
                .HasColumnType("int")
                .IsRequired();

            modelBuilder.Entity<Price>()
                .Property(input => input.Value)
                .HasColumnName("Value")
                .HasColumnType("DECIMAL(6,2)")
                .IsRequired();

            modelBuilder.Entity<Price>()
                .Property(input => input.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Price>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            // Foreign Key Relations

            modelBuilder.Entity<Price>()
                .HasOne(input => input.Metric)
                .WithOne(input => input.Price)
                .HasForeignKey<Metrics>(input => input.PriceId);

            // Two sets of Many to One relationship between User and ApplicationUser  entity (Start)
            modelBuilder.Entity<Price>()
             .HasOne(userClass => userClass.CreatedBy)
             .WithMany()
             .HasForeignKey(userClass => userClass.CreatedById)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();

            modelBuilder.Entity<Price>()
                .HasOne(userClass => userClass.DeletedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            // ------------ End of Defining Price Entity ------------ //

            // --------------- Defining Metric Entity --------------- //

            modelBuilder.Entity<Metrics>()
                .HasKey(input => input.MetricId)
                .HasName("PrimaryKey_MetricId");

            modelBuilder.Entity<Metrics>()
                .Property(input => input.MetricId)
                .HasColumnName("MetricId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<Metrics>()
                .Property(input => input.ProdId)
                .HasColumnName("ProdId")
                .HasColumnType("int")
                .IsRequired();

            modelBuilder.Entity<Metrics>()
                .Property(input => input.PMetricId)
                .HasColumnName("PMetricId")
                .HasColumnType("int")
                .IsRequired(false);

            modelBuilder.Entity<Metrics>()
                .Property(input => input.MetricAmount)
                .HasColumnName("MetricAmount")
                .HasColumnType("int")
                // 0 Shows that the user entered nothing
                .HasDefaultValue(0) // Give the default a 1 if user does not implement any
                .IsRequired();

            modelBuilder.Entity<Metrics>()
                .Property(input => input.MetricType)
                .HasColumnName("MetricName")
                .HasColumnType("VARCHAR(200)")
                .IsRequired();

            modelBuilder.Entity<Metrics>()
                .Property(input => input.Quantity)
                .HasColumnName("Quantity")
                .HasColumnType("int")
                .IsRequired();

            modelBuilder.Entity<Metrics>()
                .Property(input => input.PriceId)
                .HasColumnName("PriceId")
                .HasColumnType("int")
                .IsRequired();

            modelBuilder.Entity<Metrics>()
                .Property(input => input.StatusId)
                .HasColumnName("StatusId")
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            modelBuilder.Entity<Metrics>()
                .Property(input => input.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Metrics>()
                .Property(input => input.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Metrics>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            // Foreign Key Relationships

            modelBuilder.Entity<Metrics>()
                .HasOne(input => input.PresetMetric)
                .WithMany(input => input.Metrics)
                .HasForeignKey(input => input.PMetricId);

            modelBuilder.Entity<Metrics>()
                .HasOne(input => input.Price)
                .WithOne(input => input.Metric)
                .HasForeignKey<Price>(input => input.MetricId);

            modelBuilder.Entity<Metrics>()
                .HasOne(input => input.Status)
                .WithMany(input => input.Metrics)
                .HasForeignKey(input => input.StatusId);

            //Three sets of Many to One relationship between User and ApplicationUser  entity (Start)
            modelBuilder.Entity<Metrics>()
             .HasOne(userClass => userClass.CreatedBy)
             .WithMany()
             .HasForeignKey(userClass => userClass.CreatedById)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();
            modelBuilder.Entity<Metrics>()
                .HasOne(userClass => userClass.UpdatedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            modelBuilder.Entity<Metrics>()
                .HasOne(userClass => userClass.DeletedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------- End of Defining Metric Entity --------- //

            // -------------- Defining PresetMetric Entity ---------- //

            modelBuilder.Entity<PresetMetric>()
                .HasKey(input => input.PMetricId)
                .HasName("PrimaryKey_PMetricId");

            modelBuilder.Entity<PresetMetric>()
                .Property(input => input.PMetricId)
                .HasColumnName("PMetricId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<PresetMetric>()
                .Property(input => input.MetricType)
                .HasColumnName("MetricType")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            modelBuilder.Entity<PresetMetric>()
                .Property(input => input.MetricSubType)
                .HasColumnName("MetricSubType")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            modelBuilder.Entity<PresetMetric>()
                .Property(input => input.MetricCharacter)
                .HasColumnName("MetricCharacter")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            // No need to have any user tracking data as there
            // won't be any chance that the admin will be able to 
            // change a single shit unless the SI Unit had a 
            // massive revolution.

            // -------- End of Defining PresetMetric Entity -------- //

            // -------------- Defining Product Entity --------------- //
            modelBuilder.Entity<Product>()
                .HasKey(input => input.ProdId)
                .HasName("PrimaryKey_Product_ProdId");

            modelBuilder.Entity<Product>()
                .Property(input => input.ProdId)
                .HasColumnName("ProdId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(input => input.BrandId)
                .HasColumnName("BrandId")
                .HasColumnType("int")
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(input => input.ProdName)
                .HasColumnName("ProdName")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(input => input.Description)
                .HasColumnName("Description")
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired(false);

            // Quantity will be generated from the metrics web api
            modelBuilder.Entity<Product>()
                .Property(input => input.Quantity)
                .HasColumnName("Quantity")
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(input => input.ThresholdInvertoryQuantity)
                .HasColumnName("ThresholdInventoryQuantity")
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(input => input.Published)
                .HasColumnName("Published")
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(input => input.isConsumable)
                .HasColumnName("isConsumable")
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(input => input.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Product>()
                .Property(input => input.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Product>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            // Unique Constraint

            modelBuilder.Entity<Product>()
                .HasIndex(input => input.ProdName).IsUnique()
                .HasName("Product_ProdName_UniqueConstraint");

            //Foreign Key Initializations

            modelBuilder.Entity<Product>()
                .HasOne(input => input.Brand)
                .WithMany(input => input.Products)
                .HasForeignKey(input => input.BrandId);

            modelBuilder.Entity<Product>()
                .HasMany(input => input.Metrics)
                .WithOne(input => input.Product)
                .HasForeignKey(input => input.ProdId);

            //Three sets of Many to One relationship between User and ApplicationUser  entity (Start)
            modelBuilder.Entity<Product>()
             .HasOne(userClass => userClass.CreatedBy)
             .WithMany()
             .HasForeignKey(userClass => userClass.CreatedById)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();

            modelBuilder.Entity<Product>()
                .HasOne(userClass => userClass.UpdatedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasOne(userClass => userClass.DeletedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------- Defining Product Entity --------------- //
            // END.


            // -------------- Defining ProductPhoto Entity --------------- //

            modelBuilder.Entity<ProductPhoto>()
            .HasKey(ProductPhotoObject => ProductPhotoObject.ProductPhotoId)
            .HasName("PrimaryKey_ProductPhoto_ProductPhotoId");

            modelBuilder.Entity<ProductPhoto>()
            .Property(ProductPhotoObject => ProductPhotoObject.ProductPhotoId)
            .HasColumnName("ProductPhotoId")
            .HasColumnType("int")
            .UseSqlServerIdentityColumn()
            .ValueGeneratedOnAdd()
            .IsRequired(true);

            // Created At
            modelBuilder.Entity<ProductPhoto>()
            .Property(ProductPhotoObject => ProductPhotoObject.CreatedAt)
            .HasDefaultValueSql("GetDate()");

            // Deleted At
            modelBuilder.Entity<ProductPhoto>()
            .Property(ProductPhotoObject => ProductPhotoObject.DeletedAt)
            .IsRequired(false);

            // Public Cloudinary ID
            modelBuilder.Entity<ProductPhoto>()
            .Property(ProductPhotoObject => ProductPhotoObject.PublicCloudinaryId)
            .HasColumnName("PublicCloudinaryId")
            .HasColumnType("VARCHAR(100)")
            .HasDefaultValue("")
            .IsRequired(false);

            // Version
            modelBuilder.Entity<ProductPhoto>()
            .Property(ProductPhotoObject => ProductPhotoObject.Version)
            .HasColumnName("Version")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Width
            modelBuilder.Entity<ProductPhoto>()
            .Property(ProductPhotoObject => ProductPhotoObject.Width)
            .HasColumnName("Width")
            .HasColumnType("int")
            .HasDefaultValue(0);
            // Height
            modelBuilder.Entity<ProductPhoto>()
            .Property(ProductPhotoObject => ProductPhotoObject.Height)
            .HasColumnName("Height")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Format
            modelBuilder.Entity<ProductPhoto>()
            .Property(ProductPhotoObject => ProductPhotoObject.Format)
            .HasColumnName("Format")
            .HasColumnType("VARCHAR(14)")
            .HasDefaultValue("")
            .IsRequired(true);

            // ImageSize
            modelBuilder.Entity<ProductPhoto>()
            .Property(ProductPhotoObject => ProductPhotoObject.ImageSize)
            .HasColumnName("ImageSize")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Image URL
            modelBuilder.Entity<ProductPhoto>()
            .Property(ProductPhotoObject => ProductPhotoObject.Url)
            .HasColumnName("Url")
            .HasColumnType("VARCHAR(300)")
            .HasDefaultValue("")
            .IsRequired(true);

            // Secure Image URL
            modelBuilder.Entity<ProductPhoto>()
            .Property(ProductPhotoObject => ProductPhotoObject.SecureUrl)
            .HasColumnName("SecureUrl")
            .HasColumnType("VARCHAR(300)")
            .HasDefaultValue("")
            .IsRequired(false);

            modelBuilder.Entity<ProductPhoto>()
                .Property(ProductPhotoObject => ProductPhotoObject.isPrimaryPhoto)
                .HasColumnName("isPrimaryPhoto")
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            //----------------------------------------------------------------------------
            // Define One to Many relationship between Product and ProductPhoto
            //
            // This ensures that many ProductPhoto objects/rows can be directed to just
            // one entity of Product.
            //----------------------------------------------------------------------------

            modelBuilder.Entity<ProductPhoto>()
                .HasOne(input => input.Product)
                .WithMany(input => input.ProductPhotos)
                .HasForeignKey(input => input.ProdId);

            //Three sets of Many to One relationship between User and ApplicationUser  entity (Start)
            modelBuilder.Entity<ProductPhoto>()
             .HasOne(userClass => userClass.CreatedBy)
             .WithMany()
             .HasForeignKey(userClass => userClass.CreatedById)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();

            modelBuilder.Entity<ProductPhoto>()
                .HasOne(userClass => userClass.DeletedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------- Defining ProductPhoto Entity --------------- //
            // END.

            // -------------- Defining Consumable Entity --------------- //
            modelBuilder.Entity<Consumable>()
                .HasKey(input => input.ConsumableId)
                .HasName("PrimaryKey_ConsumableId");

            modelBuilder.Entity<Consumable>()
            .Property(input => input.ConsumableId)
            .HasColumnName("ConsumableId")
            .HasColumnType("int")
            .UseSqlServerIdentityColumn()
            .ValueGeneratedOnAdd()
            .IsRequired(true);

            modelBuilder.Entity<Consumable>()
                .Property(input => input.TypicalAnalysis)
                .HasColumnName("TypicalAnalysis")
                .HasColumnType("VARCHAR(1000)")
                .IsRequired(false);

            modelBuilder.Entity<Consumable>()
                .Property(input => input.GuranteedAnalysis)
                .HasColumnName("GuranteedAnalysis")
                .HasColumnType("VARCHAR(1000)")
                .IsRequired(false);

            modelBuilder.Entity<Consumable>()
                .Property(input => input.Ingredients)
                .HasColumnName("Ingredients")
                .HasColumnName("VARCHAR(1000)")
                .IsRequired(false);

            modelBuilder.Entity<Consumable>()
                .Property(input => input.ActiveIngredients)
                .HasColumnName("ActiveIngredients")
                .HasColumnType("VARCHAR(1000)")
                .IsRequired(false);

            modelBuilder.Entity<Consumable>()
                .Property(input => input.InActiveIngredients)
                .HasColumnName("InActiveIngredients")
                .HasColumnType("VARCHAR(1000)")
                .IsRequired(false);

            // Foreign Key initializations

            modelBuilder.Entity<Consumable>()
                .HasOne(input => input.Product)
                .WithOne(input => input.Consumable)
                .HasForeignKey<Consumable>(input => input.ProdId);

            //----------- Defining Consumable Entity - End --------------

            base.OnModelCreating(modelBuilder);

        }
    }
}
