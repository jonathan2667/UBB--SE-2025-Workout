using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Core.Data.Interfaces
{
    internal interface IDatabaseHelper
    {
        DataTable ExecuteReader(string commandText, SqlParameter[] parameters);
        int ExecuteNonQuery(string commandText, SqlParameter[] parameters);
        SqlConnection GetConnection();
        void OpenConnection();
        void CloseConnection();
        T? ExecuteScalar<T>(string storedProcedure, SqlParameter[]? sqlParameters = null);
    }
}
