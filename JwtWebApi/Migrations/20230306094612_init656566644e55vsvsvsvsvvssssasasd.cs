using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwtWebApi.Migrations
{
    public partial class init656566644e55vsvsvsvsvvssssasasd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hero_Users_UserId",
                table: "Hero");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hero",
                table: "Hero");

            migrationBuilder.RenameTable(
                name: "Hero",
                newName: "AllHeroes");

            migrationBuilder.RenameIndex(
                name: "IX_Hero_UserId",
                table: "AllHeroes",
                newName: "IX_AllHeroes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllHeroes",
                table: "AllHeroes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AllHeroes_Users_UserId",
                table: "AllHeroes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllHeroes_Users_UserId",
                table: "AllHeroes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllHeroes",
                table: "AllHeroes");

            migrationBuilder.RenameTable(
                name: "AllHeroes",
                newName: "Hero");

            migrationBuilder.RenameIndex(
                name: "IX_AllHeroes_UserId",
                table: "Hero",
                newName: "IX_Hero_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hero",
                table: "Hero",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hero_Users_UserId",
                table: "Hero",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
