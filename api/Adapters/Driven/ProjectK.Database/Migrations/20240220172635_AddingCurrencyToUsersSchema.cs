using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectK.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddingCurrencyToUsersSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "currency",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at_utc",
                table: "transactions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 2, 16, 16, 10, 44, 402, DateTimeKind.Utc).AddTicks(1830));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at_utc",
                table: "planned_transactions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 2, 16, 16, 10, 44, 398, DateTimeKind.Utc).AddTicks(6600));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at_utc",
                table: "custom_planned_transactions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 2, 16, 16, 10, 44, 399, DateTimeKind.Utc).AddTicks(8310));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at_utc",
                table: "categories",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 2, 16, 16, 10, 44, 396, DateTimeKind.Utc).AddTicks(5160));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency",
                table: "users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at_utc",
                table: "transactions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 16, 16, 10, 44, 402, DateTimeKind.Utc).AddTicks(1830),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at_utc",
                table: "planned_transactions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 16, 16, 10, 44, 398, DateTimeKind.Utc).AddTicks(6600),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at_utc",
                table: "custom_planned_transactions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 16, 16, 10, 44, 399, DateTimeKind.Utc).AddTicks(8310),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at_utc",
                table: "categories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 16, 16, 10, 44, 396, DateTimeKind.Utc).AddTicks(5160),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
