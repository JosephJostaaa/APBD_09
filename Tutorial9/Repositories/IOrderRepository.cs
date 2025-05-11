using Tutorial9.Dto;
using Tutorial9.Model;

namespace Tutorial9.Repositories;

public interface IOrderRepository
{
    public Task<int> FullfillOrderAsync(CancellationToken ct, ProductWarehouseRecord productWarehouseRecord);

    public Task<int> FulfillOrderProcedurallyAsync(CancellationToken ct, int idWarehouse, int idProduct,
        int amount, DateTime createdAt);

    public Task<List<Order>> GetOrdersAsync(CancellationToken ct, SimpleOrderFilter filter);
}