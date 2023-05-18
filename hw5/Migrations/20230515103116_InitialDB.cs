using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hw6.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    categ_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categ_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.categ_id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    exp_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    exp_cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    exp_comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    exp_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    exp_categ = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.exp_id);
                    table.ForeignKey(
                        name: "FK_Expenses_Categories_exp_categ",
                        column: x => x.exp_categ,
                        principalTable: "Categories",
                        principalColumn: "categ_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_exp_categ",
                table: "Expenses",
                column: "exp_categ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
