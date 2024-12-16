using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Informing.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseParameter",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category = table.Column<int>(type: "int", nullable: false),
                    field = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    kernelBaseParameterId = table.Column<long>(type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseParameter", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    emailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fullname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Information",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Information", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    informingType = table.Column<int>(type: "int", nullable: false),
                    informingSendType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    platformType = table.Column<int>(type: "int", nullable: false),
                    version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    deviceToken = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    Contactid = table.Column<long>(type: "bigint", nullable: true),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.id);
                    table.ForeignKey(
                        name: "FK_Device_Contact_Contactid",
                        column: x => x.Contactid,
                        principalTable: "Contact",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GroupContact",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contactId = table.Column<long>(type: "bigint", nullable: false),
                    groupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupContact", x => x.id);
                    table.ForeignKey(
                        name: "FK_GroupContact_Contact_contactId",
                        column: x => x.contactId,
                        principalTable: "Contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupContact_Group_groupId",
                        column: x => x.groupId,
                        principalTable: "Group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactInformation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contactId = table.Column<long>(type: "bigint", nullable: false),
                    infromationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInformation", x => x.id);
                    table.ForeignKey(
                        name: "FK_ContactInformation_Contact_contactId",
                        column: x => x.contactId,
                        principalTable: "Contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactInformation_Information_infromationId",
                        column: x => x.infromationId,
                        principalTable: "Information",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupInformation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    groupId = table.Column<long>(type: "bigint", nullable: false),
                    infromationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupInformation", x => x.id);
                    table.ForeignKey(
                        name: "FK_GroupInformation_Group_groupId",
                        column: x => x.groupId,
                        principalTable: "Group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupInformation_Information_infromationId",
                        column: x => x.infromationId,
                        principalTable: "Information",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InformationLog",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<int>(type: "int", nullable: false),
                    contactId = table.Column<long>(type: "bigint", nullable: false),
                    informationId = table.Column<long>(type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationLog", x => x.id);
                    table.ForeignKey(
                        name: "FK_InformationLog_Contact_contactId",
                        column: x => x.contactId,
                        principalTable: "Contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InformationLog_Information_informationId",
                        column: x => x.informationId,
                        principalTable: "Information",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseParameter_field",
                table: "BaseParameter",
                column: "field",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contact_userId",
                table: "Contact",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformation_contactId",
                table: "ContactInformation",
                column: "contactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformation_infromationId",
                table: "ContactInformation",
                column: "infromationId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_Contactid",
                table: "Device",
                column: "Contactid");

            migrationBuilder.CreateIndex(
                name: "IX_Device_deviceToken",
                table: "Device",
                column: "deviceToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Group_title",
                table: "Group",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupContact_contactId",
                table: "GroupContact",
                column: "contactId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupContact_groupId",
                table: "GroupContact",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInformation_groupId",
                table: "GroupInformation",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInformation_infromationId",
                table: "GroupInformation",
                column: "infromationId");

            migrationBuilder.CreateIndex(
                name: "IX_Information_title",
                table: "Information",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "IX_InformationLog_contactId",
                table: "InformationLog",
                column: "contactId");

            migrationBuilder.CreateIndex(
                name: "IX_InformationLog_informationId",
                table: "InformationLog",
                column: "informationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseParameter");

            migrationBuilder.DropTable(
                name: "ContactInformation");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "GroupContact");

            migrationBuilder.DropTable(
                name: "GroupInformation");

            migrationBuilder.DropTable(
                name: "InformationLog");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Information");
        }
    }
}
