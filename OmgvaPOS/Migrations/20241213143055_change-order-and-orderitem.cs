using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmgvaPOS.Migrations
{
    /// <inheritdoc />
    public partial class changeorderandorderitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItemVariations_OrderItemId",
                table: "OrderItemVariations");

            migrationBuilder.AlterColumn<string>(
                name: "RefundReason",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemVariations_OrderItemId",
                table: "OrderItemVariations",
                column: "OrderItemId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItemVariations_OrderItemId",
                table: "OrderItemVariations");

            migrationBuilder.AlterColumn<string>(
                name: "RefundReason",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemVariations_OrderItemId",
                table: "OrderItemVariations",
                column: "OrderItemId");
        }
    }
}
