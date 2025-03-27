using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFAccounting.Migrations
{
    /// <inheritdoc />
    public partial class CreatePriceEntityInWDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriceId",
                table: "Wdays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Label = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Wdays_PriceId",
                table: "Wdays",
                column: "PriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wdays_Price_PriceId",
                table: "Wdays",
                column: "PriceId",
                principalTable: "Price",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wdays_Price_PriceId",
                table: "Wdays");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropIndex(
                name: "IX_Wdays_PriceId",
                table: "Wdays");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Wdays");
        }
    }
}
