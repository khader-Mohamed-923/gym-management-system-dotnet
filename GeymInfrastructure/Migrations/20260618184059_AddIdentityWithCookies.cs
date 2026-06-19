using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityWithCookies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bokings_User_MemberId",
                table: "Bokings");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecord_User_MemberId",
                table: "HealthRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberShips_User_MemberId",
                table: "MemberShips");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_User_TrainerId",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthRecord",
                table: "HealthRecord");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "HealthRecord");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "HealthRecord",
                newName: "HealthRecords");



            migrationBuilder.RenameColumn(
                name: "Hight",
                table: "HealthRecords",
                newName: "Height");

            migrationBuilder.RenameIndex(
                name: "IX_HealthRecord_MemberId",
                table: "HealthRecords",
                newName: "IX_HealthRecords_MemberId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MedicalConditions",
                table: "HealthRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthRecords",
                table: "HealthRecords",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ApplicationUserId",
                table: "Users",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bokings_Users_MemberId",
                table: "Bokings",
                column: "MemberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Users_MemberId",
                table: "HealthRecords",
                column: "MemberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShips_Users_MemberId",
                table: "MemberShips",
                column: "MemberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Users_TrainerId",
                table: "Sessions",
                column: "TrainerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AspNetUsers_ApplicationUserId",
                table: "Users",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bokings_Users_MemberId",
                table: "Bokings");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Users_MemberId",
                table: "HealthRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberShips_Users_MemberId",
                table: "MemberShips");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Users_TrainerId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AspNetUsers_ApplicationUserId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ApplicationUserId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthRecords",
                table: "HealthRecords");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MedicalConditions",
                table: "HealthRecords");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "HealthRecords",
                newName: "HealthRecord");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Phone",
                table: "User",
                newName: "IX_User_Phone");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "User",
                newName: "IX_User_Email");

            migrationBuilder.RenameColumn(
                name: "Height",
                table: "HealthRecord",
                newName: "Hight");

            migrationBuilder.RenameIndex(
                name: "IX_HealthRecords_MemberId",
                table: "HealthRecord",
                newName: "IX_HealthRecord_MemberId");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "HealthRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthRecord",
                table: "HealthRecord",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bokings_User_MemberId",
                table: "Bokings",
                column: "MemberId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecord_User_MemberId",
                table: "HealthRecord",
                column: "MemberId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShips_User_MemberId",
                table: "MemberShips",
                column: "MemberId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_User_TrainerId",
                table: "Sessions",
                column: "TrainerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
