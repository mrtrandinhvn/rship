using LegacyOrderService.Exceptions;
using LegacyOrderService.Interfaces;
using LegacyOrderService.Models;
using Microsoft.Data.Sqlite;

namespace LegacyOrderService.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SaveAsync(Order order)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await using var transaction = (SqliteTransaction)await connection.BeginTransactionAsync();

            await using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = @"
                INSERT INTO Orders (CustomerName, ProductName, Quantity, Price)
                VALUES (@customerName, @productName, @quantity, @price);
                SELECT last_insert_rowid();";

            command.Parameters.AddWithValue("@customerName", order.CustomerName);
            command.Parameters.AddWithValue("@productName", order.ProductName);
            command.Parameters.AddWithValue("@quantity", order.Quantity);
            command.Parameters.AddWithValue("@price", order.Price);

            var id = (long?)await command.ExecuteScalarAsync();

            if (id is null or <= 0)
                throw new OrderPersistenceException("Failed to persist order.");

            order.AssignId(id.Value);

            await transaction.CommitAsync();
        }
    }
}
