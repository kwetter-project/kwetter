using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TweetService.Migrations
{
    /// <inheritdoc />
    public partial class fixmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User",
                table: "Tweets",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Tweets",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Tweets",
                newName: "Content");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Tweets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TweetId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "Reply",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "Retweet",
                table: "Tweets");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Tweets",
                newName: "User");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Tweets",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Tweets",
                newName: "DateTime");
        }
    }
}
