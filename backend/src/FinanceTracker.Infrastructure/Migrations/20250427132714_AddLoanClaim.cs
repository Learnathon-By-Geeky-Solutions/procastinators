using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLoanClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanRequests_Wallets_WalletId",
                table: "LoanRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Wallets_BorrowerWalletId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Wallets_LenderWalletId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_BorrowerWalletId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LenderWalletId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_LoanRequests_WalletId",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "BorrowerWalletId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "LenderWalletId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "LoanRequests");

            migrationBuilder.CreateTable(
                name: "LoanClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(type: "int", nullable: false),
                    IsClaimed = table.Column<bool>(type: "bit", nullable: false),
                    ClaimedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanClaims_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanClaims_LoanId",
                table: "LoanClaims",
                column: "LoanId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanClaims");

            migrationBuilder.AddColumn<int>(
                name: "BorrowerWalletId",
                table: "Loans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LenderWalletId",
                table: "Loans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "LoanRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BorrowerWalletId",
                table: "Loans",
                column: "BorrowerWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LenderWalletId",
                table: "Loans",
                column: "LenderWalletId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Wallets_BorrowerWalletId",
                table: "Loans",
                column: "BorrowerWalletId",
                principalTable: "Wallets",
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
    }
}
