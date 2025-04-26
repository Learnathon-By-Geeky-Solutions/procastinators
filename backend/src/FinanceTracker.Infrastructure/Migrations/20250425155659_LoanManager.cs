using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LoanManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "PersonalTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "Loans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalTransactions_UserId1",
                table: "PersonalTransactions",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_WalletId",
                table: "Loans",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Wallets_WalletId",
                table: "Loans",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalTransactions_AspNetUsers_UserId1",
                table: "PersonalTransactions",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Wallets_WalletId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalTransactions_AspNetUsers_UserId1",
                table: "PersonalTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PersonalTransactions_UserId1",
                table: "PersonalTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Loans_WalletId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "PersonalTransactions");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Loans");
        }
    }
}
