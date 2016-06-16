using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace WEBACA.Migrations
{
    public partial class setupdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandName = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    NoOfProducts = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_BrandId", x => x.BrandId);
                });
            migrationBuilder.CreateTable(
                name: "Visibility",
                columns: table => new
                {
                    VisibilityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VisibilityName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_VisibilityId", x => x.VisibilityId);
                });
            migrationBuilder.CreateTable(
                name: "BrandPhoto",
                columns: table => new
                {
                    BrandPhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    Format = table.Column<string>(type: "VARCHAR(14)", nullable: false, defaultValue: ""),
                    Height = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ImageSize = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PublicCloudinaryId = table.Column<string>(type: "VARCHAR(100)", nullable: true, defaultValue: ""),
                    SecureUrl = table.Column<string>(type: "VARCHAR(300)", nullable: true, defaultValue: ""),
                    Url = table.Column<string>(type: "VARCHAR(300)", nullable: false, defaultValue: ""),
                    Version = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Width = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_BrandPhotoId", x => x.BrandPhotoId);
                    table.ForeignKey(
                        name: "FK_BrandPhoto_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CatName = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    VisibilityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_CatId", x => x.CatId);
                    table.ForeignKey(
                        name: "FK_Category_Visibility_VisibilityId",
                        column: x => x.VisibilityId,
                        principalTable: "Visibility",
                        principalColumn: "VisibilityId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "BrandCategory",
                columns: table => new
                {
                    BrandId = table.Column<int>(nullable: false),
                    CatId = table.Column<int>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("BrandsOfCategories_CompositeKey", x => new { x.BrandId, x.CatId });
                    table.ForeignKey(
                        name: "FK_BrandCategory_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandCategory_Category_CatId",
                        column: x => x.CatId,
                        principalTable: "Category",
                        principalColumn: "CatId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                name: "Brand_BrandName_UniqueConstraint",
                table: "Brands",
                column: "BrandName",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "Category_CatName_UniqueConstraint",
                table: "Category",
                column: "CatName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("BrandCategory");
            migrationBuilder.DropTable("BrandPhoto");
            migrationBuilder.DropTable("Category");
            migrationBuilder.DropTable("Brands");
            migrationBuilder.DropTable("Visibility");
        }
    }
}
