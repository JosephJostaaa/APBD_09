namespace Tutorial9.Dto;

public class SimpleOrderFilter
{
    public int? IdOrder { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? Amount { get; set; }

    public SimpleOrderFilter(int? idOrder = null, DateTime? createdAt = null, int? amount = null)
    {
        IdOrder = idOrder;
        CreatedAt = createdAt;
        Amount = amount;
    }
}