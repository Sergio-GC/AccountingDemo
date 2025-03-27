using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFAccounting.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kids_Kids_KidId",
                table: "Kids");

            migrationBuilder.DropIndex(
                name: "IX_Kids_KidId",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "KidId",
                table: "Kids");

            migrationBuilder.AddColumn<int>(
                name: "KidId",
                table: "Wdays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "KidSiblings",
                columns: table => new
                {
                    SiblingFromId = table.Column<int>(type: "int", nullable: false),
                    SiblingToId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidSiblings", x => new { x.SiblingFromId, x.SiblingToId });
                    table.ForeignKey(
                        name: "FK_KidSiblings_Kids_SiblingFromId",
                        column: x => x.SiblingFromId,
                        principalTable: "Kids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidSiblings_Kids_SiblingToId",
                        column: x => x.SiblingToId,
                        principalTable: "Kids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Wdays_KidId",
                table: "Wdays",
                column: "KidId");

            migrationBuilder.CreateIndex(
                name: "IX_KidSiblings_SiblingToId",
                table: "KidSiblings",
                column: "SiblingToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wdays_Kids_KidId",
                table: "Wdays",
                column: "KidId",
                principalTable: "Kids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wdays_Kids_KidId",
                table: "Wdays");

            migrationBuilder.DropTable(
                name: "KidSiblings");

            migrationBuilder.DropIndex(
                name: "IX_Wdays_KidId",
                table: "Wdays");

            migrationBuilder.DropColumn(
                name: "KidId",
                table: "Wdays");

            migrationBuilder.AddColumn<int>(
                name: "KidId",
                table: "Kids",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kids_KidId",
                table: "Kids",
                column: "KidId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kids_Kids_KidId",
                table: "Kids",
                column: "KidId",
                principalTable: "Kids",
                principalColumn: "Id");
        }
    }
}
