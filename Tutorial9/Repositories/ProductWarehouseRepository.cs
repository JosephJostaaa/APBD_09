using Microsoft.Data.SqlClient;

namespace Tutorial9.Repositories;

public class ProductWarehouseRepository : IProductWarehouseRepository
{
    private readonly string _connectionString;
    
    public ProductWarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }
    
    public async Task<bool> ExistsByOrderIdAsync(CancellationToken cancellationToken, int id)
    {
        string query = "SELECT COUNT(*) FROM Product_Warehouse WHERE IdOrder = @id";
        using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.CommandText = query;
                command.Connection = sqlConnection;
                command.Parameters.AddWithValue("@id", id);
                
                sqlConnection.Open();
                var result = await command.ExecuteScalarAsync(cancellationToken);
                return (int) result > 0;
            }
        }
    }
}