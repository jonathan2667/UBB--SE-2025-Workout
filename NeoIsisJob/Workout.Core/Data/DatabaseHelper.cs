using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Workout.Core.Data.Interfaces;

namespace Workout.Core.Data
{
    internal class DatabaseHelper : IDatabaseHelper
    {
        private readonly string connectionString;
        private SqlConnection sqlConnection;

        public DatabaseHelper()
        {
            // connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Workout;Integrated Security=True;";
            connectionString = @"Server=localhost;Database=Workout;Integrated Security=True;TrustServerCertificate=True;";

            try
            {
                this.sqlConnection = new SqlConnection(this.connectionString);
            }
            catch (Exception exception)
            {
                throw new Exception($"Error initializing SQL connection: {this.connectionString}", exception);
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (this.sqlConnection.State != ConnectionState.Open)
            {
                this.sqlConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (this.sqlConnection.State != ConnectionState.Closed)
            {
                this.sqlConnection.Close();
            }
        }

        // Explicitly implement the interface methods
        public DataTable ExecuteReader(string commandText, SqlParameter[] parameters)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(commandText, sqlConnection))
                {
                    command.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteReader: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

        public int ExecuteNonQuery(string commandText, SqlParameter[] parameters)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(commandText, sqlConnection))
                {
                    command.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteNonQuery: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

        // Keep this if needed by other parts of the app
        public T? ExecuteScalar<T>(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(storedProcedure, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (sqlParameters != null)
                    {
                        command.Parameters.AddRange(sqlParameters);
                    }

                    var result = command.ExecuteScalar();
                    return (result == null || result == DBNull.Value)
                        ? default
                        : (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteScalar: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
