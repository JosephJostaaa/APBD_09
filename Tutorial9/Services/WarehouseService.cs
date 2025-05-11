using Tutorial9.Dto;
using Tutorial9.Exceptions;
using Tutorial9.Mappers;
using Tutorial9.Model;
using Tutorial9.Repositories;

namespace Tutorial9.Services;

public class WarehouseService : IWarehouseService
{
    
    private readonly IOrderRepository orderRepository;
    private readonly IProductRepository productRepository;
    private readonly IWarehouseRepository warehouseRepository;
    private readonly IProductWarehouseRepository productWarehouseRepository;

    public WarehouseService(IOrderRepository orderRepository, IProductRepository productRepository, IWarehouseRepository warehouseRepository, IProductWarehouseRepository productWarehouseRepository)
    {
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
        this.warehouseRepository = warehouseRepository;
        this.productWarehouseRepository = productWarehouseRepository;
    }

    public async Task<int> FulfillOrderAsync(CancellationToken ct, ProductWarehouseDto productWarehouseDto)
    {
        if (!await productRepository.ExistsByIdAsync(ct, productWarehouseDto.IdProduct))
            throw new NotFoundException($"Product with id {productWarehouseDto.IdProduct} does not exist");
        
        if (!await warehouseRepository.ExistsByIdAsync(ct, productWarehouseDto.IdWarehouse))
            throw new NotFoundException($"Warehouse with id {productWarehouseDto.IdWarehouse} does not exist");
        
        var filter = new SimpleOrderFilter();
        filter.Amount = productWarehouseDto.Amount;
        filter.CreatedAt = productWarehouseDto.CreatedAt;
        filter.IdProduct = productWarehouseDto.IdProduct;
        
        
        List<Order> orders = await orderRepository.GetOrdersAsync(ct, new SimpleOrderFilter());
        
        if (!orders.Any())
            throw new NotFoundException($"Order with id {productWarehouseDto.IdProduct} does not exist");
        
        Order order = orders.First();

        if (await productWarehouseRepository.ExistsByOrderIdAsync(ct, order.IdOrder))
            throw new ArgumentException($"Order with id {order.IdOrder} has already been fulfilled");
        
        Decimal productPrice = await productRepository.GetProductPriceByIdAsync(ct, productWarehouseDto.IdProduct);
        
        Decimal totalPrice = productPrice * productWarehouseDto.Amount;
        return await orderRepository.FullfillOrderAsync(ct, ProductWarehouseMapper.ToProductWarehouseRecord(productWarehouseDto, totalPrice, DateTime.UtcNow, order.IdOrder));
    }

    public async Task<int> FulfillOrderWithProcedureAsync(CancellationToken ct, ProductWarehouseDto productWarehouseDto)
    {
        return await orderRepository.FulfillOrderProcedurallyAsync(ct, productWarehouseDto.IdWarehouse, productWarehouseDto.IdProduct, productWarehouseDto.Amount, productWarehouseDto.CreatedAt);
    }
}