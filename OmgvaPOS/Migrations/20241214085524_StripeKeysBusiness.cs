﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmgvaPOS.Migrations
{
    /// <inheritdoc />
    public partial class StripeKeysBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StripeAccId",
                table: "Businesses",
                newName: "StripeSecretKey");

            migrationBuilder.AddColumn<string>(
                name: "StripePublishKey",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripePublishKey",
                table: "Businesses");

            migrationBuilder.RenameColumn(
                name: "StripeSecretKey",
                table: "Businesses",
                newName: "StripeAccId");
        }
    }
}
