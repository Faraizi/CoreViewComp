using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankViewComp.Migrations
{
    /// <inheritdoc />
    public partial class initialA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Type_Tid",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Tid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Tid);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Types_Tid",
                table: "Customer",
                column: "Tid",
                principalTable: "Types",
                principalColumn: "Tid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Types_Tid",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    Tid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.Tid);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Type_Tid",
                table: "Customer",
                column: "Tid",
                principalTable: "Type",
                principalColumn: "Tid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
