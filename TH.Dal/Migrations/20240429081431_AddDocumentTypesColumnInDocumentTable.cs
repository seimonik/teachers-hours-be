using Microsoft.EntityFrameworkCore.Migrations;
using TH.Dal.Enums;

#nullable disable

namespace TH.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentTypesColumnInDocumentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:document_types", "ordinary,request,calculation");

            migrationBuilder.AddColumn<DocumentTypes>(
                name: "DocumentType",
                table: "Documents",
                type: "document_types",
                nullable: false,
                defaultValue: DocumentTypes.Ordinary);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "Documents");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:document_types", "ordinary,request,calculation");
        }
    }
}
