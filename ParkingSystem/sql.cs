using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;//和SQL相关的命名空间

namespace parking
{
    class Sql
    {
        private string connStr = @"Data Source=LAPTOP-GLILAS4P;Initial Catalog=parking;User Id=sa;Password=123456";
        
        public int Conn(string strsql)//单一操作，返回是（count>0）否（count<=0）成功
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(strsql, conn);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return count;
        }

        public int Comm(string strsql)//单一更改，返回整数从而判断是否修改成功
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand comm = new SqlCommand(strsql, conn);
            conn.Open();
            int count = Convert.ToInt32(comm.ExecuteNonQuery());
            conn.Close();
            return count;
        }
    }
}
