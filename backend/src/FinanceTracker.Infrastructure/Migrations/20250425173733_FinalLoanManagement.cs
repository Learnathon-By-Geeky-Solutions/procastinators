using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalLoanManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BorrowerWalletId",
                table: "Loans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BorrowerWalletId",
                table: "Loans",
                column: "BorrowerWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Wallets_BorrowerWalletId",
                table: "Loans",
                column: "BorrowerWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Wallets_BorrowerWalletId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_BorrowerWalletId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "BorrowerWalletId",
                table: "Loans");
        }
    }
}
