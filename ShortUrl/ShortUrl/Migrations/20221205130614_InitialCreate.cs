using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShortUrl.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Urls",
                columns: table => new
                {
                    ShortURL = table.Column<string>(type: "varchar(767)", nullable: false),
                    //Id = table.Column<int>(type: "int", nullable: false),
                    FullURL = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urls", x => x.ShortURL);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Urls");
        }
    }
}
