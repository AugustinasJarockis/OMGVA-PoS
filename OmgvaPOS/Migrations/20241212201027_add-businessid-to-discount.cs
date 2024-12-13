using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmgvaPOS.Migrations
{
    /// <inheritdoc />
    public partial class addbusinessidtodiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BusinessId",
                table: "Discounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Discounts");
        }
    }
}
