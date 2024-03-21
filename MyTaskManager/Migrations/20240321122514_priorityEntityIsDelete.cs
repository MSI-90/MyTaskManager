using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class priorityEntityIsDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Priority_PrioryId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "Priority");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_PrioryId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PrioryId",
                table: "Tasks");

            migrationBuilder.AddColumn<byte>(
                name: "Prior",
                table: "Tasks",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prior",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "PrioryId",
                table: "Tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Priority",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priority", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PrioryId",
                table: "Tasks",
                column: "PrioryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Priority_PrioryId",
                table: "Tasks",
                column: "PrioryId",
                principalTable: "Priority",
                principalColumn: "Id");
        }
    }
}
