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
                    Norm = table.Column<double>(type: "double precision", nullable: false)
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
                    { new Guid("051f35df-bc4e-452f-9d1a-4a708372fc2f"), "Coursework3", "Норма часов по курсовым работам (3 курс)", 10.0 },
                    { new Guid("25d1411a-e920-4d44-a036-849698387abf"), "EducationalPractice24", "Учебная практика (НИР) 2 курс 4 семестр", 16.0 },
                    { new Guid("386cfcb3-643d-4b0f-9135-b5ea4f5182cd"), "EducationalPractice36", "Учебная практика (НИР) 3 курс 6 семестр", 16.0 },
                    { new Guid("39d825fb-5e2e-4bc8-b0f6-44d95a122fab"), "FinalQualifyingWorkMagistracy", "ВКР (магистратура)", 34.25 },
                    { new Guid("3ec94a49-266d-4cf2-bfee-fe35516cbd79"), "EducationalPractice35", "Учебная практика (НИР) 3 курс 5 семестр", 18.0 },
                    { new Guid("41371a87-ec4c-4038-bd03-d07c80ff88a7"), "EducationalPractice23", "Учебная практика (НИР) 2 курс 3 семестр", 18.0 },
                    { new Guid("760ecbf0-03be-484b-b076-c37059dc478b"), "EducationalPractice47", "Учебная практика (НИР) 4 курс 7 семестр", 14.0 },
                    { new Guid("837a8528-d235-4649-aa32-1e3b01f33928"), "Coursework2", "Норма часов по курсовым работам (2 курс)", 5.0 },
                    { new Guid("85744b33-9494-4436-ad8d-743b92135b17"), "ProductionPractice", "Производственная практика", 4.0 },
                    { new Guid("89bae101-3f08-4e1e-b93f-d2bd578b9361"), "FinalQualifyingWorkBachelor", "ВКР (бакалавриат)", 24.25 },
                    { new Guid("a90b91fa-b823-4643-8186-9e2cb90d7156"), "Coursework3PO", "Норма часов по курсовым работам для ПО (3 курс)", 10.0 },
                    { new Guid("d456e79d-c74d-4783-b9a1-8aecdfb31750"), "Coursework2PO", "Норма часов по курсовым работам для ПО (2 курс)", 3.0 }
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
