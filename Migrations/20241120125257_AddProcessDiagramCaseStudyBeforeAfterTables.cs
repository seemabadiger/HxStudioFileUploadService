using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HxStudioFileUploadService.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessDiagramCaseStudyBeforeAfterTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Mockups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            //migrationBuilder.AddColumn<int>(
            //    name: "MockupTypeId",
            //    table: "MockupGroups",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "BeforeAfters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MockupGroupId = table.Column<int>(type: "int", nullable: false),
                    BeforeDesignFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeforeDesignFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AfterDesignFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AfterDesignFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeforeTags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AfterTags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeforeAfters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeforeAfters_MockupGroups_MockupGroupId",
                        column: x => x.MockupGroupId,
                        principalTable: "MockupGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseStudies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MockupGroupId = table.Column<int>(type: "int", nullable: false),
                    CaseStudyFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseStudyFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseStudies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseStudies_MockupGroups_MockupGroupId",
                        column: x => x.MockupGroupId,
                        principalTable: "MockupGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deliverables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliverableName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliverables", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "MockupTypes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MockupTypes", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "ProcessTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessDiagrams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessTypeId = table.Column<int>(type: "int", nullable: false),
                    DeliverableId = table.Column<int>(type: "int", nullable: false),
                    DeliverableFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliverableFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliverableLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessDiagrams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessDiagrams_Deliverables_DeliverableId",
                        column: x => x.DeliverableId,
                        principalTable: "Deliverables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessDiagrams_ProcessTypes_ProcessTypeId",
                        column: x => x.ProcessTypeId,
                        principalTable: "ProcessTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Deliverables",
                columns: new[] { "Id", "DeliverableName" },
                values: new object[,]
                {
                    { 1, "Empathy Mapping" },
                    { 2, "Journey Mapping" },
                    { 3, "Task Flow" },
                    { 4, "Personas" },
                    { 5, "Scenarios" },
                    { 6, "Heuristic Evaluation" },
                    { 7, "Information Architecture (Block Diagram)" },
                    { 8, "Low-hi Fidelity Wireframes" },
                    { 9, "Prototype" },
                    { 10, "Research Report" },
                    { 11, "Branding Style Guide" },
                    { 12, "Visual Design" },
                    { 13, "Design System (Assets, Micro interactions)" },
                    { 14, "Clickable Prototype" },
                    { 15, "HTML CSS Markups" },
                    { 16, "Atomic Design" },
                    { 17, "Accessibilty Compliance(WCAG)" },
                    { 18, "React/Angular based components" }
                });

            //migrationBuilder.InsertData(
            //    table: "MockupTypes",
            //    columns: new[] { "Id", "Name" },
            //    values: new object[,]
            //    {
            //        { 1, "Visual Samples" },
            //        { 2, "Case Studies" },
            //        { 3, "Process Diagram & Artifacts" },
            //        { 4, "Before After" }
            //    });

            migrationBuilder.InsertData(
                table: "ProcessTypes",
                columns: new[] { "Id", "ProcessName" },
                values: new object[,]
                {
                    { 1, "Discover" },
                    { 2, "Define" },
                    { 3, "Design" },
                    { 4, "Develop" }
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_MockupGroups_MockupTypeId",
            //    table: "MockupGroups",
            //    column: "MockupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BeforeAfters_MockupGroupId",
                table: "BeforeAfters",
                column: "MockupGroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseStudies_MockupGroupId",
                table: "CaseStudies",
                column: "MockupGroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDiagrams_DeliverableId",
                table: "ProcessDiagrams",
                column: "DeliverableId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDiagrams_ProcessTypeId",
                table: "ProcessDiagrams",
                column: "ProcessTypeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MockupGroups_MockupTypes_MockupTypeId",
            //    table: "MockupGroups",
            //    column: "MockupTypeId",
            //    principalTable: "MockupTypes",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_MockupGroups_MockupTypes_MockupTypeId",
            //    table: "MockupGroups");

            migrationBuilder.DropTable(
                name: "BeforeAfters");

            migrationBuilder.DropTable(
                name: "CaseStudies");

            //migrationBuilder.DropTable(
            //    name: "MockupTypes");

            migrationBuilder.DropTable(
                name: "ProcessDiagrams");

            migrationBuilder.DropTable(
                name: "Deliverables");

            migrationBuilder.DropTable(
                name: "ProcessTypes");

            //migrationBuilder.DropIndex(
            //    name: "IX_MockupGroups_MockupTypeId",
            //    table: "MockupGroups");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Mockups");

            //migrationBuilder.DropColumn(
            //    name: "MockupTypeId",
            //    table: "MockupGroups");
        }
    }
}
