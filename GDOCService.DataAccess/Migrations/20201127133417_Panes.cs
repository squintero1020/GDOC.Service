using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace GDOCService.DataAccess.Migrations
{
    public partial class Panes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Miguel",
                table: "Guns",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Panes",
                columns: table => new
                {
                    PanId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    flour = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Panes", x => x.PanId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Panes");

            migrationBuilder.DropColumn(
                name: "Miguel",
                table: "Guns");
        }
    }
}
