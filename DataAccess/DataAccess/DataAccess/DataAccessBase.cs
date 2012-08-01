using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace InternationalTalentTest {
     abstract internal class DataAccessBase {
        virtual protected string ConnectionString { get { throw new NotImplementedException(); } }

        protected SqlParameter CreateParameter(string parameterName, SqlDbType dbType, object value) {
            return DoCreateParameter(parameterName, dbType, null, value, null);
        }
        protected SqlParameter CreateParameter(string parameterName, SqlDbType dbType, int size, object value) {
            return DoCreateParameter(parameterName, dbType, size, value, null);
        }
        protected SqlParameter CreateOutputParameter(string parameterName, SqlDbType dbType) {
            return DoCreateParameter(parameterName, dbType, null, null, ParameterDirection.Output);
        }
        protected SqlParameter CreateOutputParameter(string parameterName, SqlDbType dbType, int size) {
            return DoCreateParameter(parameterName, dbType, size, null, ParameterDirection.Output);
        }
        private SqlParameter DoCreateParameter(string parameterName, SqlDbType dbType, int? size, object value, ParameterDirection? parameterDirection) {
            SqlParameter p = new SqlParameter(parameterName, dbType);

            if (size.HasValue) {
                p.Size = (int)size;
            }

            if (parameterDirection.HasValue) {
                p.Direction = (ParameterDirection)parameterDirection;
            }

            p.Value = value ?? (object)DBNull.Value;

            return p;
        }

        protected DataTable ExecuteSelect(string cmdText) {
            return DoExecuteSelect(cmdText, null, CommandType.Text);
        }
        protected DataTable ExecuteSelect(string cmdText, CommandType commandType) {
            return DoExecuteSelect(cmdText, null, commandType);
        }
        protected DataTable ExecuteSelect(string cmdText, SqlParameter parameter) {
            return DoExecuteSelect(cmdText, new SqlParameter[] { parameter }, CommandType.Text);
        }
        protected DataTable ExecuteSelect(string cmdText, SqlParameter parameter, CommandType commandType) {
            return DoExecuteSelect(cmdText, new SqlParameter[] { parameter }, commandType);
        }
        protected DataTable ExecuteSelect(string cmdText, SqlParameter[] parameters) {
            return DoExecuteSelect(cmdText, parameters, CommandType.Text);
        }
        protected DataTable ExecuteSelect(string cmdText, SqlParameter[] parameters, CommandType commandType) {
            return DoExecuteSelect(cmdText, parameters, commandType);
        }
        private DataTable DoExecuteSelect(string cmdText, SqlParameter[] parameters, CommandType commandType) {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                using (SqlCommand cmd = new SqlCommand(cmdText, conn)) {
                    cmd.CommandType = commandType;

                    if (parameters != null) {
                        foreach (SqlParameter parameter in parameters) {
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                        try {
                            conn.Open();
                            da.Fill(dt);
                        }
                        finally {
                            if (conn != null) {
                                conn.Close();
                            }
                        }
                    }
                }
            }

            return dt;
        }

        protected int ExecuteNonQuery(string cmdText) {
            return DoExecuteNonQuery(cmdText, null, CommandType.Text);
        }
        protected int ExecuteNonQuery(string cmdText, CommandType commandType) {
            return DoExecuteNonQuery(cmdText, null, commandType);
        }
        protected int ExecuteNonQuery(string cmdText, SqlParameter parameter) {
            return DoExecuteNonQuery(cmdText, new SqlParameter[] { parameter }, CommandType.Text);
        }
        protected int ExecuteNonQuery(string cmdText, SqlParameter parameter, CommandType commandType) {
            return DoExecuteNonQuery(cmdText, new SqlParameter[] { parameter }, commandType);
        }
        protected int ExecuteNonQuery(string cmdText, SqlParameter[] parameters) {
            return DoExecuteNonQuery(cmdText, parameters, CommandType.Text);
        }
        protected int ExecuteNonQuery(string cmdText, SqlParameter[] parameters, CommandType commandType) {
            return DoExecuteNonQuery(cmdText, parameters, commandType);
        }
        private int DoExecuteNonQuery(string cmdText, SqlParameter[] parameters, CommandType commandType) {
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                using (SqlCommand cmd = new SqlCommand(cmdText, conn)) {
                    cmd.CommandType = commandType;

                    if (parameters != null) {
                        foreach (SqlParameter parameter in parameters) {
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    try {
                        conn.Open();
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    finally {
                        if (conn != null) {
                            conn.Close();
                        }
                    }
                }
            }

            return rowsAffected;
        }
        protected string UpdatedBy { get { return HttpContext.Current.User.Identity.Name; } }
    }
}