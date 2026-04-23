using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderEntityModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 🔥 Step 1: Add temporary column
            migrationBuilder.AddColumn<int>(
                name: "MealSlotTemp",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            // 🔥 Step 2: Convert string → int
            migrationBuilder.Sql(@"
        UPDATE Orders
        SET MealSlotTemp =
            CASE MealSlot
                WHEN 'Breakfast' THEN 1
                WHEN 'Lunch' THEN 2
                WHEN 'Dinner' THEN 3
                ELSE 0
            END
    ");

            // 🔥 Step 3: Drop old column
            migrationBuilder.DropColumn(
                name: "MealSlot",
                table: "Orders");

            // 🔥 Step 4: Rename temp column
            migrationBuilder.RenameColumn(
                name: "MealSlotTemp",
                table: "Orders",
                newName: "MealSlot");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MealSlot",
                table: "Orders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);
        }
    }
}
