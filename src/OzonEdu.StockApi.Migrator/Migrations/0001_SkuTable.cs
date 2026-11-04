using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Migrations
{
    [Migration(1)]
    public class SkuTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE If NOT EXISTS skus (
                    id BIGSERIAL PRIMARY KEY,
                    name TEXT NOT NULL,
                    item_type_id INT NOT NULL,
                    clothing_size INT)");
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE IF EXISTS skus"); 
        }
    }
}