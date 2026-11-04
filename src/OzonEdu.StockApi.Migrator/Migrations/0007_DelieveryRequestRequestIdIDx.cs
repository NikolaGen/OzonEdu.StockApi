using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Migrations
{
    [Migration(7)]
    public class DelieveryRequestRequestIdIDx : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create
                .Index("delivery_requests_request_id_idx")
                .OnTable("delievery_requests")
                .InSchema("public")
                .OnColumn("request_id");
        }
    }
}