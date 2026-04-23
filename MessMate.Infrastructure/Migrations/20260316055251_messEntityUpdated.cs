using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class messEntityUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messes_Categories_CategoryId",
                table: "Messes");

            migrationBuilder.DropIndex(
                name: "IX_Messes_CategoryId",
                table: "Messes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Messes");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Messes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AuthorisedName",
                table: "Messes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Messes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Messes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "Messes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MessName",
                table: "Messes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorisedName",
                table: "Messes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Messes");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Messes");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "Messes");

            migrationBuilder.DropColumn(
                name: "MessName",
                table: "Messes");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Messes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Messes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messes_CategoryId",
                table: "Messes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messes_Categories_CategoryId",
                table: "Messes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
