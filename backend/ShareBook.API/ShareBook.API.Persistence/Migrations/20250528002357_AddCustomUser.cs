using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareBook.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentLibraryId",
                table: "AspNetUsers",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "AppUserLibrary",
                columns: table => new
                {
                    AdministratorsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LibrariesId = table.Column<string>(type: "nvarchar(128)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_AppUserLibrary",
                        x => new { x.AdministratorsId, x.LibrariesId }
                    );
                    table.ForeignKey(
                        name: "FK_AppUserLibrary_AspNetUsers_AdministratorsId",
                        column: x => x.AdministratorsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_AppUserLibrary_Libraries_LibrariesId",
                        column: x => x.LibrariesId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLibrary_LibrariesId",
                table: "AppUserLibrary",
                column: "LibrariesId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AppUserLibrary");

            migrationBuilder.DropColumn(name: "CurrentLibraryId", table: "AspNetUsers");
        }
    }
}
