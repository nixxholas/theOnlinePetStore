using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace ClientSide_CaseStudy_2_Practise.Migrations
{
    public partial class setupdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyType",
                columns: table => new
                {
                    CompanyTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
										TypeName = table.Column<string>(type: "VARCHAR(50)", nullable: false),
										CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
										DeletedAt = table.Column<DateTime>(nullable: true)
								},
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_CompanyTypeId", x => x.CompanyTypeId);
                });
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CourseAbbreviation = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    CourseName = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
										DeletedAt = table.Column<DateTime>(nullable: true),
								},
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_CourseId", x => x.CourseId);
                });
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
										CompanyName = table.Column<string>(type: "VARCHAR(100)", nullable: false),
										Address = table.Column<string>(type: "VARCHAR(100)", nullable: false),
										PostalCode = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    CompanyTypeId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
										DeletedAt = table.Column<DateTime>(nullable: true)
								},
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_CompanyId", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_Company_CompanyType_CompanyTypeId",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyType",
                        principalColumn: "CompanyTypeId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
										FullName = table.Column<string>(type: "VARCHAR(100)", nullable: false),
										AdmissionId = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    CourseId = table.Column<int>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    MobileContact = table.Column<string>(type: "VARCHAR(10)", nullable: false),
										CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
										UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
										DeletedAt = table.Column<DateTime>(nullable: true),
								},
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_StudentId", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Student_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                name: "Company_CompanyName_UniqueConstraint",
                table: "Company",
                column: "CompanyName",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "CompanyType_TypeName_UniqueConstraint",
                table: "CompanyType",
                column: "TypeName",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "Course_CourseAbbreviation_UniqueConstraint",
                table: "Course",
                column: "CourseAbbreviation",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "Student_AdmissionId_UniqueConstraint",
                table: "Student",
                column: "AdmissionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Company");
            migrationBuilder.DropTable("Student");
            migrationBuilder.DropTable("CompanyType");
            migrationBuilder.DropTable("Course");
        }
    }
}
