using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashTransit.EntityFramework.Migrations
{
    public partial class Fault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fault",
                schema: "mt",
                columns: table => new
                {
                    FaultId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaisedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fault", x => x.FaultId);
                    table.ForeignKey(
                        name: "FK_Fault_Message_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "mt",
                        principalTable: "Message",
                        principalColumn: "MessageId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fault_MessageId",
                schema: "mt",
                table: "Fault",
                column: "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fault",
                schema: "mt");
        }
    }
}
