namespace Tutorial9.Model;

public class ProductWarehouseRecord
{
    public ProductWarehouseRecord (int idProduct, int idWarehouse, int idOrder, int amount, decimal price, DateTime createdAt)
    {
        IdProduct = idProduct;
        IdWarehouse = idWarehouse;
        IdOrder = idOrder;
        Amount = amount;
        Price = price;
        CreatedAt = createdAt;
    }

    public ProductWarehouseRecord()
    {
        
    }

    public int IdProduct{ get; set; }
    public int IdWarehouse { get; set; }
    public int IdOrder { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}