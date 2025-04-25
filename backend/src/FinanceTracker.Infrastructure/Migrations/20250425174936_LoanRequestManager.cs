using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LoanRequestManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "LoanRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanRequests_WalletId",
                table: "LoanRequests",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanRequests_Wallets_WalletId",
                table: "LoanRequests",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanRequests_Wallets_WalletId",
                table: "LoanRequests");

            migrationBuilder.DropIndex(
                name: "IX_LoanRequests_WalletId",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "LoanRequests");
        }
    }
}
