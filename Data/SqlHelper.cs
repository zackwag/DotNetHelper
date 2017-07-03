using System;
using System.Data;
using System.Data.SqlClient;
using Helper.Extensions;
using static System.String;

namespace Helper.Data
{
    /// <summary>
    /// This class provides helper functions that assist in basic SQL tasks.
    /// </summary>
    public static class SqlHelper
    {
        #region PUBLIC METHODS

        #region EXECUTE DATA SET

        /// <summary>
        /// This method will execute a SQL query and return a SQL dataset of the results-based query.
        /// </summary>
        /// <param name="command">SQL Command object</param>
        /// <param name="connectionString">Database connection string</param>
        /// <returns>SQL Dataset</returns>
        public static DataSet ExecuteDataSet(SqlCommand command, string connectionString)
        {
            SqlConnection connection = null;

            try
            {
                // CREATE CONNECTION OBJECT
                using (connection = new SqlConnection(connectionString))
                {
                    // OPEN CONNECTION
                    connection.Open();

                    // ASSIGN CONNECTION TO COMMAND OBJECT
                    command.Connection = connection;

                    // START THE TRANSACTION
                    command.Transaction = connection.BeginTransaction();

                    // CREATE THE DATASET OBJECT
                    var dsReturn = new DataSet();

                    // FILL THE DATASET
                    using (var adpResults = new SqlDataAdapter(command))
                    {
                        adpResults.Fill(dsReturn);
                    }

                    // COMMITE THE TRANSACTION
                    command.Transaction.Commit();

                    // FUNCTION RETURN
                    return dsReturn;
                }
            }
            catch (Exception)
            {
                // ROLLBACK THE TRANSACTION ON ERROR
                if (!connection.IsNull() && command.HasTransaction())
                    command.Transaction.Rollback();

                // RETHROW THE EXCEPTION TO THE CALLER
                throw;
            }
            finally
            {
                // CLEANUP ...
                if (!connection.IsNull())
                {
                    if (!connection.IsClosed())
                        connection.Close();

                    connection.Dispose();
                }
            }
        }

        #endregion EXECUTE DATA SET

        #region EXECUTE DATA TABLE

        public static DataTable ExecuteDataTable(SqlCommand command, string connectionString)
        {
            SqlConnection connection = null;

            try
            {
                // CREATE CONNECTION OBJECT
                using (connection = new SqlConnection(connectionString))
                {
                    // OPEN CONNECTION
                    connection.Open();

                    // ASSIGN CONNECTION TO COMMAND OBJECT
                    command.Connection = connection;

                    // START THE TRANSACTION
                    command.Transaction = connection.BeginTransaction();

                    // CREATE THE DATASET OBJECT
                    var dtReturn = new DataTable();

                    // FILL THE DATASET
                    using (var adpResults = new SqlDataAdapter(command))
                    {
                        adpResults.Fill(dtReturn);
                    }

                    // COMMITE THE TRANSACTION
                    command.Transaction.Commit();

                    // FUNCTION RETURN
                    return dtReturn;
                }
            }
            catch (Exception)
            {
                // ROLLBACK THE TRANSACTION ON ERROR
                if (!connection.IsNull() && command.HasTransaction())
                    command.Transaction.Rollback();

                // RETHROW THE EXCEPTION TO THE CALLER
                throw;
            }
            finally
            {
                // CLEANUP ...
                if (!connection.IsNull())
                {
                    if (!connection.IsClosed())
                        connection.Close();

                    connection.Dispose();
                }
            }
        }

        #endregion EXECUTE DATA SET

        #region EXECUTE NON QUERY

