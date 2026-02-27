using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathApp.Migrations
{
    /// <inheritdoc />
    public partial class NewTableProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirebaseUUID",
                table: "MathCalculations",
                newName: "FirebaseUuid"
            );

            migrationBuilder.AlterColumn<string>(
                name: "FirebaseUuid",
                table: "MathCalculations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirebaseUuid",
                table: "MathCalculations",
                newName: "FirebaseUUID"
            );

            migrationBuilder.AlterColumn<string>(
                name: "FirebaseUUID",
                table: "MathCalculations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true
            );
        }
    }
}
