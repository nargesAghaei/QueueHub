using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.PostgreSql.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddCityInOrgan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Organizations");

            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                table: "Organizations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CityId",
                table: "Organizations",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Cities_CityId",
                table: "Organizations",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Cities_CityId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_CityId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Organizations",
                type: "text",
                nullable: true);
        }
    }
}
