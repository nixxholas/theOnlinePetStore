using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

/**
 * DECLARE @sql NVARCHAR(max)=''

SELECT @sql += ' Drop table [' + TABLE_SCHEMA + '].['+ TABLE_NAME + ']'
FROM   INFORMATION_SCHEMA.TABLES
WHERE  TABLE_TYPE = 'BASE TABLE'

Exec Sp_executesql @sql 

    Note to self,
    After doing ef migrations, I should check my class in the migrations folder
    to organise the columns
    */

namespace WEBACA.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<Product> Products { get; set; }
        public DbSet<BrandCategory> BrandCategory { get; set; }
        public DbSet<Visibility> Visibilities { get; set; }
        public DbSet<BrandPhoto> BrandPhotos { get; set; }
        //public DbSet<Metrics> Metrics { get; set; }
        //public DbSet<Specials> Specials { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = tcp:nixholas.database.windows.net, 1433; Data Source = nixholas.database.windows.net; Initial Catalog = WEBACA1; Persist Security Info = False; User ID = nixholas; Password = Nicholaschen29; Pooling = False; MultipleActiveResultSets = True; Encrypt = True; TrustServerCertificate = False;");
            //optionsBuilder.UseSqlServer(@"Server=NIXH\SQLEXPRESS;Database=WEBACA;Trusted_Connection=True;MultipleActiveResultSets=True");
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            // -------------- Defining Product Entity --------------- //
            //mb.Entity<Product>()
            //    .HasKey(input => input.ProdId)
            //    .HasName("PrimaryKey_ProdId");

            //mb.Entity<Product>()
            //    .Property(input => input.ProdId)
            //    .HasColumnName("ProdId")
            //    .HasColumnType("int")
            //    .ValueGeneratedOnAdd()
            //    .IsRequired();

            //mb.Entity<Product>()
            //    .Property(input => input.ProdName)
            //    .HasColumnName("ProdName")
            //    .HasColumnType("VARCHAR(100)")
            //    .IsRequired();

            //mb.Entity<Product>()
            //    .Property(input => input.Description)
            //    .HasColumnName("Description")
            //    .HasColumnType("VARCHAR(30000)")
            //    .IsRequired();

            //mb.Entity<Product>()
            //    .Property(input => input.Price)
            //    .HasColumnName("Price")
            //    .HasColumnType("FLOAT(5,2)")
            //    .IsRequired();

            //mb.Entity<Product>()
            //    .Property(input => input.BrandId)
            //    .HasColumnName("BrandId")
            //    .HasColumnType("int")
            //    .IsRequired();

            //mb.Entity<Product>()
            //    .Property(input => input.Quantity)
            //    .HasColumnName("Quantity")
            //    .HasColumnType("int")
            //    .IsRequired();

            //mb.Entity<Product>()
            //    .Property(input => input.ThresholdInvertoryQuantity)
            //    .HasColumnName("ThresholdInventoryQuantity")
            //    .HasColumnType("int")
            //    .IsRequired();

            //mb.Entity<Product>()
            //    .Property(input => input.Published)
            //    .HasColumnName("Published")
            //    .HasColumnType("int")
            //    .IsRequired();

            //mb.Entity<Product>()
            //    .Property(input => input.SpecialId)
            //    .HasColumnName("SpecialId")
            //    .HasColumnType("int")
            //    .IsRequired(false);

            //mb.Entity<Product>()
            //    .Property(input => input.MetricId)
            //    .HasColumnName("MetricId")
            //    .HasColumnType("int")
            //    .IsRequired(false);

            //mb.Entity<Product>()
            //    .Property(input => input.CreatedAt)
            //    .HasDefaultValueSql("GETDATE()");

            //mb.Entity<Product>()
            //    .Property(input => input.UpdatedAt)
            //    .HasDefaultValueSql("GETDATE()");

            //mb.Entity<Product>()
            //    .Property(input => input.DeletedAt)
            //    .IsRequired(false);

            // Foreign Key Initializations
            //mb.Entity<Product>()
            //    .HasOne(input => input.Brand)
            //    .WithMany(input => input.Products)
            //    .HasForeignKey(input => input.BrandId);

            //mb.Entity<Product>()
            //    .HasOne(input => input.Special)
            //    .WithMany(input => input.Products)
            //    .HasForeignKey(input => input.SpecialId);

            //mb.Entity<Product>()
            //    .HasOne(input => input.Metrics)
            //    .WithMany(input => input.Products)
            //    .HasForeignKey(input => input.MetricId);

            // -------------- Defining Product Entity --------------- //
            // END.

            // -------------- Defining Brand Entity --------------- //

            // Make BrandId the primary key and an Identity Column
            mb.Entity<Brands>()
                .HasKey(input => input.BrandId)
                .HasName("PrimaryKey_BrandId");

            mb.Entity<Brands>()
                .Property(input => input.BrandId)
                .HasColumnName("BrandId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            mb.Entity<Brands>()
                .Property(input => input.BrandName)
                .HasColumnName("BrandName")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            mb.Entity<Brands>()
                .Property(input => input.NoOfProducts)
                .HasColumnName("NoOfProducts")
                .HasColumnType("int")
                .IsRequired();

            mb.Entity<Brands>()
                .Property(input => input.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            mb.Entity<Brands>()
                .Property(input => input.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            mb.Entity<Brands>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            // Unique Constraint
            mb.Entity<Brands>()
                .HasIndex(input => input.BrandName).IsUnique()
                .HasName("Brand_BrandName_UniqueConstraint");

            // -------------- Defining Brand Entity --------------- //
            // END.

            // -------------- Defining BrandPhoto Entity --------------- //

            mb.Entity<BrandPhoto>()
            .HasKey(BrandPhotoObject => BrandPhotoObject.BrandPhotoId)
            .HasName("PrimaryKey_BrandPhotoId");

            mb.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.BrandPhotoId)
            .HasColumnName("BrandPhotoId")
            .HasColumnType("int")
            .UseSqlServerIdentityColumn()
            .ValueGeneratedOnAdd()
            .IsRequired(true);

            // Created At
            mb.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.CreatedAt)
            .HasDefaultValueSql("GetDate()");

            // Deleted At
            mb.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.DeletedAt)
            .IsRequired(false);

            // Public Cloudinary ID
            mb.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.PublicCloudinaryId)
            .HasColumnName("PublicCloudinaryId")
            .HasColumnType("VARCHAR(100)")
            .HasDefaultValue("")
            .IsRequired(false);

            // Version
            mb.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.Version)
            .HasColumnName("Version")
            .HasColumnType("int")
            .HasDefaultValue(0);


            // Width
            mb.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.Width)
            .HasColumnName("Width")
            .HasColumnType("int")
            .HasDefaultValue(0);
            // Height
            mb.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.Height)
            .HasColumnName("Height")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Format
            mb.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.Format)
            .HasColumnName("Format")
            .HasColumnType("VARCHAR(14)")
            .HasDefaultValue("")
            .IsRequired(true);

            // ImageSize
            mb.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.ImageSize)
            .HasColumnName("ImageSize")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Image URL
            mb.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.Url)
            .HasColumnName("Url")
            .HasColumnType("VARCHAR(300)")
            .HasDefaultValue("")
            .IsRequired(true);

            // Secure Image URL
            mb.Entity<BrandPhoto>()
            .Property(BrandPhotoObject => BrandPhotoObject.SecureUrl)
            .HasColumnName("SecureUrl")
            .HasColumnType("VARCHAR(300)")
            .HasDefaultValue("")
            .IsRequired(false);

            //----------------------------------------------------------------------------
            //Define One to One relationship
            //----------------------------------------------------------------------------
            //Reference: https://readthedocs.com/projects/aspnet-ef/downloads/pdf/latest/
            mb.Entity<Brands>()
            .HasOne(brandsObject => brandsObject.BrandPhoto)
            .WithOne(brandsPhotoObject => brandsPhotoObject.Brand)
            .HasForeignKey<BrandPhoto>(brandsPhotoObject => brandsPhotoObject.BrandId);

            // -------------- Defining BrandPhoto Entity --------------- //
            // END.

            // -------------- Defining Category Entity --------------- //

            // Make CatId a primary key and an identity column
            mb.Entity<Category>()
                .HasKey(input => input.CatId)
                .HasName("PrimaryKey_CatId");

            // Provide the properties of the CatId column
            mb.Entity<Category>()
                .Property(input => input.CatId)
                .HasColumnName("CatId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            // Provide the properties of the CatName Column
            mb.Entity<Category>()
                .Property(input => input.CatName)
                .HasColumnName("CatName")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            // Provide the properties of the NoOfSubCategories Column
            //mb.Entity<Category>()
            //    .Property(input => input.NoOfSubCategories)
            //    .HasColumnName("NoOfSubCategories")
            //    .HasColumnType("int")
            //    .IsRequired();

            // Make sure this is required, we don't want to have
            // a nullpointer exception.
            mb.Entity<Category>()
                .Property(input => input.VisibilityId)
                .HasColumnName("VisibilityId")
                .HasColumnType("int")
                .IsRequired();

            mb.Entity<Category>()
                .Property(input => input.CreatedAt)
                .HasDefaultValueSql("GetDate()");

            mb.Entity<Category>()
                .Property(input => input.UpdatedAt)
                .HasDefaultValueSql("GetDate()");

            mb.Entity<Category>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            // Enforce unique constraint on Category Name
            mb.Entity<Category>()
                .HasIndex(input => input.CatName).IsUnique()
                .HasName("Category_CatName_UniqueConstraint");

            // Foreign Keys
            mb.Entity<Category>()
                .HasOne(input => input.Visibility)
                .WithMany(input => input.Categories)
                .HasForeignKey(input => input.VisibilityId);

            // -------------- Defining Category Entity --------------- //
            // END.

            // -------------- Defining BrandsOfCategories Entity --------------- //
            mb.Entity<BrandCategory>()
                .HasKey(input => new { input.BrandId, input.CatId })
                .HasName("BrandsOfCategories_CompositeKey");

            mb.Entity<BrandCategory>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            // -------------- Defining BrandsOfCategories Entity --------------- //
            // END.

            // -------------- Defining Visibility Entity --------------- //
            mb.Entity<Visibility>()
                .HasKey(input => input.VisibilityId)
                .HasName("PrimaryKey_VisibilityId");

            // -------------- Defining Visibility Entity --------------- //
            // END.

            // -------------- Defining Consumable Entity --------------- //
            //mb.Entity<Consumable>()
            //    .HasKey(input => input.ProdId)
            //    .HasName("Consumable_ProdId");

            //mb.Entity<Consumable>()
            //    .Property(input => input.TypicalAnalysis)
            //    .HasColumnName("TypicalAnalysis")
            //    .HasColumnType("VARCHAR(1000)")
            //    .IsRequired(false);

            //mb.Entity<Consumable>()
            //    .Property(input => input.GuranteedAnalysis)
            //    .HasColumnName("GuranteedAnalysis")
            //    .HasColumnType("VARCHAR(1000)")
            //    .IsRequired(false);

            //mb.Entity<Consumable>()
            //    .Property(input => input.Ingredients)
            //    .HasColumnName("Ingredients")
            //    .HasColumnName("VARCHAR(1000)")
            //    .IsRequired(false);

            //mb.Entity<Consumable>()
            //    .Property(input => input.ActiveIngredients)
            //    .HasColumnName("ActiveIngredients")
            //    .HasColumnType("VARCHAR(1000)")
            //    .IsRequired();

            //mb.Entity<Consumable>()
            //    .Property(input => input.InActiveIngredients)
            //    .HasColumnName("InActiveIngredients")
            //    .HasColumnType("VARCHAR(1000)")
            //    .IsRequired();

            // Foreign key up the primary key


            base.OnModelCreating(mb);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
