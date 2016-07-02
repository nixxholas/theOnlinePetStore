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
        public DbSet<ShopUser> ShopUsers { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<Product> Products { get; set; }
        public DbSet<BrandCategory> BrandCategory { get; set; }
        public DbSet<Visibility> Visibilities { get; set; }
        public DbSet<BrandPhoto> BrandPhotos { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=nixh\SQLEXPRESS;database=weba_assignmentdb;Trusted_Connection=True;MultipleActiveResultSets=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }
            // -------------- Defining Product Entity --------------- //
            //modelBuilder.Entity<Product>()
            //    .HasKey(input => input.ProdId)
            //    .HasName("PrimaryKey_ProdId");

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.ProdId)
            //    .HasColumnName("ProdId")
            //    .HasColumnType("int")
            //    .ValueGeneratedOnAdd()
            //    .IsRequired();

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.ProdName)
            //    .HasColumnName("ProdName")
            //    .HasColumnType("VARCHAR(100)")
            //    .IsRequired();

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.Description)
            //    .HasColumnName("Description")
            //    .HasColumnType("VARCHAR(30000)")
            //    .IsRequired();

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.Price)
            //    .HasColumnName("Price")
            //    .HasColumnType("FLOAT(5,2)")
            //    .IsRequired();

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.BrandId)
            //    .HasColumnName("BrandId")
            //    .HasColumnType("int")
            //    .IsRequired();

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.Quantity)
            //    .HasColumnName("Quantity")
            //    .HasColumnType("int")
            //    .IsRequired();

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.ThresholdInvertoryQuantity)
            //    .HasColumnName("ThresholdInventoryQuantity")
            //    .HasColumnType("int")
            //    .IsRequired();

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.Published)
            //    .HasColumnName("Published")
            //    .HasColumnType("int")
            //    .IsRequired();

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.SpecialId)
            //    .HasColumnName("SpecialId")
            //    .HasColumnType("int")
            //    .IsRequired(false);

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.MetricId)
            //    .HasColumnName("MetricId")
            //    .HasColumnType("int")
            //    .IsRequired(false);

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.CreatedAt)
            //    .HasDefaultValueSql("GETDATE()");

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.UpdatedAt)
            //    .HasDefaultValueSql("GETDATE()");

            //modelBuilder.Entity<Product>()
            //    .Property(input => input.DeletedAt)
            //    .IsRequired(false);

            // Foreign Key Initializations
            //modelBuilder.Entity<Product>()
            //    .HasOne(input => input.Brand)
            //    .WithMany(input => input.Products)
            //    .HasForeignKey(input => input.BrandId);

            //modelBuilder.Entity<Product>()
            //    .HasOne(input => input.Special)
            //    .WithMany(input => input.Products)
            //    .HasForeignKey(input => input.SpecialId);

            //modelBuilder.Entity<Product>()
            //    .HasOne(input => input.Metrics)
            //    .WithMany(input => input.Products)
            //    .HasForeignKey(input => input.MetricId);

            // -------------- Defining Product Entity --------------- //
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
                .IsRequired(false);

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

            // -------------- Defining Category Entity --------------- //

            // Make CatId a primary key and an identity column
            modelBuilder.Entity<Category>()
                .HasKey(input => input.CatId)
                .HasName("PrimaryKey_CatId");

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

            // -------------- Defining BrandsOfCategories Entity --------------- //
            modelBuilder.Entity<BrandCategory>()
                .HasKey(input => new { input.BrandId, input.CatId })
                .HasName("BrandsOfCategories_CompositeKey");

            modelBuilder.Entity<BrandCategory>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            // -------------- Defining BrandsOfCategories Entity --------------- //
            // END.

            // -------------- Defining Visibility Entity --------------- //
            modelBuilder.Entity<Visibility>()
                .HasKey(input => input.VisibilityId)
                .HasName("PrimaryKey_VisibilityId");

            // -------------- Defining Visibility Entity --------------- //
            // END.

            // -------------- Defining Consumable Entity --------------- //
            //modelBuilder.Entity<Consumable>()
            //    .HasKey(input => input.ProdId)
            //    .HasName("Consumable_ProdId");

            //modelBuilder.Entity<Consumable>()
            //    .Property(input => input.TypicalAnalysis)
            //    .HasColumnName("TypicalAnalysis")
            //    .HasColumnType("VARCHAR(1000)")
            //    .IsRequired(false);

            //modelBuilder.Entity<Consumable>()
            //    .Property(input => input.GuranteedAnalysis)
            //    .HasColumnName("GuranteedAnalysis")
            //    .HasColumnType("VARCHAR(1000)")
            //    .IsRequired(false);

            //modelBuilder.Entity<Consumable>()
            //    .Property(input => input.Ingredients)
            //    .HasColumnName("Ingredients")
            //    .HasColumnName("VARCHAR(1000)")
            //    .IsRequired(false);

            //modelBuilder.Entity<Consumable>()
            //    .Property(input => input.ActiveIngredients)
            //    .HasColumnName("ActiveIngredients")
            //    .HasColumnType("VARCHAR(1000)")
            //    .IsRequired();

            //modelBuilder.Entity<Consumable>()
            //    .Property(input => input.InActiveIngredients)
            //    .HasColumnName("InActiveIngredients")
            //    .HasColumnType("VARCHAR(1000)")
            //    .IsRequired();

            //----------- Defining Consumable Entity - End --------------

            //----------- Defining User Entity - Start --------------
            //Make the UserId a  Primary Key and Identity column
            modelBuilder.Entity<ShopUser>()
                                        .HasKey(userClass => userClass.UserId)
                                        .HasName("PrimaryKey_UserId");
            modelBuilder.Entity<ShopUser>()
             .Property(userClass => userClass.UserId)
             .HasColumnName("UserId")
             .HasColumnType("int")
             .UseSqlServerIdentityColumn()
             .ValueGeneratedOnAdd()
             .IsRequired();
            modelBuilder.Entity<ShopUser>()
             .Property(userClass => userClass.IdentityCode)
             .HasColumnName("IdentityCode")
             .HasColumnType("VARCHAR(30)")
             .IsRequired(true);
            modelBuilder.Entity<ShopUser>()
             .Property(userClass => userClass.MobileContact)
             .HasColumnName("MobileContact")
             .HasColumnType("VARCHAR(10)")
             .IsRequired(true);
            modelBuilder.Entity<ShopUser>()
             .Property(userClass => userClass.Email)
             .HasColumnName("Email")
             .HasColumnType("VARCHAR(50)")
             .IsRequired(true);
            modelBuilder.Entity<ShopUser>()
             .Property(userClass => userClass.FullName)
             .HasColumnName("FullName")
             .HasColumnType("VARCHAR(100)")
             .IsRequired(true);
            modelBuilder.Entity<ShopUser>()
                .Property(userClass => userClass.CreatedAt)
                .HasDefaultValueSql("GetDate()");
            modelBuilder.Entity<ShopUser>()
                .Property(userClass => userClass.UpdatedAt)
                .HasDefaultValueSql("GetDate()");
            modelBuilder.Entity<ShopUser>()
                .Property(userClass => userClass.DeletedAt)
                .IsRequired(false);

            //Three sets of Many to One relationship between User and ApplicationUser  entity (Start)
            modelBuilder.Entity<ShopUser>()
             .HasOne(userClass => userClass.CreatedBy)
             .WithMany()
             .HasForeignKey(userClass => userClass.CreatedById)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();
            modelBuilder.Entity<ShopUser>()
                .HasOne(userClass => userClass.UpdatedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            modelBuilder.Entity<ShopUser>()
                .HasOne(userClass => userClass.DeletedBy)
                .WithMany()
                .HasForeignKey(userClass => userClass.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);
            //----------- Defining Student Entity - End --------------



            base.OnModelCreating(modelBuilder);

        }
    }
}
