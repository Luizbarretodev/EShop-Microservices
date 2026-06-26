using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.ProductApi.Migrations
{
    public partial class SeedProduct : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Products(Name, Price, Description, Stock, ImageURL, CategoryId) " +
                   "Values('Notebook', 7.55, '20 -subject notebook', 10, 'Notebook1.JPG', 2)");

            mb.Sql("Insert Into Products(Name,Price,Description,Stock,ImageURL,CategoryId) " +
                   "Values('Pencil',3.45,'Black Pencil',20,'lapis1.jpg',2)");

            mb.Sql("Insert Into Products(Name,Price,Description,Stock,ImageURL,CategoryId) " +
                   "Values('Paper Clips',5.33,'Paper clips',50,'clips1.jpg',2)");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Products");
        }
    }
}