        /// <summary>
        /// This method will execute a SQL query and return the number of rows affected by the query.
        /// </summary>
        /// <param name="command">SQL Command object</param>
        /// <param name="connectionString">Database connection string</param>
        /// <returns>Integer nubmer of rows affected</returns>
        public static int ExecuteNonQuery(SqlCommand command, string connectionString)
        {
            SqlConnection connection = null;

            try
            {
                // CREATE CONNECTION OBJECT
                using (connection = new SqlConnection(connectionString))
                {

                    // OPEN CONNECTION
                    connection.Open();

                    // ASSIGN CONNECTION TO COMMAND OBJECT
                    command.Connection = connection;

                    // START THE TRANSACTION
                    command.Transaction = connection.BeginTransaction();

                    // GET COUNT OF RECORDS AFFECTED BY QUERY
                    var recordsAffected = command.ExecuteNonQuery();

                    // COMMIT THE TRANSACTION
                    command.Transaction.Commit();

                    // FUNCTION RETURN
                    return recordsAffected;
                }
            }
            catch (Exception)
            {
                // ROLLBACK THE TRANSACTION ON ERROR
                if (!connection.IsNull() && command.HasTransaction())
                    command.Transaction.Rollback();

                // RETHROW THE EXCEPTION TO THE CALLER
                throw;
            }
            finally
            {
                // CLEANUP ...
                if (!connection.IsNull())
                {
                    if (!connection.IsClosed())
                        connection.Close();

                    connection.Dispose();
                }
            }
        }

        #endregion EXECUTE NON QUERY

        #region EXECUTE SCALAR

        /// <summary>
        /// This method will execute an SQL query and return an object containing the data contained in the 1st cell of the 1st row in the result set
        /// </summary>
        /// <param name="command">SQL Command object</param>
        /// <param name="connectionString">Database connection string</param>
        /// <returns>Object containing the data contained in the 1st cell of the 1st row in the result set</returns>
        public static object ExecuteScalar(SqlCommand command, string connectionString)
        {
            SqlConnection connection = null;

            try
            {
                // CREATE CONNECTION OBJECT
                using (connection = new SqlConnection(connectionString))
                {
                    // OPEN CONNECTION
                    connection.Open();

                    // ASSIGN CONNECTION TO COMMAND OBJECT
                    command.Connection = connection;

                    // START THE TRANSACTION
                    command.Transaction = connection.BeginTransaction();

                    // GET THE VALUE RETURNED BY THE QUERY
                    var scalarObject = command.ExecuteScalar();

                    // COMMIT THE TRANSACTION
                    command.Transaction.Commit();

                    // FUNCTION RETURN
                    return scalarObject;
                }
            }
            catch (Exception)
            {
                // ROLLBACK THE TRANSACTION ON ERROR
                if (!connection.IsNull() && command.HasTransaction())
                    command.Transaction.Rollback();

                // RETHROW THE EXCEPTION TO THE CALLER
                throw;
            }
            finally
            {
                // CLEANUP ...
                if (!connection.IsNull())
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                    connection.Dispose();
                }
            }
        }

        #endregion EXECUTE SCALAR

        #region EXECUTE DATA READER

        /// <summary>
        /// This method will execute a SQL query and return a SQL Data Reader of the results-based query.
        /// </summary>
        /// <param name="command">SQL Command object</param>
        /// <param name="connectionString">Database connection string</param>
        /// <returns>SQL DataReader</returns>
        public static SqlDataReader ExecuteDataReader(SqlCommand command, string connectionString)
        {
            SqlConnection connection = null;

            try
            {
                // CREATE CONNECTION OBJECT
                using (connection = new SqlConnection(connectionString))
                {

                    // OPEN CONNECTION
                    connection.Open();

                    // ASSIGN CONNECTION TO COMMAND OBJECT
                    command.Connection = connection;

                    var reader = command.ExecuteReader();

                    // FUNCTION RETURN
                    return reader;
                }
            }
            catch (Exception)
            {
                // CLEANUP ...
                if (!connection.IsNull())
                {
                    if (!connection.IsClosed())
                        connection.Close();

                    connection.Dispose();
                }

                // RETHROW THE EXCEPTION TO THE CALLER
                throw;
            }
        }

        #endregion EXECUTE DATA READER

        #region CREATE PARAMETER

