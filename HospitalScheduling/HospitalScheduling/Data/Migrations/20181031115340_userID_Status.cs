using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalScheduling.Data.Migrations
{
    public partial class userID_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Nurse",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Doctor",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Nurse");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Doctor");
        }
    }
}
