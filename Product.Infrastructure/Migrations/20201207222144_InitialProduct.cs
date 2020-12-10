using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Product.Infrastructure.Migrations
{
    public partial class InitialProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photo_Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photo_Width = table.Column<int>(type: "int", nullable: true),
                    Photo_Height = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastChangeDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
