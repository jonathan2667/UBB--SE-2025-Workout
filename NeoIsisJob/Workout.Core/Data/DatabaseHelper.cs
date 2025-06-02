using System.Data;
using System.Data.SqlClient;
using Workout.Core.Data.Interfaces;

namespace Workout.Core.Data
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly string connectionString;
        private SqlConnection sqlConnection;

        public DatabaseHelper()
        {
            connectionString = @"Server=WIN-IVAPD6T4EJF\MSSQLSERVER01;Database=Workout;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
            //   connectionString = @"Server=WIN-IVAPD6T4EJF\MSSQLSERVER01;Database=Workout;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
            // connectionString = @"Data Source=DESKTOP-DCVLDLM\SQLEXPRESS;Initial Catalog=Workout;Integrated Security=True;TrustServerCertificate=True";
            sqlConnection = new SqlConnection(connectionString);
        }

        public SqlConnection GetConnection() => new SqlConnection(connectionString);

        public void OpenConnection()
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close();
            }
        }
        public async Task<DataTable> ExecuteReaderAsync(string commandText, SqlParameter[] parameters)
        {
            // Create and open a brand‐new connection for this call
            await using var conn = new SqlConnection(this.connectionString);
            await conn.OpenAsync();

            // Build the command on that connection
            await using var cmd = new SqlCommand(commandText, conn)
            {
                CommandType = CommandType.Text
            };
            if (parameters is not null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            // Execute and load into a DataTable
            await using var reader = await cmd.ExecuteReaderAsync();
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }
        public async Task<int> ExecuteNonQueryAsync(string commandText, SqlParameter[] parameters)
        {
            try
            {
                OpenConnection();
                using (var command = new SqlCommand(commandText, sqlConnection))
                {
                    command.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteNonQueryAsync: {ex.Message}", ex);
            }
            finally
            {
                CloseConnection();
            }
        }

        public async Task<T?> ExecuteScalarAsync<T>(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                OpenConnection();
                using (var command = new SqlCommand(storedProcedure, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (sqlParameters != null)
                    {
                        command.Parameters.AddRange(sqlParameters);
                    }

                    var result = await command.ExecuteScalarAsync();
                    return (result == null || result == DBNull.Value)
                        ? default
                        : (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteScalarAsync: {ex.Message}", ex);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
