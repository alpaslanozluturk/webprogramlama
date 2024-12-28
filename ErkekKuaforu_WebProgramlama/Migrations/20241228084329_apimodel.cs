using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErkekKuaforu_WebProgramlama.Migrations
{
    /// <inheritdoc />
    public partial class apimodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KisiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BirinciFotoIsim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirinciFotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IkinciFotoIsim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IkinciFotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UcuncuFotoIsim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UcuncuFotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiModels_AspNetUsers_KisiId",
                        column: x => x.KisiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiModels_KisiId",
                table: "ApiModels",
                column: "KisiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiModels");
        }
    }
}
