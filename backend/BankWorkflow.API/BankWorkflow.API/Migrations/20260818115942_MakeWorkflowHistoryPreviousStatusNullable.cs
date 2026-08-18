using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankWorkflow.API.Migrations
{
    public partial class MakeWorkflowHistoryPreviousStatusNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE "WorkflowHistory"
                ALTER COLUMN "PreviousStatus" DROP NOT NULL;
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE "WorkflowHistory"
                ALTER COLUMN "PreviousStatus" SET NOT NULL;
                """);
        }
    }
}