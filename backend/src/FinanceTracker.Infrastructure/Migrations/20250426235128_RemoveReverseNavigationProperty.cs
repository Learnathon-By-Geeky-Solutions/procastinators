using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveReverseNavigationProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanRequests_AspNetUsers_UserId",
                table: "LoanRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AspNetUsers_UserId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalTransactions_AspNetUsers_UserId1",
                table: "PersonalTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PersonalTransactions_UserId1",
                table: "PersonalTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Loans_UserId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_LoanRequests_UserId",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "PersonalTransactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LoanRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "PersonalTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Loans",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LoanRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalTransactions_UserId1",
                table: "PersonalTransactions",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserId",
                table: "Loans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanRequests_UserId",
                table: "LoanRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanRequests_AspNetUsers_UserId",
                table: "LoanRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_AspNetUsers_UserId",
                table: "Loans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalTransactions_AspNetUsers_UserId1",
                table: "PersonalTransactions",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
