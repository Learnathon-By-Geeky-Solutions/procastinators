using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLoanAddBorrowerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Wallets_WalletId",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "Loans",
                newName: "LenderWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Loans_WalletId",
                table: "Loans",
                newName: "IX_Loans_LenderWalletId");

            migrationBuilder.AlterColumn<string>(
                name: "LenderId",
                table: "Loans",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "BorrowerId",
                table: "Loans",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BorrowerId",
                table: "Loans",
                column: "BorrowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_AspNetUsers_BorrowerId",
                table: "Loans",
                column: "BorrowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Wallets_LenderWalletId",
                table: "Loans",
                column: "LenderWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AspNetUsers_BorrowerId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Wallets_LenderWalletId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_BorrowerId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "BorrowerId",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "LenderWalletId",
                table: "Loans",
                newName: "WalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Loans_LenderWalletId",
                table: "Loans",
                newName: "IX_Loans_WalletId");

            migrationBuilder.AlterColumn<string>(
                name: "LenderId",
                table: "Loans",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Wallets_WalletId",
                table: "Loans",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
