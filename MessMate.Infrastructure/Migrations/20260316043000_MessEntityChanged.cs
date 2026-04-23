using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MessMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MessEntityChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "DeliveryAvailable",
                table: "Messes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeliveryAvailable",
                table: "Messes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "IsDeleted", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 3, 14, 5, 7, 54, 135, DateTimeKind.Utc).AddTicks(9950), null, null, false, "Home Mess", null, null },
                    { 2, null, new DateTime(2026, 3, 14, 5, 7, 54, 135, DateTimeKind.Utc).AddTicks(9954), null, null, false, "Restaurant Mess", null, null }
                });
        }
    }
}
