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

            migrationBuilder.AlterColumn<string>(
                name: "region_id",
                table: "UZRegistrations",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "language",
                table: "UZSessions");

            migrationBuilder.AlterColumn<int>(
                name: "region_id",
                table: "UZRegistrations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
