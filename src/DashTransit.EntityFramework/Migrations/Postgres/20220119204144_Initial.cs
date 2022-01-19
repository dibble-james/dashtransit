using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DashTransit.EntityFramework.Migrations.Postgres
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "__audit",
                columns: table => new
                {
                    AuditRecordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: true),
                    InitiatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    SentTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SourceAddress = table.Column<string>(type: "text", nullable: true),
                    DestinationAddress = table.Column<string>(type: "text", nullable: true),
                    ResponseAddress = table.Column<string>(type: "text", nullable: true),
                    FaultAddress = table.Column<string>(type: "text", nullable: true),
                    InputAddress = table.Column<string>(type: "text", nullable: true),
                    ContextType = table.Column<string>(type: "text", nullable: true),
                    MessageType = table.Column<string>(type: "text", nullable: true),
                    Custom = table.Column<string>(type: "text", nullable: true),
                    Headers = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___audit", x => x.AuditRecordId);
                });

            migrationBuilder.CreateTable(
                name: "Fault",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Exceptions = table.Column<string>(type: "text", nullable: false),
                    Produced = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProducedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fault", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__audit");

            migrationBuilder.DropTable(
                name: "Fault");
        }
    }
}
