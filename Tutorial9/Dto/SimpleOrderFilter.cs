namespace Tutorial9.Dto;

public class SimpleOrderFilter
{
    public int? IdProduct { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? Amount { get; set; }

    public SimpleOrderFilter(int? idProduct = null, DateTime? createdAt = null, int? amount = null)
    {
        IdProduct = idProduct;
        CreatedAt = createdAt;
        Amount = amount;
    }
}