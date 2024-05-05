using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TH.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeNormsLookupsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "lookups");

            migrationBuilder.CreateTable(
                name: "TimeNorms",
                schema: "lookups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Norm = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeNorms", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "lookups",
                table: "TimeNorms",
                columns: new[] { "Id", "Code", "Name", "Norm" },
                values: new object[,]
                {
                    { new Guid("440014d1-fa11-455e-84cf-64bcd8d1b391"), "Coursework3", "Норма часов по курсовым работам (3 курс)", 0 },
                    { new Guid("4b4b7e08-bcf6-4a63-862a-e596cbc7931d"), "Coursework2", "Норма часов по курсовым работам (2 курс)", 0 },
                    { new Guid("53f1740e-f13f-4d5a-8068-e566f5caa87a"), "ProductionPractice", "Производственная практика", 0 },
                    { new Guid("54df6a58-1185-4b46-949d-7287daea86dc"), "FinalQualifyingWorkMagistracy", "ВКР (магистратура)", 0 },
                    { new Guid("6b06b872-5147-4658-ba13-66956d196d70"), "Coursework2PO", "Норма часов по курсовым работам для ПО (2 курс)", 0 },
                    { new Guid("b9bd6779-ccf4-440d-809b-7cbaa183cebb"), "Coursework3PO", "Норма часов по курсовым работам для ПО (3 курс)", 0 },
                    { new Guid("bdb7ced0-e611-41b3-ba11-9770fd61afc9"), "EducationalPractice", "Учебная практика (НИР)", 0 },
                    { new Guid("e1d63cf1-4672-4619-b116-35d6cf41eca4"), "FinalQualifyingWorkBachelor", "ВКР (бакалавриат)", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeNorms_Code",
                schema: "lookups",
                table: "TimeNorms",
                column: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeNorms",
                schema: "lookups");
        }
    }
}
