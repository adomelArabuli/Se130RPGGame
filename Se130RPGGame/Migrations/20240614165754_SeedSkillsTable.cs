using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Se130RPGGame.Migrations
{
    public partial class SeedSkillsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "skills",
                columns: new[] { "Id", "Damage", "Name" },
                values: new object[] { 1, 30, "FireBall" });

            migrationBuilder.InsertData(
                table: "skills",
                columns: new[] { "Id", "Damage", "Name" },
                values: new object[] { 2, 20, "Frenzy" });

            migrationBuilder.InsertData(
                table: "skills",
                columns: new[] { "Id", "Damage", "Name" },
                values: new object[] { 3, 50, "Blizzard" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "skills",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "skills",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "skills",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
