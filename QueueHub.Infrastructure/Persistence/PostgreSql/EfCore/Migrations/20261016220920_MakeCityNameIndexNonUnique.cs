using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.PostgreSql.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class MakeCityNameIndexNonUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cities_ProvinceId_Name",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ExternalCode",
                table: "Cities",
                column: "ExternalCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ProvinceId_Name",
                table: "Cities",
                columns: new[] { "ProvinceId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cities_ExternalCode",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_ProvinceId_Name",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ProvinceId_Name",
                table: "Cities",
                columns: new[] { "ProvinceId", "Name" },
                unique: true);
        }
    }
}
