using Microsoft.Data.SqlClient;

namespace Tutorial9.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly string connectionString;
    
    public WarehouseRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("Default");
    }

    public async Task<bool> ExistsByIdAsync(CancellationToken cancellationToken, int id)
    {
        string query = "SELECT COUNT(*) FROM Warehouse WHERE IdWarehouse = @id";
        
        await using SqlConnection connection = new SqlConnection(connectionString);
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        
        await connection.OpenAsync(cancellationToken);
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);
        
        int count = (int)await command.ExecuteScalarAsync(cancellationToken);
        
        return count > 0;
        
    }
}