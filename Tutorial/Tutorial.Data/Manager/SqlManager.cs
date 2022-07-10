using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using Tutorial.Global.Exceptions;
using Tutorial.Logger;
using Tutorial.Security;

namespace Tutorial.Data
{
    public class SqlManager : ISqlManager
    {
        /// <summary>
        /// Configuration Object
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Sql Connection to Database
        /// </summary>
        private static SqlConnection connection;

        /// <summary>
        /// Constructor: Initialize configuration
        /// </summary>
        /// <param name="configuration"></param>
        public SqlManager(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new SqlConnection(_configuration.GetConnectionString(SqlConstants.ConnetionString));
        }

        /// <summary>
        /// Open Sql Connection
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public async Task<SqlConnection> GetConnection(string connectionString)
        {
            return await Task.FromResult(new SqlConnection(connectionString));
        }

        /// <summary>
        /// Create SQL Command
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public async Task<SqlCommand> CreateCommand(string procedureName, SqlConnection connection)
        {
            SqlCommand sqlCommand = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };


            return await Task.FromResult(sqlCommand);
        }

        #region Public and Protected Methods

        /// <summary>
        /// Execute a stored procedure and return a dataset with the results
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure to call</param>
        /// <param name="paramArray">The procedure parameters</param>
        /// <returns>A new DataSet containing the query results</returns>
        public async Task<DataSet> ExecuteDataset(string procedureName, params SqlParameter[] paramArray)
        {
            DataSet resultSet = new DataSet();
            SqlCommand sqlCommand = null;
            try
            {
                connection.Open();
                sqlCommand = PrepareDatabaseCommand(procedureName, paramArray);

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    dataAdapter.Fill(resultSet);
                }
                return await Task.FromResult(resultSet);
            }
            catch (SqlException sqlEx)
            {
                string message = "Procedure call failed." + sqlEx.Message;
                TutorialLogger.LogError(CustomPrincipal.GetCurrentUserName(), message, sqlEx.StackTrace);

                throw new DataLayerException(sqlEx.Message, (int)HttpStatusCode.InternalServerError, sqlEx);
            }
            finally
            {
                // Dispose command object
                if (sqlCommand != null)
                    sqlCommand.Dispose();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
		/// Execute a stored procedure
		/// </summary>
		/// <param name="procedureName">Name of the stored procedure to call</param>
		/// <param name="paramArray">The procedure parameters</param>
		public async Task<int> ExecuteNonQuery(string procedureName, params SqlParameter[] paramArray)
        {
            SqlCommand sqlCommand = null;
            try
            {
                connection.Open();
                sqlCommand = PrepareDatabaseCommand(procedureName, paramArray);
                SqlParameter returnparam = new SqlParameter("@returnvalue", SqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                sqlCommand.Parameters.Add(returnparam);
                await Task.FromResult(sqlCommand.ExecuteNonQuery());
                var returnValue = sqlCommand.Parameters["@returnvalue"].Value;
                return Convert.ToInt32(returnValue);
            }
            catch (SqlException sqlEx)
            {
                string message = sqlEx.Message;
                TutorialLogger.LogError(CustomPrincipal.GetCurrentUserName(), message, sqlEx.StackTrace);
                throw new DataLayerException(sqlEx.Message, (int)HttpStatusCode.InternalServerError, sqlEx);
            }
            finally
            {
                // Dispose command object
                if (sqlCommand != null)
                    sqlCommand.Dispose();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// Execute a stored procedure and return a scalar value
        /// </summary>
        /// <remarks>The value of the first field in the first row of the first resultset from the query is returned</remarks>
        /// <param name="procedureName">Name of the stored procedure to call</param>
        /// <param name="paramArray">The procedure parameters</param>
        /// <returns>The scalar result value</returns>
        public async Task<object> ExecuteScalar(string procedureName, params SqlParameter[] paramArray)
        {
            SqlCommand sqlCommand = null;
            object result;
            try
            {
                connection.Open();
                sqlCommand = PrepareDatabaseCommand(procedureName, paramArray);
                result = sqlCommand.ExecuteScalar();
                return await Task.FromResult(result);
            }
            catch (SqlException sqlEx)
            {
                string message = sqlEx.Message;
                TutorialLogger.LogError(CustomPrincipal.GetCurrentUserName(), message, sqlEx.StackTrace);
                throw new DataLayerException(sqlEx.Message, (int)HttpStatusCode.InternalServerError, sqlEx);
            }
            finally
            {
                // Dispose command object
                if (sqlCommand != null)
                    sqlCommand.Dispose();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// Execute a stored procedure and return a SqlDataReader to navigate the results
        /// </summary>
        /// <remarks>The value of the first field in the first row of the first resultset from the query is returned</remarks>
        /// <param name="procedureName">Name of the stored procedure to call</param>
        /// <param name="paramArray">The procedure parameters</param>
        /// <returns>A SqlDataReader to read the results</returns>
        public async Task<SqlDataReader> ExecuteReader(string procedureName, params SqlParameter[] paramArray)
        {
            SqlCommand sqlCommand = null;
            try
            {
                connection.Open();
                sqlCommand = PrepareDatabaseCommand(procedureName, paramArray);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                return await Task.FromResult(reader);
            }
            catch (SqlException sqlEx)
            {
                string message = sqlEx.Message;
                TutorialLogger.LogError(CustomPrincipal.GetCurrentUserName(), message, sqlEx.StackTrace);
                throw new DataLayerException(sqlEx.Message, (int)HttpStatusCode.InternalServerError, sqlEx);
            }
            finally
            {
                // Dispose command object
                if (sqlCommand != null)
                    sqlCommand.Dispose();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// Prepare SQL Parameter Name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<SqlParameter> GetParameter(string name, int size, SqlDbType type, ParameterDirection direction)
        {
            SqlParameter sqlParameter = new SqlParameter("@" + name, type, size);
            sqlParameter.Direction = direction;
            return await Task.FromResult(sqlParameter);
        }

        /// <summary>
        /// Prepare SQL Parameter Name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<SqlParameter> GetParameter(string name, object value)
        {
            return await Task.FromResult(new SqlParameter("@" + name, EvaluateNull(value, DBNull.Value)));
        }

        /// <summary>
        /// Prepare SQL Parameter of table-valued type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="SqlType"></param>
        /// <param name="dbTypeName"></param>
        /// <returns></returns>
        public async Task<SqlParameter> GetParameter(string name, object value, SqlDbType SqlType, string dbTypeName)
        {
            SqlParameter sqlParameter = new SqlParameter
            {
                ParameterName = "@" + name,
                SqlDbType = SqlType,
                Value = EvaluateNull(value, DBNull.Value),
                TypeName = dbTypeName
            };
            return await Task.FromResult(sqlParameter);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Prepare DbCommand
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="paramArray"></param>
        /// <returns></returns>
        private static SqlCommand PrepareDatabaseCommand(string procedureName, SqlParameter[] paramArray)
        {
            SqlCommand sqlCommand = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (paramArray != null && paramArray.Length > 0)
            {
                foreach (SqlParameter param in paramArray)
                {
                    sqlCommand.Parameters.Add(param);
                }
            }
            return sqlCommand;
        }

        /// <summary>
        /// Evaluate value before adding to sql parameters
        /// </summary>
        /// <param name="testValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static object EvaluateNull(object testValue, object defaultValue)
        {
            if (testValue == null || testValue is DBNull)
                return defaultValue;
            if (testValue is string @string)
                return string.IsNullOrWhiteSpace(@string) ? defaultValue : testValue;
            if (testValue is int @int)
                return @int == int.MinValue ? defaultValue : testValue;
            if (testValue is DateTime time)
                return EvaluateNull(time, defaultValue);

            return testValue;
        }

        #endregion
    }
}
