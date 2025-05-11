using System.Data.Common;
using Microsoft.Data.SqlClient;
using Tutorial9.Dto;
using Tutorial9.Model;

namespace Tutorial9.Repositories;

public class OrderRepository
{
    
    private readonly string _connectionString;
    
    public OrderRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<List<Order>> GetOrdersAsync(CancellationToken ct, SimpleOrderFilter filter)
    {

        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        await connection.OpenAsync(ct);

        command.CommandText = @"SELECT * FROM Order 
                                WHERE 1 = 1";

        if (filter.CreatedAt != null)
        {
            command.CommandText += " AND CreatedAt < @CreatedAt";
            command.Parameters.AddWithValue("@CreatedAt", filter.CreatedAt);
        }

        if (filter.IdOrder != null)
        {
            command.CommandText += " AND IdOrder = @IdOrder";
            command.Parameters.AddWithValue("@IdOrder", filter.IdOrder);
        }

        if (filter.Amount != null)
        {
            command.CommandText += " AND Amount = @Amount";
            command.Parameters.AddWithValue("@Amount", filter.Amount);
        }

        List<Order> orders = new List<Order>();

        await using SqlDataReader reader = await command.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            orders.Add(new Order
            {
                IdOrder = reader.GetInt32(reader.GetOrdinal("IdOrder")),
                IdProduct = reader.GetInt32(reader.GetOrdinal("IdProduct")),
                Amount = reader.GetInt32(reader.GetOrdinal("Amount")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                FulfilledAt = reader.IsDBNull(reader.GetOrdinal("FulfilledAt"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("FulfilledAt"))
            });
        }

        return orders;
    }
    
    public async Task<int> FullfillOrderAsync(CancellationToken ct, ProductWarehouseRecord productWarehouseRecord)
    {
        string query = "UPDATE Order SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder";
        
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);
        
        DbTransaction transaction = await connection.BeginTransactionAsync();

        try
        {
            await using SqlCommand command = new SqlCommand();
            command.Transaction = transaction as SqlTransaction;

            command.Connection = connection;

            command.CommandText = query;
            command.Parameters.AddWithValue("@IdOrder", productWarehouseRecord.IdOrder);
            command.Parameters.AddWithValue("@FulfilledAt", DateTime.Now);

            int rowsAffected = await command.ExecuteNonQueryAsync(ct);
            
            if (rowsAffected == 0)
            {
                throw new Exception("Order not found");
            }
            
            command.Parameters.Clear();
            
            command.CommandText = @"INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
                                    OUTPUT INSERTED.IdProductWarehouse
                                    VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";
            
            command.Parameters.AddWithValue("@IdWarehouse", productWarehouseRecord.IdWarehouse);
            command.Parameters.AddWithValue("@IdProduct", productWarehouseRecord.IdProduct);
            command.Parameters.AddWithValue("@IdOrder", productWarehouseRecord.IdOrder);
            command.Parameters.AddWithValue("@Amount", productWarehouseRecord.Amount);
            command.Parameters.AddWithValue("@Price", productWarehouseRecord.Price);
            command.Parameters.AddWithValue("@CreatedAt", productWarehouseRecord.CreatedAt);
            
            int idProductWarehouse = (int)await command.ExecuteScalarAsync(ct);
            
            return idProductWarehouse;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}