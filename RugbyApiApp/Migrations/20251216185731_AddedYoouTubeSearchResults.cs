using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RugbyApiApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedYoouTubeSearchResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YouTubeVideoSearchResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    VideoId = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelTitle = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelId = table.Column<string>(type: "TEXT", nullable: false),
                    Duration = table.Column<string>(type: "TEXT", nullable: true),
                    Definition = table.Column<string>(type: "TEXT", nullable: true),
                    Dimension = table.Column<string>(type: "TEXT", nullable: true),
                    LicensedContent = table.Column<bool>(type: "INTEGER", nullable: false),
                    ViewCount = table.Column<long>(type: "INTEGER", nullable: false),
                    LikeCount = table.Column<long>(type: "INTEGER", nullable: false),
                    CommentCount = table.Column<long>(type: "INTEGER", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SearchedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsIgnored = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YouTubeVideoSearchResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YouTubeVideoSearchResults_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YouTubeVideoSearchResults_GameId",
                table: "YouTubeVideoSearchResults",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YouTubeVideoSearchResults");
        }
    }
}
