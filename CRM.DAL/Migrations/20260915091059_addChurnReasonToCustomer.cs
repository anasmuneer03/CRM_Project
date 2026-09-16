using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addChurnReasonToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChurnReason",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChurnReason",
                table: "Customers");
        }
    }
}
