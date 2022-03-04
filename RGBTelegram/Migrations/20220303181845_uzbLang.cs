using Microsoft.EntityFrameworkCore.Migrations;

namespace RGBTelegram.Migrations
{
    public partial class uzbLang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "language",
                table: "UZSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "language",
                table: "UZSessions");

           
        }
    }
}
