using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectK.Database.Migrations
{
    /// <inheritdoc />
    public partial class DropAmountTypeColumnFromTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amount_type",
                table: "transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "amount_type",
                table: "transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
