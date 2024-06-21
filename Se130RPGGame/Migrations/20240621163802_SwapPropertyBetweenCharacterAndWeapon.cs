using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Se130RPGGame.Migrations
{
    public partial class SwapPropertyBetweenCharacterAndWeapon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_weapons_characters_CharacterId",
                table: "weapons");

            migrationBuilder.DropIndex(
                name: "IX_weapons_CharacterId",
                table: "weapons");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "weapons");

            migrationBuilder.AddColumn<int>(
                name: "WeaponId",
                table: "characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_characters_WeaponId",
                table: "characters",
                column: "WeaponId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_characters_weapons_WeaponId",
                table: "characters",
                column: "WeaponId",
                principalTable: "weapons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_characters_weapons_WeaponId",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "IX_characters_WeaponId",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "WeaponId",
                table: "characters");

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "weapons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_weapons_CharacterId",
                table: "weapons",
                column: "CharacterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_weapons_characters_CharacterId",
                table: "weapons",
                column: "CharacterId",
                principalTable: "characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
