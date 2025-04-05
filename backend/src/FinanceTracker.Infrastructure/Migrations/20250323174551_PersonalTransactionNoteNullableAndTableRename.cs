using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PersonalTransactionNoteNullableAndTableRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_UserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "PersonalTransactions");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_WalletId",
                table: "PersonalTransactions",
                newName: "IX_PersonalTransactions_WalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_UserId",
                table: "PersonalTransactions",
                newName: "IX_PersonalTransactions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_CategoryId",
                table: "PersonalTransactions",
                newName: "IX_PersonalTransactions_CategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "PersonalTransactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalTransactions",
                table: "PersonalTransactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalTransactions_AspNetUsers_UserId",
                table: "PersonalTransactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalTransactions_Categories_CategoryId",
                table: "PersonalTransactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalTransactions_Wallets_WalletId",
                table: "PersonalTransactions",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalTransactions_AspNetUsers_UserId",
                table: "PersonalTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalTransactions_Categories_CategoryId",
                table: "PersonalTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalTransactions_Wallets_WalletId",
                table: "PersonalTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalTransactions",
                table: "PersonalTransactions");

            migrationBuilder.RenameTable(
                name: "PersonalTransactions",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalTransactions_WalletId",
                table: "Transactions",
                newName: "IX_Transactions_WalletId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalTransactions_UserId",
                table: "Transactions",
                newName: "IX_Transactions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalTransactions_CategoryId",
                table: "Transactions",
                newName: "IX_Transactions_CategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_UserId",
                table: "Transactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
