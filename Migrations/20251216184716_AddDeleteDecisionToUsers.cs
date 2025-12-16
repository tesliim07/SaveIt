using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodSaver.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteDecisionToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeleteDecision",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteDecision",
                table: "Users");
        }
    }
}
