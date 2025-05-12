using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Workout.Core.Data.Interfaces
{
    public interface IDatabaseHelper
    {
        Task<DataTable> ExecuteReaderAsync(string commandText, SqlParameter[] parameters);
        Task<int> ExecuteNonQueryAsync(string commandText, SqlParameter[] parameters);
#pragma warning disable SA1011   // allow no-space after ]
        Task<T?> ExecuteScalarAsync<T>(string storedProcedure, SqlParameter[]? sqlParameters = null);
#pragma warning restore SA1011
        SqlConnection GetConnection();
        void OpenConnection();
        void CloseConnection();
    }
}
