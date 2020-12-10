using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Product.Infrastructure.Migrations
{
    public partial class InitialProductReadModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AggregateRootId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhotoWidth = table.Column<int>(type: "int", nullable: false),
                    PhotoHeight = table.Column<int>(type: "int", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReadModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductReadModel");
        }
    }
}
