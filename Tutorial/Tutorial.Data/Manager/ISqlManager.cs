using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Tutorial.Data
{
    public interface ISqlManager
    {
        /// <summary>
        /// Open Sql Connection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        Task<SqlConnection> GetConnection(string connectionString);

        /// <summary>
        /// Create SQL Command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        Task<SqlCommand> CreateCommand(string procedureName, SqlConnection connection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="paramArray"></param>
        /// <returns></returns>
        Task<DataSet> ExecuteDataset(string procedureName, params SqlParameter[] paramArray);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="paramArray"></param>
        /// <returns></returns>
        Task<int> ExecuteNonQuery(string procedureName, params SqlParameter[] paramArray);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="paramArray"></param>
        /// <returns></returns>
        Task<object> ExecuteScalar(string procedureName, params SqlParameter[] paramArray);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="paramArray"></param>
        /// <returns></returns>
        Task<SqlDataReader> ExecuteReader(string procedureName, params SqlParameter[] paramArray);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="type"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        Task<SqlParameter> GetParameter(string name, int size, SqlDbType type, ParameterDirection direction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<SqlParameter> GetParameter(string name, object value);

        /// <summary>
        /// Prepare SQL Parameter of table-valued type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="SqlType"></param>
        /// <param name="dbTypeName"></param>
        /// <returns></returns>
        Task<SqlParameter> GetParameter(string name, object value, SqlDbType SqlType, string dbTypeName);
    }
}
