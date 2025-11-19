using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptTracker.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReceiptSchemav2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receipt",
                table: "ReceiptItems");

            migrationBuilder.RenameColumn(
                name: "isManuallyAdded",
                table: "ReceiptItems",
                newName: "IsManuallyAdded");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ReceiptItems",
                newName: "OriginalPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "ReceiptItems",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalPrice",
                table: "ReceiptItems",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "ReceiptItems");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "ReceiptItems");

            migrationBuilder.RenameColumn(
                name: "IsManuallyAdded",
                table: "ReceiptItems",
                newName: "isManuallyAdded");

            migrationBuilder.RenameColumn(
                name: "OriginalPrice",
                table: "ReceiptItems",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "Receipt",
                table: "ReceiptItems",
                type: "text",
                nullable: true);
        }
    }
}
