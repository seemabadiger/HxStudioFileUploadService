using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HxStudioFileUploadService.Migrations
{
    /// <inheritdoc />
    public partial class AddMockupTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MockupTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MockupTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MockupTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Visual Samples" },
                    { 2, "Case Studies" },
                    { 3, "Process Diagram & Artifacts" },
                    { 4, "Before After" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MockupTypes");
        }
    }
}
