using Tutorial9.Dto;
using Tutorial9.Model;

namespace Tutorial9.Repositories;

public interface IOrderRepository
{
    public Task<int> FullfillOrderAsync(CancellationToken ct, ProductWarehouseRecord productWarehouseRecord);

    public Task<List<Order>> GetOrdersAsync(CancellationToken ct, SimpleOrderFilter filter);
}