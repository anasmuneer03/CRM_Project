using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLogging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Tasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Sales",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Sales",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Sales",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Opportunities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Opportunities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Opportunities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Opportunities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Notes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Notes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Notes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Notes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Leads",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Leads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Leads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Leads",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Leads",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Attachments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Attachments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Attachments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Attachments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Activities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedById",
                table: "Tasks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UpdatedById",
                table: "Tasks",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CreatedById",
                table: "Sales",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_UpdatedById",
                table: "Sales",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_CreatedById",
                table: "Opportunities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_UpdatedById",
                table: "Opportunities",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CreatedById",
                table: "Notes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UpdatedById",
                table: "Notes",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CreatedById",
                table: "Leads",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_UpdatedById",
                table: "Leads",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedById",
                table: "Customers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UpdatedById",
                table: "Customers",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CreatedById",
                table: "Attachments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UpdatedById",
                table: "Attachments",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CreatedById",
                table: "Activities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UpdatedById",
                table: "Activities",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_CreatedById",
                table: "Activities",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_UpdatedById",
                table: "Activities",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Users_CreatedById",
                table: "Attachments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Users_UpdatedById",
                table: "Attachments",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_CreatedById",
                table: "Customers",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_UpdatedById",
                table: "Customers",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Users_CreatedById",
                table: "Leads",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Users_UpdatedById",
                table: "Leads",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Users_CreatedById",
                table: "Notes",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Users_UpdatedById",
                table: "Notes",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_Users_CreatedById",
                table: "Opportunities",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_Users_UpdatedById",
                table: "Opportunities",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Users_CreatedById",
                table: "Sales",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Users_UpdatedById",
                table: "Sales",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_CreatedById",
                table: "Tasks",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UpdatedById",
                table: "Tasks",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_CreatedById",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_UpdatedById",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Users_CreatedById",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Users_UpdatedById",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_CreatedById",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_UpdatedById",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Users_CreatedById",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Users_UpdatedById",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Users_CreatedById",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Users_UpdatedById",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_Users_CreatedById",
                table: "Opportunities");

            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_Users_UpdatedById",
                table: "Opportunities");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Users_CreatedById",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Users_UpdatedById",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_CreatedById",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UpdatedById",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_CreatedById",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_UpdatedById",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Sales_CreatedById",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_UpdatedById",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Opportunities_CreatedById",
                table: "Opportunities");

            migrationBuilder.DropIndex(
                name: "IX_Opportunities_UpdatedById",
                table: "Opportunities");

            migrationBuilder.DropIndex(
                name: "IX_Notes_CreatedById",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_UpdatedById",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Leads_CreatedById",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_UpdatedById",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CreatedById",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_UpdatedById",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_CreatedById",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_UpdatedById",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Activities_CreatedById",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_UpdatedById",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Activities");
        }
    }
}
