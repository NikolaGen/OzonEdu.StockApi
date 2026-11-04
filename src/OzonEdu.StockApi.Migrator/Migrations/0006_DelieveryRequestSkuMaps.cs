using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Migrations
{
    [Migration(6)]
    public class DelieveryRequestSkuMaps : Migration
    {
        public override void Up()
        {
            Create
                .Table("delievery_request_sku_maps")
                .WithColumn("delievery_request_id").AsInt64().PrimaryKey()
                .WithColumn("sku_id").AsInt64().PrimaryKey();
        }

        public override void Down()
        {
            Delete.Table("delievery_request_sku_maps"); 
        }
    }
}