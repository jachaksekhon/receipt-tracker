using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptTracker.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReceiptItemSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ReceiptItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "ReceiptItems",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
