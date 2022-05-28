using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudApi.Migrations
{
    /// <inheritdoc />
    public partial class MakeDeviceAttachableToHome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HomeId",
                table: "Devices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_HomeId",
                table: "Devices",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Homes_HomeId",
                table: "Devices",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Homes_HomeId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_HomeId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "Devices");
        }
    }
}
