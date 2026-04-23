using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedMessNameToMessId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messes_Users_OwnerId",
                table: "Messes");

            migrationBuilder.DropColumn(
                name: "MessName",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "MessId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_MessId",
                table: "Users",
                column: "MessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messes_Users_OwnerId",
                table: "Messes",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Messes_MessId",
                table: "Users",
                column: "MessId",
                principalTable: "Messes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messes_Users_OwnerId",
                table: "Messes");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Messes_MessId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_MessId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MessId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "MessName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messes_Users_OwnerId",
                table: "Messes",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
