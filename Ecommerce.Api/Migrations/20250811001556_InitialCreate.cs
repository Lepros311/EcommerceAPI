﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineItems_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Shoes" },
                    { 2, "Socks" },
                    { 3, "Pants" },
                    { 4, "Shirts" }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "TimeStamp", "Total" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 1, 12, 37, 22, 0, DateTimeKind.Unspecified), 22.99m },
                    { 2, new DateTime(2025, 8, 3, 14, 30, 48, 0, DateTimeKind.Unspecified), 61.20m },
                    { 3, new DateTime(2025, 8, 7, 11, 39, 10, 0, DateTimeKind.Unspecified), 156.20m },
                    { 4, new DateTime(2025, 8, 7, 19, 13, 55, 0, DateTimeKind.Unspecified), 107.50m },
                    { 5, new DateTime(2025, 8, 8, 9, 4, 17, 0, DateTimeKind.Unspecified), 95.80m }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Hightop Sneakers", 75.50m },
                    { 2, 1, "Boat Loafers", 53.75m },
                    { 3, 2, "Dress Socks", 15.25m },
                    { 4, 2, "Ankle Socks", 10.15m },
                    { 5, 3, "Dress Slacks", 35.99m },
                    { 6, 3, "Stonewash Jeans", 45.95m },
                    { 7, 4, "Flannel Shirt", 34.75m },
                    { 8, 4, "Shortsleeve Polo", 22.99m }
                });

            migrationBuilder.InsertData(
                table: "LineItems",
                columns: new[] { "Id", "ProductId", "Quantity", "SaleId", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 8, 1, 1, 22.99m },
                    { 2, 6, 1, 2, 45.95m },
                    { 3, 3, 1, 2, 15.25m },
                    { 4, 7, 1, 3, 34.75m },
                    { 5, 6, 1, 3, 45.95m },
                    { 6, 1, 1, 3, 75.50m },
                    { 7, 2, 2, 4, 53.75m },
                    { 8, 4, 2, 5, 10.15m },
                    { 9, 1, 1, 5, 75.50m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_ProductId",
                table: "LineItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_SaleId",
                table: "LineItems",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
