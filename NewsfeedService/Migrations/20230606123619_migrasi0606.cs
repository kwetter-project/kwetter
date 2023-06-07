using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsFeedService.Migrations
{
    /// <inheritdoc />
    public partial class migrasi0606 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NewsFeeds");

            migrationBuilder.DropColumn(
                name: "FolloweeId",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "FollowerId",
                table: "Followers");

            migrationBuilder.AddColumn<int>(
                name: "NewsFeedId",
                table: "Tweets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Tweets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "NewsFeeds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FolloweeName",
                table: "Followers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FollowerName",
                table: "Followers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tweets_NewsFeedId",
                table: "Tweets",
                column: "NewsFeedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tweets_NewsFeeds_NewsFeedId",
                table: "Tweets",
                column: "NewsFeedId",
                principalTable: "NewsFeeds",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tweets_NewsFeeds_NewsFeedId",
                table: "Tweets");

            migrationBuilder.DropIndex(
                name: "IX_Tweets_NewsFeedId",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "NewsFeedId",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "NewsFeeds");

            migrationBuilder.DropColumn(
                name: "FolloweeName",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "FollowerName",
                table: "Followers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tweets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "NewsFeeds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FolloweeId",
                table: "Followers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FollowerId",
                table: "Followers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
