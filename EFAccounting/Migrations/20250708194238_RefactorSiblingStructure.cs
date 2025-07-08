using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFAccounting.Migrations
{
    /// <inheritdoc />
    public partial class RefactorSiblingStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wdays_Price_PriceId",
                table: "Wdays");

            migrationBuilder.DropTable(
                name: "KidSiblings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Price",
                table: "Price");

            migrationBuilder.RenameTable(
                name: "Price",
                newName: "Prices");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Departure",
                table: "Wdays",
                type: "time(6)",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time(0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Arrival",
                table: "Wdays",
                type: "time(6)",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time(0)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prices",
                table: "Prices",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SiblingRelationships",
                columns: table => new
                {
                    FromKidId = table.Column<int>(type: "int", nullable: false),
                    ToKidId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiblingRelationships", x => new { x.FromKidId, x.ToKidId });
                    table.ForeignKey(
                        name: "FK_SiblingRelationships_Kids_FromKidId",
                        column: x => x.FromKidId,
                        principalTable: "Kids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SiblingRelationships_Kids_ToKidId",
                        column: x => x.ToKidId,
                        principalTable: "Kids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SiblingRelationships_ToKidId",
                table: "SiblingRelationships",
                column: "ToKidId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wdays_Prices_PriceId",
                table: "Wdays",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wdays_Prices_PriceId",
                table: "Wdays");

            migrationBuilder.DropTable(
                name: "SiblingRelationships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prices",
                table: "Prices");

            migrationBuilder.RenameTable(
                name: "Prices",
                newName: "Price");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Departure",
                table: "Wdays",
                type: "time(0)",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Arrival",
                table: "Wdays",
                type: "time(0)",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time(6)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Price",
                table: "Price",
                column: "Id");

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
                name: "IX_KidSiblings_SiblingToId",
                table: "KidSiblings",
                column: "SiblingToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wdays_Price_PriceId",
                table: "Wdays",
                column: "PriceId",
                principalTable: "Price",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
