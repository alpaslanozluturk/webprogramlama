using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErkekKuaforu_WebProgramlama.Migrations
{
    /// <inheritdoc />
    public partial class randevular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Randevular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusteriId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GirisSaati = table.Column<TimeSpan>(type: "time", nullable: false),
                    CikisSaati = table.Column<TimeSpan>(type: "time", nullable: false),
                    CalisanHizmetleri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToplamUcret = table.Column<int>(type: "int", nullable: false),
                    isStatus = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Randevular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Randevular_AspNetUsers_MusteriId",
                        column: x => x.MusteriId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_MusteriId",
                table: "Randevular",
                column: "MusteriId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Randevular");
        }
    }
}
