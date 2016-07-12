using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WEBA_ASSIGNMENT.Migrations
{
    public partial class setupdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PresetMetric",
                columns: table => new
                {
                    PMetricId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MetricCharacter = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    MetricSubType = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    MetricType = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_PMetricId", x => x.PMetricId);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_Status_StatusId", x => x.StatusId);
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
                    table.PrimaryKey("PrimaryKey_Visibility_VisibilityId", x => x.VisibilityId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandName = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedById = table.Column<string>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    NoOfProducts = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedById = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_BrandId", x => x.BrandId);
                    table.ForeignKey(
                        name: "FK_Brands_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Brands_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Brands_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Specials",
                columns: table => new
                {
                    SpecialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    CreatedById = table.Column<string>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    SpecialName = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedById = table.Column<string>(nullable: false),
                    numericalDiscount = table.Column<double>(nullable: false),
                    percentageDiscount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_SpecialId", x => x.SpecialId);
                    table.ForeignKey(
                        name: "FK_Specials_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Specials_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Specials_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CatName = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    CreatedById = table.Column<string>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedById = table.Column<string>(nullable: false),
                    VisibilityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_Category_CatId", x => x.CatId);
                    table.ForeignKey(
                        name: "FK_Category_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Category_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Category_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Category_Visibility_VisibilityId",
                        column: x => x.VisibilityId,
                        principalTable: "Visibility",
                        principalColumn: "VisibilityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandPhoto",
                columns: table => new
                {
                    BrandPhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    CreatedById = table.Column<string>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
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
                    table.ForeignKey(
                        name: "FK_BrandPhoto_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrandPhoto_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProdId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedById = table.Column<string>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    Description = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
                    ProdName = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Published = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ThresholdInventoryQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedById = table.Column<string>(nullable: false),
                    isConsumable = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_Product_ProdId", x => x.ProdId);
                    table.ForeignKey(
                        name: "FK_Product_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BrandSpecials",
                columns: table => new
                {
                    BrandId = table.Column<int>(nullable: false),
                    SpecialId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("BrandSpecials_CompositeKey", x => new { x.BrandId, x.SpecialId });
                    table.ForeignKey(
                        name: "FK_BrandSpecials_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandSpecials_Specials_SpecialId",
                        column: x => x.SpecialId,
                        principalTable: "Specials",
                        principalColumn: "SpecialId",
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

            migrationBuilder.CreateTable(
                name: "CategorySpecials",
                columns: table => new
                {
                    CatId = table.Column<int>(nullable: false),
                    SpecialId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CategorySpecials_CompositeKey", x => new { x.CatId, x.SpecialId });
                    table.ForeignKey(
                        name: "FK_CategorySpecials_Category_CatId",
                        column: x => x.CatId,
                        principalTable: "Category",
                        principalColumn: "CatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategorySpecials_Specials_SpecialId",
                        column: x => x.SpecialId,
                        principalTable: "Specials",
                        principalColumn: "SpecialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Consumable",
                columns: table => new
                {
                    ConsumableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActiveIngredients = table.Column<string>(type: "VARCHAR(1000)", nullable: true),
                    GuranteedAnalysis = table.Column<string>(type: "VARCHAR(1000)", nullable: true),
                    InActiveIngredients = table.Column<string>(type: "VARCHAR(1000)", nullable: true),
                    VARCHAR1000 = table.Column<string>(name: "VARCHAR(1000)", nullable: true),
                    ProdId = table.Column<int>(nullable: false),
                    TypicalAnalysis = table.Column<string>(type: "VARCHAR(1000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_ConsumableId", x => x.ConsumableId);
                    table.ForeignKey(
                        name: "FK_Consumable_Product_ProdId",
                        column: x => x.ProdId,
                        principalTable: "Product",
                        principalColumn: "ProdId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    MetricId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedById = table.Column<string>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    MetricAmount = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    MetricName = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    PMetricId = table.Column<int>(type: "int", nullable: true),
                    PriceId = table.Column<int>(type: "int", nullable: false),
                    ProdId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedById = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_MetricId", x => x.MetricId);
                    table.ForeignKey(
                        name: "FK_Metrics_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Metrics_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Metrics_PresetMetric_PMetricId",
                        column: x => x.PMetricId,
                        principalTable: "PresetMetric",
                        principalColumn: "PMetricId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Metrics_Product_ProdId",
                        column: x => x.ProdId,
                        principalTable: "Product",
                        principalColumn: "ProdId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Metrics_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Metrics_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductPhoto",
                columns: table => new
                {
                    ProductPhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    CreatedById = table.Column<string>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    Format = table.Column<string>(type: "VARCHAR(14)", nullable: false, defaultValue: ""),
                    Height = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ImageSize = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ProdId = table.Column<int>(nullable: false),
                    PublicCloudinaryId = table.Column<string>(type: "VARCHAR(100)", nullable: true, defaultValue: ""),
                    SecureUrl = table.Column<string>(type: "VARCHAR(300)", nullable: true, defaultValue: ""),
                    Url = table.Column<string>(type: "VARCHAR(300)", nullable: false, defaultValue: ""),
                    Version = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Width = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    isPrimaryPhoto = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_ProductPhoto_ProductPhotoId", x => x.ProductPhotoId);
                    table.ForeignKey(
                        name: "FK_ProductPhoto_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPhoto_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPhoto_Product_ProdId",
                        column: x => x.ProdId,
                        principalTable: "Product",
                        principalColumn: "ProdId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecials",
                columns: table => new
                {
                    ProdId = table.Column<int>(nullable: false),
                    SpecialId = table.Column<int>(nullable: false),
                    ProductsProdId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ProductSpecials_CompositeKey", x => new { x.ProdId, x.SpecialId });
                    table.ForeignKey(
                        name: "FK_ProductSpecials_Product_ProductsProdId",
                        column: x => x.ProductsProdId,
                        principalTable: "Product",
                        principalColumn: "ProdId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSpecials_Specials_SpecialId",
                        column: x => x.SpecialId,
                        principalTable: "Specials",
                        principalColumn: "SpecialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    PriceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedById = table.Column<string>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    MetricId = table.Column<int>(type: "int", nullable: false),
                    RRP = table.Column<decimal>(type: "money", nullable: true),
                    Value = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_PriceId", x => x.PriceId);
                    table.ForeignKey(
                        name: "FK_Price_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Price_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Price_Metrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Metrics",
                        principalColumn: "MetricId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrandCategory_BrandId",
                table: "BrandCategory",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandCategory_CatId",
                table: "BrandCategory",
                column: "CatId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandPhoto_BrandId",
                table: "BrandPhoto",
                column: "BrandId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrandPhoto_CreatedById",
                table: "BrandPhoto",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BrandPhoto_DeletedById",
                table: "BrandPhoto",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "Brand_BrandName_UniqueConstraint",
                table: "Brands",
                column: "BrandName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_CreatedById",
                table: "Brands",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_DeletedById",
                table: "Brands",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_UpdatedById",
                table: "Brands",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BrandSpecials_BrandId",
                table: "BrandSpecials",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandSpecials_SpecialId",
                table: "BrandSpecials",
                column: "SpecialId");

            migrationBuilder.CreateIndex(
                name: "Category_CatName_UniqueConstraint",
                table: "Category",
                column: "CatName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_CreatedById",
                table: "Category",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Category_DeletedById",
                table: "Category",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Category_UpdatedById",
                table: "Category",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Category_VisibilityId",
                table: "Category",
                column: "VisibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySpecials_CatId",
                table: "CategorySpecials",
                column: "CatId");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySpecials_SpecialId",
                table: "CategorySpecials",
                column: "SpecialId");

            migrationBuilder.CreateIndex(
                name: "IX_Consumable_ProdId",
                table: "Consumable",
                column: "ProdId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_CreatedById",
                table: "Metrics",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_DeletedById",
                table: "Metrics",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_PMetricId",
                table: "Metrics",
                column: "PMetricId");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_ProdId",
                table: "Metrics",
                column: "ProdId");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_StatusId",
                table: "Metrics",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_UpdatedById",
                table: "Metrics",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Price_CreatedById",
                table: "Price",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Price_DeletedById",
                table: "Price",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Price_MetricId",
                table: "Price",
                column: "MetricId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_BrandId",
                table: "Product",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CreatedById",
                table: "Product",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Product_DeletedById",
                table: "Product",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "Product_ProdName_UniqueConstraint",
                table: "Product",
                column: "ProdName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_UpdatedById",
                table: "Product",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhoto_CreatedById",
                table: "ProductPhoto",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhoto_DeletedById",
                table: "ProductPhoto",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhoto_ProdId",
                table: "ProductPhoto",
                column: "ProdId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecials_ProductsProdId",
                table: "ProductSpecials",
                column: "ProductsProdId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecials_SpecialId",
                table: "ProductSpecials",
                column: "SpecialId");

            migrationBuilder.CreateIndex(
                name: "IX_Specials_CreatedById",
                table: "Specials",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Specials_DeletedById",
                table: "Specials",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Specials_UpdatedById",
                table: "Specials",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BrandCategory");

            migrationBuilder.DropTable(
                name: "BrandPhoto");

            migrationBuilder.DropTable(
                name: "BrandSpecials");

            migrationBuilder.DropTable(
                name: "CategorySpecials");

            migrationBuilder.DropTable(
                name: "Consumable");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "ProductPhoto");

            migrationBuilder.DropTable(
                name: "ProductSpecials");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Metrics");

            migrationBuilder.DropTable(
                name: "Specials");

            migrationBuilder.DropTable(
                name: "Visibility");

            migrationBuilder.DropTable(
                name: "PresetMetric");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
