using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class messEntityUpdated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessName",
                table: "Messes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessName",
                table: "Messes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
