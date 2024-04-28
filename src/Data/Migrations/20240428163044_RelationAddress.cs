using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RelationAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_ApplicationId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_ApplicationId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "Addresses",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_UserId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Addresses",
                newName: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ApplicationId",
                table: "Addresses",
                column: "ApplicationId",
                unique: true,
                filter: "[ApplicationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_ApplicationId",
                table: "Addresses",
                column: "ApplicationId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
