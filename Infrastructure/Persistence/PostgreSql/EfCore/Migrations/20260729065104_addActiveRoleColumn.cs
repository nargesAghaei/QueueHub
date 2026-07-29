using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.persistence.PostgreSql.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class addActiveRoleColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveRoleId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ActiveRoleId",
                table: "Users",
                column: "ActiveRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_ActiveRoleId",
                table: "Users",
                column: "ActiveRoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_ActiveRoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ActiveRoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ActiveRoleId",
                table: "Users");
        }
    }
}
