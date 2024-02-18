using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTaskManager.Migrations
{
    /// <inheritdoc />
    public partial class priorityEntityIsModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descrption",
                table: "Priority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descrption",
                table: "Priority",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
