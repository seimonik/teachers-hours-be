using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TH.Dal.Migrations
{
    /// <inheritdoc />
    public partial class DeleteEndRowColumnInDocumentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndRow",
                table: "Documents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EndRow",
                table: "Documents",
                type: "integer",
                nullable: true);
        }
    }
}
