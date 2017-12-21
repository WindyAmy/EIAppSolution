using SAP.Middleware.Connector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.Common
{
    public class SapHelper
    {
        #region 单例模式

        private static SapHelper _SapHelper = null;

        private SapHelper()
        {
        }

        public static SapHelper GetInstance()
        {
            if (_SapHelper == null)
            {
                _SapHelper = new SapHelper();
            }
            return _SapHelper;
        }

        #endregion 单例模式

        #region 私有方法

        #region 连接及1调用SAP方法

        /// <summary>
        /// 初始化SAP 连接参数
        /// </summary>
        /// <returns></returns>
        private RfcConfigParameters GetConfigParams()
        {
            RfcConfigParameters configParams = new RfcConfigParameters();
            Dictionary<string, string> dic = new Dictionary<string, string>(); //Common.Config.SapConnections;

            configParams.Add(RfcConfigParameters.Name, dic["Name"]);
            configParams.Add(RfcConfigParameters.AppServerHost, dic["AppServerHost"]);
            configParams.Add(RfcConfigParameters.SystemNumber, dic["SystemNumber"]); // instance number
            //configParams.Add(RfcConfigParameters.SystemID, "D01");

            configParams.Add(RfcConfigParameters.User, dic["User"]);
            configParams.Add(RfcConfigParameters.Password, dic["Password"]);
            configParams.Add(RfcConfigParameters.Client, dic["Client"]);
            configParams.Add(RfcConfigParameters.Language, dic["Language"]);
            //configParams.Add(RfcConfigParameters.PoolSize, "5");
            //configParams.Add(RfcConfigParameters.MaxPoolSize, "10");
            //configParams.Add(RfcConfigParameters.IdleTimeout, "30");

            return configParams;
        }

        /// <summary>
        /// 获取RfcDestination
        /// </summary>
        /// <returns></returns>
        private RfcDestination GetDestination()
        {
            RfcConfigParameters configParams = this.GetConfigParams();
            RfcDestination dest = RfcDestinationManager.GetDestination(configParams);

            return dest;
        }

        /// <summary>
        /// 获取调用RFC方法
        /// </summary>
        /// <param name="fmName"></param>
        /// <returns></returns>
        private IRfcFunction CallRfc(string fmName)
        {
            RfcDestination destination = this.GetDestination();
            IRfcFunction fm = destination.Repository.CreateFunction(fmName);
            return fm;
        }

        private IRfcFunction InvokeRFC(string fmName, Hashtable hsTable)
        {
            IRfcFunction fm = CallRfc(fmName);
            if (hsTable != null)
            {
                foreach (string key in hsTable.Keys)
                {
                    //fm.SetValue(key, hsTable[key]);//设置Import的参数
                    //如果传入的参数是DataTable
                    if (hsTable[key].GetType() == typeof(DataTable))
                    {
                        DataTable tmpDataTable = hsTable[key] as DataTable;
                        IRfcTable itb = fm.GetTable(key);
                        foreach (DataRow dr in tmpDataTable.Rows)
                        {
                            itb.Insert();
                            for (int i = 0; i < tmpDataTable.Columns.Count; i++)
                            {
                                itb.CurrentRow.SetValue(tmpDataTable.Columns[i].ColumnName, dr[tmpDataTable.Columns[i].ColumnName].ToString());
                            }
                        }

                        fm.SetValue(key, itb);//设置Import的参数
                    }
                    else
                    {
                        fm.SetValue(key, hsTable[key]);//设置Import的参数
                    }
                }
            }
            //提交调用BAPI
            fm.Invoke(this.GetDestination());
            return fm;
        }

        #endregion 连接及调用SAP方法

        #region SAP内表转换成DataTable

        /// <summary>
        /// SAP内表转换成DataTable
        /// </summary>
        /// <param name="itab"></param>
        /// <returns></returns>
        private DataTable ToDataTable(IRfcTable itab)
        {
            // purpose: convert IRfcTable to DataTable

            DataTable dataTable = new DataTable();

            // dataTable column definition
            for (int i = 0; i < itab.ElementCount; i++)
            {
                RfcElementMetadata metadata = itab.GetElementMetadata(i);
                dataTable.Columns.Add(metadata.Name);
            }

            // line items
            for (int rowIdx = 0; rowIdx < itab.RowCount; rowIdx++)
            {
                DataRow dRow = dataTable.NewRow();

                // each line is a structure
                for (int idx = 0; idx < itab.ElementCount; idx++)
                {
                    dRow[idx] = itab[rowIdx].GetString(idx);//全部转换成string类型
                }

                dataTable.Rows.Add(dRow);
            }

            return dataTable;
        }

        #endregion SAP内表转换成DataTable

        #endregion 私有方法

        #region 测试SAP连接

        /// <summary>
        /// 测试SAP连接
        /// </summary>
        public void PingDestination()
        {
            RfcDestination destination = this.GetDestination();
            destination.Ping();
        }

        #endregion 测试SAP连接

        #region call rfc 得到Sap内表并转换成DataSet/DataTable

        /// <summary>
        /// call rfc 得到DataSet
        /// </summary>
        /// <param name="fmName">rfc调用方法名称</param>
        /// <param name="hsTable">传入参数</param>
        /// <param name="tableNames">返回内表名称list</param>
        /// <returns></returns>
        public DataSet CallRfcForTables(string fmName, Hashtable hsTable, List<string> tableNames)
        {
            DataSet ds = new DataSet();
            IRfcFunction fm = InvokeRFC(fmName, hsTable);
            foreach (string tName in tableNames)
            {
                IRfcTable itab = fm.GetTable(tName);
                if (itab != null)
                {
                    DataTable dt = ToDataTable(itab);
                    dt.TableName = tName;
                    ds.Tables.Add(dt);
                }
            }

            return ds;
        }

        /// <summary>
        ///  call rfc 得到DataTable
        /// </summary>
        /// <param name="fmName">rfc调用方法名称</param>
        /// <param name="hsTable">传入参数</param>
        /// <param name="tableName">内表名称</param>
        /// <returns></returns>
        public DataTable CallRfcForTable(string fmName, Hashtable hsTable, string tableName)
        {
            DataTable dt = null;
            DataSet ds = CallRfcForTables(fmName, hsTable, new List<string>() { tableName });
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                dt.TableName = tableName;
            }
            return dt;
        }

        #endregion call rfc 得到Sap内表并转换成DataSet/DataTable

        #region call rfc 得到Sap单返回值并转换成Dictionary/string

        /// <summary>
        ///  call rfc 得到Sap单返回值并转换成Dictionary
        /// </summary>
        /// <param name="fmName">rfc调用方法名称</param>
        /// <param name="hsTable">传入参数</param>
        /// <param name="valueNames"></param>
        /// <returns></returns>
        public Dictionary<string, string> CallRfcForValue(string fmName, Hashtable hsTable, List<string> valueNames)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            IRfcFunction fm = InvokeRFC(fmName, hsTable);
            foreach (string vName in valueNames)
            {
                string tmpValue = fm.GetString(vName);
                if (!string.IsNullOrEmpty(tmpValue))
                {
                    dic.Add(vName, tmpValue);
                }
            }

            return dic;
        }

        /// <summary>
        ///   call rfc 得到Sap单返回值并转换成string
        /// </summary>
        /// <param name="fmName">rfc调用方法名称</param>
        /// <param name="hsTable">传入参数</param>
        /// <param name="valueName"></param>
        /// <returns></returns>
        public string CallRfcForValue(string fmName, Hashtable hsTable, string valueName)
        {
            string singleValue = string.Empty;
            Dictionary<string, string> dic = CallRfcForValue(fmName, hsTable, new List<string>() { valueName });
            if (dic != null && dic.Count > 0)
            {
                singleValue = dic[valueName];
            }
            return singleValue;
        }

        #endregion call rfc 得到Sap单返回值并转换成Dictionary/string

        #region call rfc 得到Sap 结构体并转换成Dictionary

        /// <summary>
        ///  #region call rfc 得到Sap 结构体并转换成Dictionary
        /// </summary>
        /// <param name="fmName">rfc调用方法名称</param>
        /// <param name="hsTable">传入参数</param>
        /// <param name="structName">结构体名称</param>
        /// <returns></returns>
        public Dictionary<string, string> CallRfcForStructure(string fmName, Hashtable hsTable, string structName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            IRfcFunction fm = InvokeRFC(fmName, hsTable);
            IRfcStructure tmpStruct = fm.GetStructure(structName);
            if (tmpStruct != null)
            {
                for (int i = 0; i < tmpStruct.ElementCount; i++)
                {
                    // get column name from position
                    RfcElementMetadata colMeta = tmpStruct.GetElementMetadata(i);
                    dic.Add(colMeta.Name, tmpStruct.GetString(colMeta.Name));//get value from column name
                }
            }

            return dic;
        }

        #endregion call rfc 得到Sap 结构体并转换成Dictionary
    }

    public static class ExtMethod
    {
        public static List<T> SapDTableToModel<T>(this DataTable table) where T : class,new()
        {
            List<T> entities = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                var arrayProperties = entity.GetType().GetProperties();
                foreach (var item in arrayProperties)
                {
                    string sapColName = string.Empty;
                    var arrayObject = item.GetCustomAttributes(typeof(MatchSapNameAttribute), false);
                    if (arrayObject != null && arrayObject.Length > 0)
                    {
                        MatchSapNameAttribute attr = arrayObject[0] as MatchSapNameAttribute;
                        if (attr != null)
                        {
                            sapColName = attr.SapColName;
                        }
                    }
                    if (row.Table.Columns.Contains(sapColName))
                    {
                        if (DBNull.Value != row[sapColName])
                        {
                            item.SetValue(entity, row[sapColName], null);
                        }
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }
    }

    public class MatchSapNameAttribute : Attribute
    {
        public string SapColName { get; set; }

        public MatchSapNameAttribute(string sapname)
        {
            SapColName = sapname;
        }
    }
}