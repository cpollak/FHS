using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using YSLProject.DAL;

namespace YSLProject.BAL
{
    public class UserMasterBAL : clsMasterDAL
    {
        public DataTable GetUserList()
        {
            DataTable dt = null;
            try
            {
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "SP_UserMaster";
                Cmd.Parameters.AddWithValue("@Spara", "Select");
                dt = new DataTable();
                dt = Select(Cmd);
            }
            catch (Exception ex) { ex.Message.ToString(); }
            return dt;
        }
    }
}
