using Tutorial9.Dto;
using Tutorial9.Model;

namespace Tutorial9.Mappers;

public class ProductWarehouseMapper
{
    public static ProductWarehouseRecord ToProductWarehouseRecord(ProductWarehouseDto productWarehouseDto, decimal price, DateTime createdAt, int orderId)
    {
        return new ProductWarehouseRecord
        {
            IdProduct = productWarehouseDto.IdProduct,
            IdWarehouse = productWarehouseDto.IdWarehouse,
            IdOrder = orderId,
            Amount = productWarehouseDto.Amount,
            Price = price,
            CreatedAt = DateTime.UtcNow
        };
    }
    
}