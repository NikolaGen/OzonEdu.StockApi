using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Migrations
{
    [Migration(5)]
    public class DelieveryRequests: Migration
    {
        public override void Up()
        {
            Create
                .Table("delievery_requests")
                .WithColumn("id").AsInt64().Identity().PrimaryKey()
                .WithColumn("request_id").AsInt64().NotNullable()
                .WithColumn("request_status").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("delievery_requests");
        }
    }
}