using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmgvaPOS.Migrations
{
    /// <inheritdoc />
    public partial class addamountpayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Amount",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Giftcards");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Giftcards");
        }
    }
}
