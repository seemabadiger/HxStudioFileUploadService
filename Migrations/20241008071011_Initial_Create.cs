using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HxStudioFileUploadService.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subdomain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DomainId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subdomain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subdomain_Domain_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domain",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MockupGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DomainId = table.Column<int>(type: "int", nullable: false),
                    SubDomainId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MockupGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MockupGroups_Domain_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MockupGroups_Subdomain_SubDomainId",
                        column: x => x.SubDomainId,
                        principalTable: "Subdomain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MockupGroupId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsLiked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => new { x.UserId, x.MockupGroupId });
                    table.ForeignKey(
                        name: "FK_Likes_MockupGroups_MockupGroupId",
                        column: x => x.MockupGroupId,
                        principalTable: "MockupGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mockups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MockupGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mockups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mockups_MockupGroups_MockupGroupId",
                        column: x => x.MockupGroupId,
                        principalTable: "MockupGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MockupGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_MockupGroups_MockupGroupId",
                        column: x => x.MockupGroupId,
                        principalTable: "MockupGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_MockupGroupId",
                table: "Likes",
                column: "MockupGroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MockupGroups_DomainId",
                table: "MockupGroups",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_MockupGroups_SubDomainId",
                table: "MockupGroups",
                column: "SubDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Mockups_MockupGroupId",
                table: "Mockups",
                column: "MockupGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Subdomain_DomainId",
                table: "Subdomain",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_MockupGroupId",
                table: "Tags",
                column: "MockupGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Mockups");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "MockupGroups");

            migrationBuilder.DropTable(
                name: "Subdomain");

            migrationBuilder.DropTable(
                name: "Domain");
        }
    }
}
