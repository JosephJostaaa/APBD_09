namespace Tutorial9.Repositories;

public interface IProductRepository
{
    public Task<bool> ExistsByIdAsync(CancellationToken cancellationToken, int id);
    public Task<Decimal> GetProductPriceByIdAsync(CancellationToken cancellationToken, int id);
}