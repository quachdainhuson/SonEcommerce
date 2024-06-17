using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonEcommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppProducts",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<Guid>(
                name: "ManufacturerId",
                table: "AppProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "AppProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserCity",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserDistrict",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserNote",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserWard",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserCity",
                table: "AbpUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserDistrict",
                table: "AbpUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserWard",
                table: "AbpUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppProducts_CategoryId",
                table: "AppProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProducts_ManufacturerId",
                table: "AppProducts",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductAttributeVarchars_AttributeId",
                table: "AppProductAttributeVarchars",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductAttributeVarchars_ProductId",
                table: "AppProductAttributeVarchars",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductAttributeTexts_AttributeId",
                table: "AppProductAttributeTexts",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductAttributeTexts_ProductId",
                table: "AppProductAttributeTexts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductAttributeInts_AttributeId",
                table: "AppProductAttributeInts",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductAttributeInts_ProductId",
                table: "AppProductAttributeInts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductAttributeDecimals_AttributeId",
                table: "AppProductAttributeDecimals",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductAttributeDecimals_ProductId",
                table: "AppProductAttributeDecimals",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductAttributeDateTimes_AttributeId",
                table: "AppProductAttributeDateTimes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductAttributeDateTimes_ProductId",
                table: "AppProductAttributeDateTimes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOrders_CustomerUserId",
                table: "AppOrders",
                column: "CustomerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOrderItems_OrderId",
                table: "AppOrderItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrderItems_AppOrders_OrderId",
                table: "AppOrderItems",
                column: "OrderId",
                principalTable: "AppOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrderItems_AppProducts_ProductId",
                table: "AppOrderItems",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrders_AbpUsers_CustomerUserId",
                table: "AppOrders",
                column: "CustomerUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductAttributeDateTimes_AppProductAttributes_AttributeId",
                table: "AppProductAttributeDateTimes",
                column: "AttributeId",
                principalTable: "AppProductAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductAttributeDateTimes_AppProducts_ProductId",
                table: "AppProductAttributeDateTimes",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductAttributeDecimals_AppProductAttributes_AttributeId",
                table: "AppProductAttributeDecimals",
                column: "AttributeId",
                principalTable: "AppProductAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductAttributeDecimals_AppProducts_ProductId",
                table: "AppProductAttributeDecimals",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductAttributeInts_AppProductAttributes_AttributeId",
                table: "AppProductAttributeInts",
                column: "AttributeId",
                principalTable: "AppProductAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductAttributeInts_AppProducts_ProductId",
                table: "AppProductAttributeInts",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductAttributeTexts_AppProductAttributes_AttributeId",
                table: "AppProductAttributeTexts",
                column: "AttributeId",
                principalTable: "AppProductAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductAttributeTexts_AppProducts_ProductId",
                table: "AppProductAttributeTexts",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductAttributeVarchars_AppProductAttributes_AttributeId",
                table: "AppProductAttributeVarchars",
                column: "AttributeId",
                principalTable: "AppProductAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductAttributeVarchars_AppProducts_ProductId",
                table: "AppProductAttributeVarchars",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProducts_AppManufacturers_ManufacturerId",
                table: "AppProducts",
                column: "ManufacturerId",
                principalTable: "AppManufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProducts_AppProductCategories_CategoryId",
                table: "AppProducts",
                column: "CategoryId",
                principalTable: "AppProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrderItems_AppOrders_OrderId",
                table: "AppOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AppOrderItems_AppProducts_ProductId",
                table: "AppOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AppOrders_AbpUsers_CustomerUserId",
                table: "AppOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductAttributeDateTimes_AppProductAttributes_AttributeId",
                table: "AppProductAttributeDateTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductAttributeDateTimes_AppProducts_ProductId",
                table: "AppProductAttributeDateTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductAttributeDecimals_AppProductAttributes_AttributeId",
                table: "AppProductAttributeDecimals");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductAttributeDecimals_AppProducts_ProductId",
                table: "AppProductAttributeDecimals");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductAttributeInts_AppProductAttributes_AttributeId",
                table: "AppProductAttributeInts");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductAttributeInts_AppProducts_ProductId",
                table: "AppProductAttributeInts");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductAttributeTexts_AppProductAttributes_AttributeId",
                table: "AppProductAttributeTexts");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductAttributeTexts_AppProducts_ProductId",
                table: "AppProductAttributeTexts");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductAttributeVarchars_AppProductAttributes_AttributeId",
                table: "AppProductAttributeVarchars");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductAttributeVarchars_AppProducts_ProductId",
                table: "AppProductAttributeVarchars");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProducts_AppManufacturers_ManufacturerId",
                table: "AppProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProducts_AppProductCategories_CategoryId",
                table: "AppProducts");

            migrationBuilder.DropIndex(
                name: "IX_AppProducts_CategoryId",
                table: "AppProducts");

            migrationBuilder.DropIndex(
                name: "IX_AppProducts_ManufacturerId",
                table: "AppProducts");

            migrationBuilder.DropIndex(
                name: "IX_AppProductAttributeVarchars_AttributeId",
                table: "AppProductAttributeVarchars");

            migrationBuilder.DropIndex(
                name: "IX_AppProductAttributeVarchars_ProductId",
                table: "AppProductAttributeVarchars");

            migrationBuilder.DropIndex(
                name: "IX_AppProductAttributeTexts_AttributeId",
                table: "AppProductAttributeTexts");

            migrationBuilder.DropIndex(
                name: "IX_AppProductAttributeTexts_ProductId",
                table: "AppProductAttributeTexts");

            migrationBuilder.DropIndex(
                name: "IX_AppProductAttributeInts_AttributeId",
                table: "AppProductAttributeInts");

            migrationBuilder.DropIndex(
                name: "IX_AppProductAttributeInts_ProductId",
                table: "AppProductAttributeInts");

            migrationBuilder.DropIndex(
                name: "IX_AppProductAttributeDecimals_AttributeId",
                table: "AppProductAttributeDecimals");

            migrationBuilder.DropIndex(
                name: "IX_AppProductAttributeDecimals_ProductId",
                table: "AppProductAttributeDecimals");

            migrationBuilder.DropIndex(
                name: "IX_AppProductAttributeDateTimes_AttributeId",
                table: "AppProductAttributeDateTimes");

            migrationBuilder.DropIndex(
                name: "IX_AppProductAttributeDateTimes_ProductId",
                table: "AppProductAttributeDateTimes");

            migrationBuilder.DropIndex(
                name: "IX_AppOrders_CustomerUserId",
                table: "AppOrders");

            migrationBuilder.DropIndex(
                name: "IX_AppOrderItems_OrderId",
                table: "AppOrderItems");

            migrationBuilder.DropColumn(
                name: "UserCity",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "UserDistrict",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "UserNote",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "UserWard",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "UserCity",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "UserDistrict",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "UserWard",
                table: "AbpUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppProducts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<Guid>(
                name: "ManufacturerId",
                table: "AppProducts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "AppProducts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