        /// <summary>
        /// This method creates and returns a SQL parameter object based on the given parameters.
        /// </summary>
        /// <param name="parameterName">Name of Parameter</param>
        /// <param name="value">Value of Parameter</param>
        /// <param name="dataType">Data Type of Parameter</param>
        /// <param name="direction">Direction of Parameter</param>
        /// <returns>Newly created SQL parameter</returns>
        public static SqlParameter CreateParameter(string parameterName, object value, DbType dataType, ParameterDirection direction)
        {
            return new SqlParameter
            {
                ParameterName = parameterName,
                Value = value,
                Direction = direction,
                DbType = dataType
            };
        }

        public static SqlParameter CreateParameter(string parameterName, object value, SqlDbType dataType, ParameterDirection direction)
        {
            return new SqlParameter
            {
                ParameterName = parameterName,
                Value = value,
                Direction = direction,
                SqlDbType = dataType
            };
        }

        /// <summary>
        /// This method creates and returns a SQL parameter object based on the given parameters.
        /// </summary>
        /// <param name="parameterName">Name of Parameter</param>
        /// <param name="value">Value of Parameter</param>
        /// <param name="size">Size of Parameter (in bytes)</param>
        /// <param name="dataType">Data Type of Parameter</param>
        /// <param name="direction">Direction of Parameter</param>
        /// <returns>Newly created SQL parameter</returns>
        public static SqlParameter CreateParameter(string parameterName, object value, int size, DbType dataType, ParameterDirection direction)
        {
            return new SqlParameter
            {
                ParameterName = parameterName,
                Size = size,
                DbType = dataType,
                Value = value,
                Direction = direction
            };
        }

        public static SqlParameter CreateParameter(string parameterName, object value, int size, SqlDbType dataType, ParameterDirection direction)
        {
            return new SqlParameter
            {
                ParameterName = parameterName,
                Size = size,
                SqlDbType = dataType,
                Value = value,
                Direction = direction
            };
        }

        #endregion CREATE PARAMETER

        /// <summary>
        /// Converts a variable into a DB friendly object.
        /// </summary>
        /// <param name="value">An Int32, String or DateTime</param>
        /// <returns>A DBNULL if the Int == -1, the String == String.Empty or Datetime == DateTime.MinValue</returns>
        public static object Parameterize<T>(T value)
        {
            object o = DBNull.Value;

            if (value.IsNull()) return o;

            if (value.GetType().ToString() == "System.Int32")
            {
                if (Convert.ToInt32(value) != -1)
                    o = value;
            }
            else if (value.GetType().ToString() == "System.String")
            {
                if (!IsNullOrEmpty(value.ToString()))
                    o = value;
            }
            else if (value.GetType().ToString() == "System.DateTime")
            {
                if (Convert.ToDateTime(value) != DateTime.MinValue)
                    o = value;
            }

            return o;
        }

        public static DateTime SqlMinDate => new DateTime(1900, 1, 1);

        public static string BuildUpdateString(string fieldName, string currentValue, string newValue, string updateStr)
        {
            if (newValue != currentValue)
            {
                updateStr += IsNullOrEmpty(updateStr) ? " SET " : ", ";

                //CONVERT SINGLE QUOTE TO DOUBLE QUOTE LITERAL
                updateStr += $"{fieldName} = '{newValue.Replace("'", "''")}'";
            }

            return updateStr;
        }

        public static DataTable GetDynamicSql(string connectionString, string sqlStmt)
        {
            try
            {
                var cmd = new SqlCommand
                {
                    CommandText = sqlStmt,
                    CommandTimeout = 120
                };

                return ExecuteDataTable(cmd, connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetDynamicSql method, message: " + ex.Message);
            }
        }

        public static void ExecuteDynamicSql(string connectionString, string sqlStmt)
        {
            try
            {
                var cmd = new SqlCommand
                {
                    CommandText = sqlStmt,
                    CommandTimeout = 120
                };

                ExecuteNonQuery(cmd, connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception("error on ExecuteDynamicSql method, message: " + ex.Message);
            }
        }

        #endregion PUBLIC METHODS
    }
}
