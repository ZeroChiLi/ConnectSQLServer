using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ConnectSQLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("motherfucker!");

            ConnectSQL();

            Console.ReadLine();
        }

        static void ConnectSQL()
        {
            Console.WriteLine("连接数据库");
            //服务器资源管理器->数据连接->数据库右键属性->连接->连接字符串
            string constr = "Data Source=PC-LCL\\SQLEXPRESS;Initial Catalog=TestDb;Integrated Security=True;Pooling=False";
            SqlConnection con = new SqlConnection(constr);
            string sql = "select * from Account";
            SqlCommand com = new SqlCommand(sql, con);
            try
            {
                con.Open();
                Console.WriteLine("成功连接数据库");
                int x = (int)com.ExecuteScalar();
                Console.WriteLine(string.Format("成功读取{0},条记录", x));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            finally
            {
                //con.Close();
                Console.WriteLine("END");
                Console.ReadLine();
            }
        }
       

    }
}
