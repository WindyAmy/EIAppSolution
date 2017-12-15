using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ORM
{
    #region DBHelper接口

    /// <summary>
    /// DBHelper接口111
    /// </summary>
    public interface IDBHelper
    {
        string ConnString { set; }

        DataTable ExecuteDataTable(string sql, SqlParameter[] parameters);

        int ExecuteNonQuery(string sql, SqlParameter[] sqlParams);

        Object ExecuteScalar(string sql, SqlParameter[] parameters);

        bool ExecuteTransaction(Dictionary<string, SqlParameter[]> sqlAndPara, bool earlyTermination);
    }

    #endregion DBHelper接口
}