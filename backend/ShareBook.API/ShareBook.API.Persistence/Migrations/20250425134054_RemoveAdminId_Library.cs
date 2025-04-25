using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareBook.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAdminId_Library : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Libraries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Libraries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
