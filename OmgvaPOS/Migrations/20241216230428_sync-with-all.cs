using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmgvaPOS.Migrations
{
    /// <inheritdoc />
    public partial class syncwithall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StripeAccId",
                table: "Businesses",
                newName: "StripeSecretKey");

            migrationBuilder.AddColumn<long>(
                name: "BusinessId",
                table: "Giftcards",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Giftcards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "BusinessId",
                table: "Giftcards");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Giftcards");

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
