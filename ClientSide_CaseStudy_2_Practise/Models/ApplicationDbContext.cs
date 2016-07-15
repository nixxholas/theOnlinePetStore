using Microsoft.Data.Entity;

namespace ClientSide_CaseStudy_2_Practise.Models
{
    public class ApplicationDbContext : DbContext
    {
    public DbSet<Company> Companies { get; set; }
		public DbSet<CompanyType> CompanyTypes {get;set;}
		public DbSet<Student> Students { get; set; }
		public DbSet<Course> Courses {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=IT-NB147067\SQLEXPRESS;Database=ClientSide_CaseStudyDB_2;Trusted_Connection=True;MultipleActiveResultSets=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //----------- Defining CompanyType Entity - Start --------------
            //Make the CompanyId a  Primary Key and Identity column
            modelBuilder.Entity<CompanyType>()
              .HasKey(input => input.CompanyTypeId)
              .HasName("PrimaryKey_CompanyTypeId");

            //Tell the modelBuilder more about the CompanyId column
            modelBuilder.Entity<CompanyType>()
             .Property(input => input.CompanyTypeId)
             .HasColumnName("CompanyTypeId")
             .HasColumnType("int")
             .UseSqlServerIdentityColumn()
             .ValueGeneratedOnAdd()
             .IsRequired();

            //Tell the modelBuilder more about the CompanyType column
            modelBuilder.Entity<CompanyType>()
             .Property(input => input.TypeName)
             .HasColumnName("TypeName")
             .HasColumnType("VARCHAR(50)")
             .IsRequired();
			
            //Tell the modelBuilder more about the CreatedAt column
            modelBuilder.Entity<CompanyType>()
             .Property(input => input.CreatedAt)
             .HasDefaultValueSql("GetDate()");

            //Tell the modelBuilder more about the UpdatedAt column
            modelBuilder.Entity<CompanyType>()
             .Property(input => input.UpdatedAt)
             .HasDefaultValueSql("GetDate()");

            //Tell the modelBuilder more about the DeletedAt column
            modelBuilder.Entity<CompanyType>()
             .Property(input => input.DeletedAt)
             .IsRequired(false);

            //Enforce unique constraint on TypeName
            modelBuilder.Entity<CompanyType>()
             .HasIndex(input => input.TypeName).IsUnique()
             .HasName("CompanyType_TypeName_UniqueConstraint");
            //Setup one-to-many relationship between CompanyType and Company
            modelBuilder.Entity<Company>()
                .HasOne(input => input.CompanyType)
                .WithMany(input => input.Companies)
                .HasForeignKey(input => input.CompanyTypeId);
            //----------- Defining CompanyType Entity - End --------------

            //----------- Defining Company Entity - Start --------------
            //Make the CompanyId a  Primary Key and Identity column
            modelBuilder.Entity<Company>()
                       .HasKey(input => input.CompanyId)
                       .HasName("PrimaryKey_CompanyId");

            //Tell the modelBuilder more about the CompanyId column
            modelBuilder.Entity<Company>()
             .Property(input => input.CompanyId)
             .HasColumnName("CompanyId")
             .HasColumnType("int")
             .UseSqlServerIdentityColumn()
             .ValueGeneratedOnAdd()
             .IsRequired();

            //Tell the modelBuilder more about the CompanyName column
            modelBuilder.Entity<Company>()
             .Property(input => input.CompanyName)
             .HasColumnName("CompanyName")
             .HasColumnType("VARCHAR(100)")
             .IsRequired();

            //Tell the modelBuilder more about the PostalCode column
            modelBuilder.Entity<Company>()
             .Property(input => input.PostalCode)
             .HasColumnName("PostalCode")
             .HasColumnType("VARCHAR(20)")
             .IsRequired();

            //Tell the modelBuilder more about the Address column
            modelBuilder.Entity<Company>()
             .Property(input => input.Address)
             .HasColumnName("Address")
             .HasColumnType("VARCHAR(100)")
             .IsRequired();

            //Tell the modelBuilder more about the CreatedAt column
            modelBuilder.Entity<Company>()
             .Property(input => input.CreatedAt)
             .HasDefaultValueSql("GetDate()");

            //Tell the modelBuilder more about the UpdatedAt column
            modelBuilder.Entity<Company>()
             .Property(input => input.UpdatedAt)
             .HasDefaultValueSql("GetDate()");

            //Tell the modelBuilder more about the DeletedAt column
            modelBuilder.Entity<Company>()
             .Property(input => input.DeletedAt)
             .IsRequired(false);

            //Enforce unique constraint on Company Name
            modelBuilder.Entity<Company>()
             .HasIndex(input => input.CompanyName).IsUnique()
             .HasName("Company_CompanyName_UniqueConstraint");

            //----------- Defining Company Entity - End --------------

			//----------- Defining Course Entity - Start --------------
            //Make the CourseId a  Primary Key and 
            modelBuilder.Entity<Course>()
                       .HasKey(input => input.CourseId)
                       .HasName("PrimaryKey_CourseId");
			        //Defining general properties of CourseId
            modelBuilder.Entity<Course>()
             .Property(input => input.CourseId)
             .HasColumnName("CourseId")
             .HasColumnType("int")
             .UseSqlServerIdentityColumn()
             .ValueGeneratedOnAdd()
             .IsRequired();
			         //Defining general properties of CourseName
            modelBuilder.Entity<Course>()
             .Property(input => input.CourseName)
             .HasColumnName("CourseName")
             .HasColumnType("VARCHAR(100)")
             .IsRequired();

			         //Defining general properties of CourseAbbreviation
            modelBuilder.Entity<Course>()
             .Property(input => input.CourseAbbreviation)
             .HasColumnName("CourseAbbreviation")
             .HasColumnType("VARCHAR(10)")
             .IsRequired();
		
            modelBuilder.Entity<Course>()
                 .Property(input => input.CreatedAt)
                 .HasDefaultValueSql("GetDate()");
            modelBuilder.Entity<Course>()
                .Property(input => input.UpdatedAt)
                .HasDefaultValueSql("GetDate()");
            modelBuilder.Entity<Course>()
                  .Property(input => input.DeletedAt)
                  .IsRequired(false);

		    //Enforce unique constraint on Course Abbreviation
            modelBuilder.Entity<Course>()
             .HasIndex(input => input.CourseAbbreviation).IsUnique()
             .HasName("Course_CourseAbbreviation_UniqueConstraint");

       //----------- Defining Course Entity - End --------------

		   //----------- Defining Student Entity - Start --------------
           //Make the StudentId a  Primary Key and Identity column
            modelBuilder.Entity<Student>()
             .HasKey(input => input.StudentId)
             .HasName("PrimaryKey_StudentId");

			modelBuilder.Entity<Student>()
             .Property(input => input.StudentId)
             .HasColumnName("StudentId")
             .HasColumnType("int")
             .UseSqlServerIdentityColumn()
             .ValueGeneratedOnAdd()
             .IsRequired();
            modelBuilder.Entity<Student>()
             .Property(input => input.AdmissionId)
             .HasColumnName("AdmissionId")
             .HasColumnType("VARCHAR(10)")
             .IsRequired(true);
            modelBuilder.Entity<Student>()
             .Property(input => input.MobileContact)
             .HasColumnName("MobileContact")
             .HasColumnType("VARCHAR(10)")
             .IsRequired(true);
            modelBuilder.Entity<Student>()
             .Property(input => input.Email)
             .HasColumnName("Email")
             .HasColumnType("VARCHAR(50)")
             .IsRequired(true);
            modelBuilder.Entity<Student>()
             .Property(input => input.FullName)
             .HasColumnName("FullName")
             .HasColumnType("VARCHAR(100)")
             .IsRequired(true);
            modelBuilder.Entity<Student>()
                .Property(input => input.CreatedAt)
                 .HasDefaultValueSql("GetDate()");
            modelBuilder.Entity<Student>()
                .Property(input => input.UpdatedAt)
                 .HasDefaultValueSql("GetDate()");
            modelBuilder.Entity<Student>()
                  .Property(input => input.DeletedAt)
                   .IsRequired(false);
						      //Enforce unique constraint on AdmissionId
            modelBuilder.Entity<Student>()
             .HasIndex(input => input.AdmissionId).IsUnique()
             .HasName("Student_AdmissionId_UniqueConstraint");
            //one-to-many 
            modelBuilder.Entity<Student>()
                .HasOne(input => input.Course)
                .WithMany(input => input.Students)
                .HasForeignKey(input => input.CourseId);


            //----------- Defining Student Entity - End --------------

            base.OnModelCreating(modelBuilder);

        }//End of onModelCreating() method
    }//End of ApplicationDbContext class definition
}//End of namespace definition
