using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErkekKuaforu_WebProgramlama.Migrations
{
    /// <inheritdoc />
    public partial class calisanlarvehizmetler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalisanBilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KisiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimeStart = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeEnd = table.Column<TimeSpan>(type: "time", nullable: true),
                    OnDay = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalisanBilgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalisanBilgileri_AspNetUsers_KisiId",
                        column: x => x.KisiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hizmetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ucret = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hizmetler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalisanHizmetleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HizmetId = table.Column<int>(type: "int", nullable: false),
                    KisiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Sure = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalisanHizmetleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalisanHizmetleri_AspNetUsers_KisiId",
                        column: x => x.KisiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalisanHizmetleri_Hizmetler_HizmetId",
                        column: x => x.HizmetId,
                        principalTable: "Hizmetler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalisanBilgileri_KisiId",
                table: "CalisanBilgileri",
                column: "KisiId");

            migrationBuilder.CreateIndex(
                name: "IX_CalisanHizmetleri_HizmetId",
                table: "CalisanHizmetleri",
                column: "HizmetId");

            migrationBuilder.CreateIndex(
                name: "IX_CalisanHizmetleri_KisiId",
                table: "CalisanHizmetleri",
                column: "KisiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalisanBilgileri");

            migrationBuilder.DropTable(
                name: "CalisanHizmetleri");

            migrationBuilder.DropTable(
                name: "Hizmetler");
        }
    }
}
