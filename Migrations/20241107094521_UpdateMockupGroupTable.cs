using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HxStudioFileUploadService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMockupGroupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MockupTypeId",
                table: "MockupGroups",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_MockupGroups_MockupTypeId",
                table: "MockupGroups",
                column: "MockupTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MockupGroups_MockupTypes_MockupTypeId",
                table: "MockupGroups",
                column: "MockupTypeId",
                principalTable: "MockupTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MockupGroups_MockupTypes_MockupTypeId",
                table: "MockupGroups");

            migrationBuilder.DropIndex(
                name: "IX_MockupGroups_MockupTypeId",
                table: "MockupGroups");

            migrationBuilder.DropColumn(
                name: "MockupTypeId",
                table: "MockupGroups");
        }
    }
}
