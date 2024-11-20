using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HxStudioFileUploadService.Migrations
{
    /// <inheritdoc />
    public partial class updateSeedDataAndDeliverableTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcessTypeId",
                table: "Deliverables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProcessTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProcessTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProcessTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProcessTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProcessTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProcessTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 7,
                column: "ProcessTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 8,
                column: "ProcessTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 9,
                column: "ProcessTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 10,
                column: "ProcessTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 11,
                column: "ProcessTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 12,
                column: "ProcessTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 13,
                column: "ProcessTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 14,
                column: "ProcessTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 15,
                column: "ProcessTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 16,
                column: "ProcessTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 17,
                column: "ProcessTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 18,
                column: "ProcessTypeId",
                value: 4);

            migrationBuilder.InsertData(
                table: "Deliverables",
                columns: new[] { "Id", "DeliverableName", "ProcessTypeId" },
                values: new object[] { 19, "Competitor Analysis", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Deliverables_ProcessTypeId",
                table: "Deliverables",
                column: "ProcessTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliverables_ProcessTypes_ProcessTypeId",
                table: "Deliverables",
                column: "ProcessTypeId",
                principalTable: "ProcessTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliverables_ProcessTypes_ProcessTypeId",
                table: "Deliverables");

            migrationBuilder.DropIndex(
                name: "IX_Deliverables_ProcessTypeId",
                table: "Deliverables");

            migrationBuilder.DeleteData(
                table: "Deliverables",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DropColumn(
                name: "ProcessTypeId",
                table: "Deliverables");
        }
    }
}
