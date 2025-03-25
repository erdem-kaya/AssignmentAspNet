using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersProfile_JobTitles_JobTitleId",
                table: "UsersProfile");

            migrationBuilder.DropTable(
                name: "JobTitles");

            migrationBuilder.DropIndex(
                name: "IX_UsersProfile_JobTitleId",
                table: "UsersProfile");

            migrationBuilder.DropColumn(
                name: "JobTitleId",
                table: "UsersProfile");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "UsersProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "UsersProfile");

            migrationBuilder.AddColumn<int>(
                name: "JobTitleId",
                table: "UsersProfile",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "JobTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersProfile_JobTitleId",
                table: "UsersProfile",
                column: "JobTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersProfile_JobTitles_JobTitleId",
                table: "UsersProfile",
                column: "JobTitleId",
                principalTable: "JobTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
