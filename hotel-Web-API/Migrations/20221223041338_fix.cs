using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotelWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_roomBookings",
                table: "roomBookings");

            migrationBuilder.AlterColumn<bool>(
                name: "isFirstBooking",
                table: "users",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoombookingId",
                table: "roomBookings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roomBookings",
                table: "roomBookings",
                column: "RoombookingId");

            migrationBuilder.CreateIndex(
                name: "IX_roomBookings_bookingId",
                table: "roomBookings",
                column: "bookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_roomBookings",
                table: "roomBookings");

            migrationBuilder.DropIndex(
                name: "IX_roomBookings_bookingId",
                table: "roomBookings");

            migrationBuilder.DropColumn(
                name: "RoombookingId",
                table: "roomBookings");

            migrationBuilder.AlterColumn<bool>(
                name: "isFirstBooking",
                table: "users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roomBookings",
                table: "roomBookings",
                columns: new[] { "bookingId", "roomId" });
        }
    }
}
