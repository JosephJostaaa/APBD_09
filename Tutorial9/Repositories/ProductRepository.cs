using Microsoft.Data.SqlClient;

namespace Tutorial9.Repositories;

public class ProductRepository : IProductRepository
{
    
    private readonly string connectionString;
    
    public ProductRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("Default");
    }

    public async Task<bool> ExistsByIdAsync(CancellationToken cancellationToken, int id)
    {
        string query = "SELECT COUNT(*) FROM Product WHERE IdProduct = @id";
        
        await using SqlConnection connection = new SqlConnection(connectionString);
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        
        await connection.OpenAsync(cancellationToken);
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);
        
        int count = (int)await command.ExecuteScalarAsync(cancellationToken);
        
        return count > 0;
        
    }

    public async Task<Decimal> GetProductPriceByIdAsync(CancellationToken cancellationToken, int id)
    {
        string query = "SELECT Price FROM Product WHERE IdProduct = @id";
        
        await using SqlConnection connection = new SqlConnection(connectionString);
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);
        
        await connection.OpenAsync();

        return  (decimal)await command.ExecuteScalarAsync(cancellationToken);
    }
}