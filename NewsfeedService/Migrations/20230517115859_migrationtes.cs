using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsFeedService.Migrations
{
    /// <inheritdoc />
    public partial class migrationtes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "NewsFeeds");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "NewsFeeds",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NewsFeeds");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "NewsFeeds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
