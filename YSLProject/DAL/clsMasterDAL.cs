using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.DAL
{
    public class clsMasterDAL
    {
        private readonly IConfiguration _configuration;

        private string GetConnectionString()
        {
            var configurationBuilder = new ConfigurationBuilder();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            string connstr = configurationBuilder.Build().GetSection("ConnectionStrings:Myconnection").Value;
            //string connstr = System.Configuration.ConfigurationManager.AppSettings["Myconnection"];
            return connstr;
        }

        internal void Insert_Update_Delete(SqlCommand cmd)
        {
            try
            {
                string strConn = GetConnectionString();
                SqlConnection Con = new SqlConnection(strConn);
                cmd.Connection = Con;
                cmd.CommandType = CommandType.StoredProcedure;
                Con.Open();
                cmd.ExecuteNonQuery();
                Con.Close();
            }
            catch (Exception ex) { ex.ToString(); }
        }

        internal int Insert_Update_Del(SqlCommand cmd)
        {
            int id = 0;
            try
            {
                string strConn = GetConnectionString();
                SqlConnection Con = new SqlConnection(strConn);
                cmd.Connection = Con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ids", 0).Direction = ParameterDirection.Output;
                Con.Open();
                cmd.ExecuteNonQuery();
                id = Convert.ToInt32(cmd.Parameters["@Ids"].Value.ToString());
                Con.Close();

            }
            catch (Exception ex) { ex.ToString(); }
            return id;
        }

        internal DataTable Select(SqlCommand cmd)
        {
            DataTable dt = null;
            try
            {
                string strConn = GetConnectionString();
                SqlConnection conn = new SqlConnection(strConn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                dt = new DataTable();
                SqlDataReader dr;
                conn.Open();
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                conn.Close();
            }
            catch (Exception ex) { ex.ToString(); }
            return dt;
        }

        internal DataSet SelectBy(SqlCommand cmd)
        {
            DataSet dt = null;
            try
            {
                string strConn = GetConnectionString();
                SqlConnection conn = new SqlConnection(strConn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                dt = new DataSet();
                conn.Open();
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();
            }
            catch (Exception ex) { ex.ToString(); }
            return dt;
        }

        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row => {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList();
        }
    }
}
