using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RugbyApiApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "Leagues",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Games_LeagueId",
                table: "Games",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Leagues_LeagueId",
                table: "Games",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Leagues_LeagueId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_LeagueId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "Leagues");
        }
    }
}
