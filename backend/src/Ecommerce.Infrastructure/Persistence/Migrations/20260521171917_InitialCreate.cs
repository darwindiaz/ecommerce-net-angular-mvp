using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Color = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.CheckConstraint("CK_Products_Price_Positive", "\"Price\" > 0");
                    table.CheckConstraint("CK_Products_Stock_NonNegative", "\"Stock\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Names = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    LastNames = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Department = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 180, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 180, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.CheckConstraint("CK_Orders_Total_NonNegative", "\"Total\" >= 0");
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CartId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.CheckConstraint("CK_CartItems_Quantity_Positive", "\"Quantity\" > 0");
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.CheckConstraint("CK_OrderItems_Price_Positive", "\"Price\" > 0");
                    table.CheckConstraint("CK_OrderItems_Quantity_Positive", "\"Quantity\" > 0");
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Code", "Color", "Description", "ImageUrl", "Name", "Price", "Size", "Stock" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "SNK-WHT-07-001", "White", "Tenis urbanos blancos para uso diario.", "https://placehold.co/600x400?text=White+Sneaker+7", "Urban Runner White 7", 179900m, 7, 15 },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "SNK-BLK-07-002", "Black", "Tenis negros flexibles con suela liviana.", "https://placehold.co/600x400?text=Black+Sneaker+7", "Street Flex Black 7", 189900m, 7, 12 },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "SNK-GRY-08-003", "Gray", "Tenis grises comodos para caminatas largas.", "https://placehold.co/600x400?text=Gray+Sneaker+8", "Daily Move Gray 8", 169900m, 8, 20 },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "SNK-WHT-08-004", "White", "Tenis blancos minimalistas con acabado suave.", "https://placehold.co/600x400?text=White+Sneaker+8", "Clean Step White 8", 199900m, 8, 10 },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "SNK-BLK-08-005", "Black", "Tenis negros casuales resistentes al uso frecuente.", "https://placehold.co/600x400?text=Black+Sneaker+8", "Night Walk Black 8", 209900m, 8, 8 },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "SNK-GRY-09-006", "Gray", "Tenis grises de estilo deportivo urbano.", "https://placehold.co/600x400?text=Gray+Sneaker+9", "Metro Pace Gray 9", 219900m, 9, 18 },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "SNK-WHT-09-007", "White", "Tenis blancos ligeros con plantilla acolchada.", "https://placehold.co/600x400?text=White+Sneaker+9", "Air Street White 9", 229900m, 9, 14 },
                    { new Guid("10000000-0000-0000-0000-000000000008"), "SNK-BLK-10-008", "Black", "Tenis negros clasicos con diseño versatil.", "https://placehold.co/600x400?text=Black+Sneaker+10", "Core Black 10", 239900m, 10, 11 },
                    { new Guid("10000000-0000-0000-0000-000000000009"), "SNK-GRY-10-009", "Gray", "Tenis grises con diseño sobrio para uso diario.", "https://placehold.co/600x400?text=Gray+Sneaker+10", "Balance Gray 10", 189900m, 10, 16 },
                    { new Guid("10000000-0000-0000-0000-000000000010"), "SNK-WHT-10-010", "White", "Tenis blancos clasicos para combinaciones casuales.", "https://placehold.co/600x400?text=White+Sneaker+10", "Classic White 10", 159900m, 10, 22 },
                    { new Guid("10000000-0000-0000-0000-000000000011"), "SNK-GRY-07-011", "Gray", "Tenis grises livianos para jornadas activas.", "https://placehold.co/600x400?text=Gray+Sneaker+7", "Soft Track Gray 7", 174900m, 7, 13 },
                    { new Guid("10000000-0000-0000-0000-000000000012"), "SNK-WHT-07-012", "White", "Tenis blancos con perfil bajo y suela flexible.", "https://placehold.co/600x400?text=White+Sneaker+7+Alt", "Fresh Walk White 7", 184900m, 7, 9 },
                    { new Guid("10000000-0000-0000-0000-000000000013"), "SNK-BLK-08-013", "Black", "Tenis negros de ajuste comodo para uso urbano.", "https://placehold.co/600x400?text=Black+Sneaker+8+Alt", "Shadow Fit Black 8", 214900m, 8, 17 },
                    { new Guid("10000000-0000-0000-0000-000000000014"), "SNK-GRY-08-014", "Gray", "Tenis grises con amortiguacion para caminar.", "https://placehold.co/600x400?text=Gray+Sneaker+8+Alt", "Cloud Step Gray 8", 204900m, 8, 19 },
                    { new Guid("10000000-0000-0000-0000-000000000015"), "SNK-WHT-09-015", "White", "Tenis blancos minimalistas para combinaciones limpias.", "https://placehold.co/600x400?text=White+Sneaker+9+Alt", "Minimal White 9", 194900m, 9, 21 },
                    { new Guid("10000000-0000-0000-0000-000000000016"), "SNK-BLK-09-016", "Black", "Tenis negros con acabado mate y suela resistente.", "https://placehold.co/600x400?text=Black+Sneaker+9", "Urban Core Black 9", 224900m, 9, 12 },
                    { new Guid("10000000-0000-0000-0000-000000000017"), "SNK-GRY-09-017", "Gray", "Tenis grises versatiles para salidas casuales.", "https://placehold.co/600x400?text=Gray+Sneaker+9+Alt", "Route Gray 9", 199900m, 9, 15 },
                    { new Guid("10000000-0000-0000-0000-000000000018"), "SNK-WHT-10-018", "White", "Tenis blancos con plantilla suave y estilo casual.", "https://placehold.co/600x400?text=White+Sneaker+10+Alt", "Pure Motion White 10", 234900m, 10, 10 },
                    { new Guid("10000000-0000-0000-0000-000000000019"), "SNK-BLK-10-019", "Black", "Tenis negros de corte clasico para uso diario.", "https://placehold.co/600x400?text=Black+Sneaker+10+Alt", "Bold Step Black 10", 244900m, 10, 7 },
                    { new Guid("10000000-0000-0000-0000-000000000020"), "SNK-GRY-10-020", "Gray", "Tenis grises con estructura estable y suela liviana.", "https://placehold.co/600x400?text=Gray+Sneaker+10+Alt", "City Flow Gray 10", 229900m, 10, 18 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId_ProductId",
                table: "CartItems",
                columns: new[] { "CartId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code",
                table: "Products",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
