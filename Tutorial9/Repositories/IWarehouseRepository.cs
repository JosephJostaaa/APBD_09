namespace Tutorial9.Repositories;

public interface IWarehouseRepository
{
    public Task<bool> ExistsByIdAsync(CancellationToken cancellationToken, int id);
}