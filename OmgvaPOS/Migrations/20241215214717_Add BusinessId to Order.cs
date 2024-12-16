using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmgvaPOS.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessIdtoOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItemVariations_OrderItemId",
                table: "OrderItemVariations");

            migrationBuilder.AddColumn<long>(
                name: "BusinessId",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemVariations_OrderItemId",
                table: "OrderItemVariations",
                column: "OrderItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItemVariations_OrderItemId",
                table: "OrderItemVariations");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemVariations_OrderItemId",
                table: "OrderItemVariations",
                column: "OrderItemId",
                unique: true);
        }
    }
}
