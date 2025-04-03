﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFAccounting.Migrations
{
    /// <inheritdoc />
    public partial class SetSoftDeleteToPriceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Price",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Price");
        }
    }
}
