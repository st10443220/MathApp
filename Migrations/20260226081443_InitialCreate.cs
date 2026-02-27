using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MathCalculations",
                columns: table => new
                {
                    CalculationID = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Operation = table.Column<int>(type: "int", nullable: false),
                    Operand1 = table.Column<double>(type: "float", nullable: false),
                    Operand2 = table.Column<double>(type: "float", nullable: false),
                    Result = table.Column<double>(type: "float", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MathCalculations", x => x.CalculationID);
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MathCalculations");
        }
    }
}
