using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SS.Template.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Example",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Example", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Example_NormalizedName",
                table: "Example",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Example_Status",
                table: "Example",
                column: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Example");
        }
    }
}
