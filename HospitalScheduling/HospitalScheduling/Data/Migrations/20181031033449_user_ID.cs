using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalScheduling.Data.Migrations
{
    public partial class user_ID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Nurse",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Doctor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Nurse");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Doctor");
        }
    }
}
