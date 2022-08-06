using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Messence.Shared;

namespace Messence.DAL
{
    internal class SQLHelper
    {
        private SQLHelper() { }


        // **** START -- SELECT USING STORED PROCEDURE WITH/WITHOUT PARAMETERS (USING SQL DATA ADAPTER)
        internal static DataSet SelectUsingStoredProcedure(string ConnectionString, string spName, List<SqlParameter> spQueryParams)
        {
            DataSet dsSpSelectToReturn = null;

            System.Text.StringBuilder sbSql_Select = new System.Text.StringBuilder();
            sbSql_Select.Append("exec ");

            sbSql_Select.Append(spName);
            for (int i = 0; i < spQueryParams.Count; i++)
            {
                if (i == 0)
                    sbSql_Select.Append(" ");
                else
                    sbSql_Select.Append(",");

                sbSql_Select.Append(spQueryParams[i].ParameterName);
                if (spQueryParams[i].Direction == ParameterDirection.Output)
                    sbSql_Select.Append(" OutPut");
            }

            using (SqlConnection scConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand scCommand = new SqlCommand(sbSql_Select.ToString(), scConnection))
                {
                    // set the parameters
                    for (int i = 0; i < spQueryParams.Count; i++)
                        scCommand.Parameters.Add(spQueryParams[i]);

                    using (SqlDataAdapter sdaAdapt = new SqlDataAdapter())
                    {
                        sdaAdapt.SelectCommand = scCommand;

                        DataSet dsSpSelect = new DataSet();
                        sdaAdapt.Fill(dsSpSelect);

                        dsSpSelectToReturn = dsSpSelect;
                    }
                }
            }

            return dsSpSelectToReturn;
        }

        internal static DataSet SelectUsingStoredProcedure(string ConnectionString, string spName)
        {
            List<SqlParameter> spQueryParams = new List<SqlParameter>();

            return SelectUsingStoredProcedure(ConnectionString, spName, spQueryParams);
        }

        internal static DataSet SelectUsingStoredProcedure_WithDefaultAppConfigConnectionString(string spName, List<SqlParameter> spQueryParams)
        {
            return SelectUsingStoredProcedure(ConfigurationHelper.Instance.MessenceDBConnectionString, spName, spQueryParams);
        }

        internal static DataSet SelectUsingStoredProcedure_WithDefaultAppConfigConnectionString(string spName)
        {
            List<SqlParameter> spQueryParams = new List<SqlParameter>();

            return SelectUsingStoredProcedure(ConfigurationHelper.Instance.MessenceDBConnectionString, spName, spQueryParams);
        }
        // **** END -- SELECT USING STORED PROCEDURE WITH/WITHOUT PARAMETERS (USING SQL DATA ADAPTER)


        // **** START -- EXECUTE STORED PROCEDURE WITH/WITHOUT PARAMETERS FOR AFFECTED ROWS RESULT NUMBER ( USING ExecuteNonQuery() -- Mainly for update/insert and other database operations )
        internal static int ExecuteStoredProcedure_ReturnAffectedRowsNumber(string ConnectionString, string spName, List<SqlParameter> spQueryParams)
        {
            int iRowsAffected = 0;

            System.Text.StringBuilder sbSql_Select = new System.Text.StringBuilder();
            sbSql_Select.Append("exec ");

            sbSql_Select.Append(spName);
            for (int i = 0; i < spQueryParams.Count; i++)
            {
                if (i == 0)
                    sbSql_Select.Append(" ");
                else
                    sbSql_Select.Append(",");

                sbSql_Select.Append(spQueryParams[i].ParameterName);
                if (spQueryParams[i].Direction == ParameterDirection.Output)
                    sbSql_Select.Append(" OutPut");
            }

            using (SqlConnection scConnection = new SqlConnection(ConnectionString))
            {
                scConnection.Open();

                using (SqlCommand scCommand = new SqlCommand(sbSql_Select.ToString(), scConnection))
                {
                    // set the parameters
                    for (int i = 0; i < spQueryParams.Count; i++)
                        scCommand.Parameters.Add(spQueryParams[i]);

                    iRowsAffected = scCommand.ExecuteNonQuery();
                }
            }

            return iRowsAffected;
        }

        internal static int ExecuteStoredProcedure_ReturnAffectedRowsNumber(string ConnectionString, string spName)
        {
            List<SqlParameter> spQueryParams = new List<SqlParameter>();

            return ExecuteStoredProcedure_ReturnAffectedRowsNumber(ConnectionString, spName, spQueryParams);
        }

        internal static int ExecuteStoredProcedure_ReturnAffectedRowsNumber_WithDefaultAppConfigConnectionString(string spName, List<SqlParameter> spQueryParams)
        {
            return ExecuteStoredProcedure_ReturnAffectedRowsNumber(ConfigurationHelper.Instance.MessenceDBConnectionString, spName, spQueryParams);
        }

        internal static int ExecuteStoredProcedure_ReturnAffectedRowsNumber_WithDefaultAppConfigConnectionString(string spName)
        {
            List<SqlParameter> spQueryParams = new List<SqlParameter>();

            return ExecuteStoredProcedure_ReturnAffectedRowsNumber(ConfigurationHelper.Instance.MessenceDBConnectionString, spName, spQueryParams);
        }

        // **** END -- EXECUTE STORED PROCEDURE WITH/WITHOUT PARAMETERS FOR AFFECTED ROWS RESULT NUMBER ( USING ExecuteNonQuery() -- Mainly for update/insert and other database operations )


        internal static object ExecuteStoredProcedure_ReturnDataObjectResult(string ConnectionString, string spName, List<SqlParameter> spQueryParams)
        {
            object spExecutionObjectResultToReturn = null;

            var spDataSetResult = SelectUsingStoredProcedure(ConnectionString, spName, spQueryParams);

            if (spDataSetResult != null && spDataSetResult.Tables.Count > 0 && spDataSetResult.Tables[0].Rows.Count > 0)
            {
                spExecutionObjectResultToReturn = spDataSetResult.Tables[0].Rows[0][0];
            }

            return spExecutionObjectResultToReturn;
        }

        internal static object ExecuteStoredProcedure_ReturnDataObjectResult(string ConnectionString, string spName)
        {
            List<SqlParameter> spQueryParams = new List<SqlParameter>();

            return ExecuteStoredProcedure_ReturnDataObjectResult(ConnectionString, spName, spQueryParams);
        }

        internal static object ExecuteStoredProcedure_ReturnDataObjectResult(string spName, List<SqlParameter> spQueryParams)
        {
            return ExecuteStoredProcedure_ReturnDataObjectResult(ConfigurationHelper.Instance.MessenceDBConnectionString, spName, spQueryParams);
        }

        internal static object ExecuteStoredProcedure_ReturnDataObjectResult(string spName)
        {
            List<SqlParameter> spQueryParams = new List<SqlParameter>();

            return ExecuteStoredProcedure_ReturnDataObjectResult(ConfigurationHelper.Instance.MessenceDBConnectionString, spName, spQueryParams);
        }

    }
}
