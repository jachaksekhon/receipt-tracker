using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptTracker.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReceiptModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Receipts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Receipts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductSku",
                table: "ReceiptItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isManuallyAdded",
                table: "ReceiptItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_ReceiptId",
                table: "ReceiptItems",
                column: "ReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptItems_Receipts_ReceiptId",
                table: "ReceiptItems",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptItems_Receipts_ReceiptId",
                table: "ReceiptItems");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptItems_ReceiptId",
                table: "ReceiptItems");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "ProductSku",
                table: "ReceiptItems");

            migrationBuilder.DropColumn(
                name: "isManuallyAdded",
                table: "ReceiptItems");
        }
    }
}
