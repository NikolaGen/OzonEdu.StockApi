namespace OzonEdu.StockApi.Domain.Contracts
{
    public interface IRepository<TAggregationRoot> //TAggregationRoot — это «заполнитель» (placeholder). Когда вы пишете IRepository<StockItem>, вы подставляете StockItem вместо TAggregationRoot.
    {
        IUnitOfWork UnitOfWork { get; }
    }
}