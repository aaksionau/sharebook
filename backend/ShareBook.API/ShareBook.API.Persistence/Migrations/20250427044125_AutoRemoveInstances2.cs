using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareBook.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AutoRemoveInstances2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookInstances_Books_BookId",
                table: "BookInstances");

            migrationBuilder.AddForeignKey(
                name: "FK_BookInstances_Books_BookId",
                table: "BookInstances",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookInstances_Books_BookId",
                table: "BookInstances");

            migrationBuilder.AddForeignKey(
                name: "FK_BookInstances_Books_BookId",
                table: "BookInstances",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
