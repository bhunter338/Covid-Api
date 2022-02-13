using Microsoft.EntityFrameworkCore.Migrations;

namespace Covid_Api.Migrations
{
    public partial class UpdateDateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UpdateDate",
                table: "dailyDatas",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "dailyDatas");
        }
    }
}
