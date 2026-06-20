using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameBokingsToBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bokings_Sessions_SessionId",
                table: "Bokings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bokings_Users_MemberId",
                table: "Bokings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bokings",
                table: "Bokings");

            migrationBuilder.RenameTable(
                name: "Bokings",
                newName: "Bookings");

            migrationBuilder.RenameIndex(
                name: "IX_Bokings_SessionId",
                table: "Bookings",
                newName: "IX_Bookings_SessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Bokings_MemberId_SessionId",
                table: "Bookings",
                newName: "IX_Bookings_MemberId_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Sessions_SessionId",
                table: "Bookings",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_MemberId",
                table: "Bookings",
                column: "MemberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Sessions_SessionId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_MemberId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Bokings");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_SessionId",
                table: "Bokings",
                newName: "IX_Bokings_SessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_MemberId_SessionId",
                table: "Bokings",
                newName: "IX_Bokings_MemberId_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bokings",
                table: "Bokings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bokings_Sessions_SessionId",
                table: "Bokings",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bokings_Users_MemberId",
                table: "Bokings",
                column: "MemberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
