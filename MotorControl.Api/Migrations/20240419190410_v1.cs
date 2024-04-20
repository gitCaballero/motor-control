using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorControl.Api.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MotorPlate",
                table: "motors",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "ModelYear",
                table: "motors",
                newName: "Plate");

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "motors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "motors");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "motors",
                newName: "MotorPlate");

            migrationBuilder.RenameColumn(
                name: "Plate",
                table: "motors",
                newName: "ModelYear");
        }
    }
}
