using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Migrations
{
    [Migration(8, TransactionBehavior.None)] // Операция будет выполняться вне транзакции
    public class StockSkuIdx : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql("CREATE INDEX CONCURRENTLY stocks_ski_id_idx ON stocks(sku_id);");
        }
    }
}