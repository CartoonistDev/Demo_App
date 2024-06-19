using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Product.API.Migrations
{
    /// <inheritdoc />
    public partial class Test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImageFile = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OrderProduct",
                columns: new[] { "Id", "Category", "Description", "ImageFile", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("018fc847-db2d-189e-952c-519f5a95b7d9"), "[\"Camera\"]", "This phone is the company's biggest change to it's first line products", "product-7.png", "Panasonic Lumix", 450.00m },
                    { new Guid("018fc847-db2d-489e-952c-519f5a95b7d3"), "[\"Smart Phone\"]", "This phone is the company's biggest change to it's first line products", "product-1.png", "IPhone XR", 1020.00m },
                    { new Guid("018fc847-db2d-489e-952c-519f8a95b7d8"), "[\"Home Kitchen\"]", "This phone is the company's biggest change to it's first line products", "product-6.png", "LG G7 ThinQ", 450.00m },
                    { new Guid("018fc847-db2d-489e-952d-519f5a95b7d6"), "[\"Smart Phone\"]", "This phone is the company's biggest change to it's first line products", "product-4.png", "Infinix Zero 3", 450.00m },
                    { new Guid("018fc847-db2d-489e-992c-519f5a95b7d7"), "[\"Smart Phone\"]", "This phone is the company's biggest change to it's first line products", "product-5.png", "Huwai Plus", 750.00m },
                    { new Guid("018fc847-db4d-489e-952c-519f5a95b7d4"), "[\"Smart Phone\"]", "This phone is the company's biggest change to it's first line products", "product-2.png", "Samsung Note 10", 950.00m },
                    { new Guid("018fc847-db6d-489e-952c-519f5a95b7d5"), "[\"Smart Phone\"]", "This phone is the company's biggest change to it's first line products", "product-3.png", "Tecno Pop 5", 450.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProduct");
        }
    }
}
