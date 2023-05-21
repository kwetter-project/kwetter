using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsFeedService.Migrations
{
    /// <inheritdoc />
    public partial class migration1705 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsFeeds_Tweets_TweetID",
                table: "NewsFeeds");

            migrationBuilder.DropIndex(
                name: "IX_NewsFeeds_TweetID",
                table: "NewsFeeds");

            migrationBuilder.DropColumn(
                name: "TweetID",
                table: "NewsFeeds");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Tweets",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "ExternalID",
                table: "Tweets",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Tweets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Tweets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Like",
                table: "Tweets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Reply",
                table: "Tweets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Retweet",
                table: "Tweets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "NewsFeeds",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "NewsFeeds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Followers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FollowerId = table.Column<int>(type: "int", nullable: false),
                    FolloweeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Followers");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "Like",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "Reply",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "Retweet",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "NewsFeeds");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Tweets",
                newName: "ExternalID");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Tweets",
                newName: "Message");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedAt",
                table: "NewsFeeds",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "TweetID",
                table: "NewsFeeds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NewsFeeds_TweetID",
                table: "NewsFeeds",
                column: "TweetID");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsFeeds_Tweets_TweetID",
                table: "NewsFeeds",
                column: "TweetID",
                principalTable: "Tweets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
