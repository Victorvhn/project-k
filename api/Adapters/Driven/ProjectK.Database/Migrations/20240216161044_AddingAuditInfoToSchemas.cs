using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectK.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddingAuditInfoToSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_users_UserId",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_custom_planned_transactions_planned_transactions_BasePlanne~",
                table: "custom_planned_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_custom_planned_transactions_users_UserId",
                table: "custom_planned_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_planned_transactions_categories_CategoryId",
                table: "planned_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_planned_transactions_users_UserId",
                table: "planned_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_categories_CategoryId",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_planned_transactions_PlannedTransactionId",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_users_UserId",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactions",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_planned_transactions",
                table: "planned_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_custom_planned_transactions",
                table: "custom_planned_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categories",
                table: "categories");

            migrationBuilder.RenameIndex(
                name: "IX_users_email",
                table: "users",
                newName: "ix_users_email");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "transactions",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "PlannedTransactionId",
                table: "transactions",
                newName: "planned_transaction_id");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "transactions",
                newName: "category_id");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_UserId",
                table: "transactions",
                newName: "ix_transactions_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_PlannedTransactionId",
                table: "transactions",
                newName: "ix_transactions_planned_transaction_id");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_CategoryId",
                table: "transactions",
                newName: "ix_transactions_category_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "planned_transactions",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "planned_transactions",
                newName: "category_id");

            migrationBuilder.RenameIndex(
                name: "IX_planned_transactions_UserId",
                table: "planned_transactions",
                newName: "ix_planned_transactions_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_planned_transactions_CategoryId",
                table: "planned_transactions",
                newName: "ix_planned_transactions_category_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "custom_planned_transactions",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "BasePlannedTransactionId",
                table: "custom_planned_transactions",
                newName: "base_planned_transaction_id");

            migrationBuilder.RenameIndex(
                name: "IX_custom_planned_transactions_UserId",
                table: "custom_planned_transactions",
                newName: "ix_custom_planned_transactions_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_custom_planned_transactions_BasePlannedTransactionId",
                table: "custom_planned_transactions",
                newName: "ix_custom_planned_transactions_base_planned_transaction_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "categories",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_categories_UserId",
                table: "categories",
                newName: "ix_categories_user_id");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                table: "transactions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 16, 16, 10, 44, 402, DateTimeKind.Utc).AddTicks(1830));

            migrationBuilder.AddColumn<byte[]>(
                name: "created_by",
                table: "transactions",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at_utc",
                table: "transactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "updated_by",
                table: "transactions",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                table: "planned_transactions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 16, 16, 10, 44, 398, DateTimeKind.Utc).AddTicks(6600));

            migrationBuilder.AddColumn<byte[]>(
                name: "created_by",
                table: "planned_transactions",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at_utc",
                table: "planned_transactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "updated_by",
                table: "planned_transactions",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                table: "custom_planned_transactions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 16, 16, 10, 44, 399, DateTimeKind.Utc).AddTicks(8310));

            migrationBuilder.AddColumn<byte[]>(
                name: "created_by",
                table: "custom_planned_transactions",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at_utc",
                table: "custom_planned_transactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "updated_by",
                table: "custom_planned_transactions",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                table: "categories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 16, 16, 10, 44, 396, DateTimeKind.Utc).AddTicks(5160));

            migrationBuilder.AddColumn<byte[]>(
                name: "created_by",
                table: "categories",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at_utc",
                table: "categories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "updated_by",
                table: "categories",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transactions",
                table: "transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_planned_transactions",
                table: "planned_transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_custom_planned_transactions",
                table: "custom_planned_transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_categories",
                table: "categories",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_categories_users_user_id",
                table: "categories",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_custom_planned_transactions_planned_transactions_base_plann",
                table: "custom_planned_transactions",
                column: "base_planned_transaction_id",
                principalTable: "planned_transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_custom_planned_transactions_users_user_id",
                table: "custom_planned_transactions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_planned_transactions_categories_category_id",
                table: "planned_transactions",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_planned_transactions_users_user_id",
                table: "planned_transactions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_transactions_categories_category_id",
                table: "transactions",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_transactions_planned_transactions_planned_transaction_id",
                table: "transactions",
                column: "planned_transaction_id",
                principalTable: "planned_transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_transactions_users_user_id",
                table: "transactions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_categories_users_user_id",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "fk_custom_planned_transactions_planned_transactions_base_plann",
                table: "custom_planned_transactions");

            migrationBuilder.DropForeignKey(
                name: "fk_custom_planned_transactions_users_user_id",
                table: "custom_planned_transactions");

            migrationBuilder.DropForeignKey(
                name: "fk_planned_transactions_categories_category_id",
                table: "planned_transactions");

            migrationBuilder.DropForeignKey(
                name: "fk_planned_transactions_users_user_id",
                table: "planned_transactions");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions_categories_category_id",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions_planned_transactions_planned_transaction_id",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions_users_user_id",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transactions",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_planned_transactions",
                table: "planned_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_custom_planned_transactions",
                table: "custom_planned_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_categories",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "created_at_utc",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "created_at_utc",
                table: "planned_transactions");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "planned_transactions");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                table: "planned_transactions");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "planned_transactions");

            migrationBuilder.DropColumn(
                name: "created_at_utc",
                table: "custom_planned_transactions");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "custom_planned_transactions");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                table: "custom_planned_transactions");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "custom_planned_transactions");

            migrationBuilder.DropColumn(
                name: "created_at_utc",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "categories");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "users",
                newName: "IX_users_email");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "transactions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "planned_transaction_id",
                table: "transactions",
                newName: "PlannedTransactionId");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "transactions",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_user_id",
                table: "transactions",
                newName: "IX_transactions_UserId");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_planned_transaction_id",
                table: "transactions",
                newName: "IX_transactions_PlannedTransactionId");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_category_id",
                table: "transactions",
                newName: "IX_transactions_CategoryId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "planned_transactions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "planned_transactions",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "ix_planned_transactions_user_id",
                table: "planned_transactions",
                newName: "IX_planned_transactions_UserId");

            migrationBuilder.RenameIndex(
                name: "ix_planned_transactions_category_id",
                table: "planned_transactions",
                newName: "IX_planned_transactions_CategoryId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "custom_planned_transactions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "base_planned_transaction_id",
                table: "custom_planned_transactions",
                newName: "BasePlannedTransactionId");

            migrationBuilder.RenameIndex(
                name: "ix_custom_planned_transactions_user_id",
                table: "custom_planned_transactions",
                newName: "IX_custom_planned_transactions_UserId");

            migrationBuilder.RenameIndex(
                name: "ix_custom_planned_transactions_base_planned_transaction_id",
                table: "custom_planned_transactions",
                newName: "IX_custom_planned_transactions_BasePlannedTransactionId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "categories",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "ix_categories_user_id",
                table: "categories",
                newName: "IX_categories_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactions",
                table: "transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_planned_transactions",
                table: "planned_transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_custom_planned_transactions",
                table: "custom_planned_transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categories",
                table: "categories",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_users_UserId",
                table: "categories",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_custom_planned_transactions_planned_transactions_BasePlanne~",
                table: "custom_planned_transactions",
                column: "BasePlannedTransactionId",
                principalTable: "planned_transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_custom_planned_transactions_users_UserId",
                table: "custom_planned_transactions",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_planned_transactions_categories_CategoryId",
                table: "planned_transactions",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_planned_transactions_users_UserId",
                table: "planned_transactions",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_categories_CategoryId",
                table: "transactions",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_planned_transactions_PlannedTransactionId",
                table: "transactions",
                column: "PlannedTransactionId",
                principalTable: "planned_transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_users_UserId",
                table: "transactions",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
