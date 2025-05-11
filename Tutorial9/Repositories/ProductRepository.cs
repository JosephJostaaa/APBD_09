using Microsoft.Data.SqlClient;

namespace Tutorial9.Repositories;

public class ProductRepository
{
    
    private readonly string connectionString;
    
    public ProductRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection");
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

    public async Task<double> GetProductPriceByIdAsync(CancellationToken cancellationToken, int id)
    {
        string query = "SELECT Price FROM Product WHERE IdProduct = @id";
        
        await using SqlConnection connection = new SqlConnection(connectionString);
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);
        
        await connection.OpenAsync();

        return  (double)await command.ExecuteScalarAsync(cancellationToken);
    }
}