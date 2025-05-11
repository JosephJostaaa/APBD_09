using Tutorial9.Dto;

namespace Tutorial9.Services;

public interface IWarehouseService
{
    public Task<int> FulfillOrderWithProcedureAsync(CancellationToken ct, ProductWarehouseDto productWarehouseDto);
    public Task<int> FulfillOrderAsync(CancellationToken ct, ProductWarehouseDto productWarehouseDto);
}