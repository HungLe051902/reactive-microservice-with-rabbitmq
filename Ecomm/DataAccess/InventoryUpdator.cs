using Dapper;
using Microsoft.Data.SqlClient;

namespace Ecomm.DataAccess
{
    public class InventoryUpdator : IInventoryUpdator
    {
        private readonly string connectionString;

        public InventoryUpdator(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task Update(int productId, int quantity)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UPDATE_INVENTORY1", new { productId, quantity });
        }
    }
}
