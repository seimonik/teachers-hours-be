using Microsoft.EntityFrameworkCore.Migrations;
using TH.Dal.Enums;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TH.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecializationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:document_types", "ordinary,request,calculation")
                .Annotation("Npgsql:Enum:education_level_types", "bachelor,specialty,magistracy,postgraduate")
                .OldAnnotation("Npgsql:Enum:document_types", "ordinary,request,calculation");

            migrationBuilder.CreateTable(
                name: "Specializations",
                schema: "lookups",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EducationLevel = table.Column<EducationLevelTypes>(type: "education_level_types", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.Code);
                });

            migrationBuilder.InsertData(
                schema: "lookups",
                table: "Specializations",
                columns: new[] { "Code", "EducationLevel", "Name" },
                values: new object[,]
                {
                    { "01.06.01", EducationLevelTypes.Postgraduate, "ММ" },
                    { "02.03.02", EducationLevelTypes.Bachelor, "ФИИТ" },
                    { "02.03.03", EducationLevelTypes.Bachelor, "МОАИС" },
                    { "02.04.03", EducationLevelTypes.Magistracy, "МОАИС" },
                    { "02.06.01", EducationLevelTypes.Postgraduate, "КИН" },
                    { "09.03.01", EducationLevelTypes.Bachelor, "ИВТ" },
                    { "09.03.03", EducationLevelTypes.Bachelor, "ПИ" },
                    { "09.04.01", EducationLevelTypes.Magistracy, "ИВТ" },
                    { "09.06.01", EducationLevelTypes.Postgraduate, "ИВТ" },
                    { "1.1.5", EducationLevelTypes.Postgraduate, "МЛ" },
                    { "1.2.2", EducationLevelTypes.Postgraduate, "ММ" },
                    { "1.2.3", EducationLevelTypes.Postgraduate, "ТИ" },
                    { "10.05.01", EducationLevelTypes.Specialty, "КБ" },
                    { "2.3.1", EducationLevelTypes.Postgraduate, "СА" },
                    { "27.03.03", EducationLevelTypes.Bachelor, "САиУ" },
                    { "44.03.01", EducationLevelTypes.Bachelor, "ПО" },
                    { "44.04.01", EducationLevelTypes.Magistracy, "ПО" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Specializations",
                schema: "lookups");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:document_types", "ordinary,request,calculation")
                .OldAnnotation("Npgsql:Enum:document_types", "ordinary,request,calculation")
                .OldAnnotation("Npgsql:Enum:education_level_types", "bachelor,specialty,magistracy,postgraduate");
        }
    }
}
