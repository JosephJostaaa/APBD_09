namespace Tutorial9.Repositories;

public interface IProductWarehouseRepository
{
    public Task<bool> ExistsByOrderIdAsync(CancellationToken cancellationToken, int id);
}