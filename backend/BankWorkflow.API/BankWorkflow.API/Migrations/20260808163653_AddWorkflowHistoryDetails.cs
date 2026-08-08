using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankWorkflow.API.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkflowHistoryDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "WorkflowHistory",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "WorkflowHistory");
        }
    }
}
