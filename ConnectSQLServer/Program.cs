using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ConnectSQLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection sqlConnection;
            ConnectSQL(out sqlConnection);

            string tableName = "Account";

            //ReaderSQLData(sqlConnection);

            SqlDataAdapter dataAdapter = GetDataAdapter(sqlConnection, tableName);
            DataSet dataSet = FillAdapterWithDataSet(ref dataAdapter, tableName);

            //ReaderSQLData2(dataSet, tableName);

            //ModifySQLDataByRow<string>(dataAdapter, dataSet, tableName, "Name", "23333");
            //ReaderSQLData2(dataSet, tableName);

            //AddSQLData(dataAdapter, dataSet, tableName, "KINGDOM", "fucker", 666);
            //ReaderSQLData2(dataSet, tableName);

            //RemoveSQLDataByRow(dataAdapter, dataSet, tableName, 1);
            //ReaderSQLData2(dataSet, tableName);

            //ReaderSQLData(sqlConnection, "777", "123");
            //AddSQLData2(sqlConnection, "sdaiofajsoi", "12333");
            //RemoveSQLData2(sqlConnection, "123");


            //dataSet.Dispose();        // 释放DataSet对象
            //dataAdapter.Dispose();    // 释放SqlDataAdapter对象
            ////myDataReader.Dispose();     // 释放SqlDataReader对象
            //sqlConnection.Close();             // 关闭数据库连接
            //sqlConnection.Dispose();           // 释放数据库连接对象

            Console.ReadLine();
        }

        static void ConnectSQL(out SqlConnection sqlConnection)
        {
            Console.WriteLine("连接数据库");
            //服务器资源管理器->数据连接->数据库右键属性->连接->连接字符串
            string constr = "Data Source=PC-LCL\\SQLEXPRESS;Initial Catalog=TestDb;Integrated Security=True;Pooling=False";
            sqlConnection = new SqlConnection(constr);
            string sql = "select * from Account";
            SqlCommand com = new SqlCommand(sql, sqlConnection);
            try
            {
                sqlConnection.Open();
                Console.WriteLine("成功连接数据库");
                int x = (int)com.ExecuteScalar();
                Console.WriteLine(string.Format("成功读取{0}条记录", x));
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
            }
        }

        static void ReaderSQLData(SqlConnection sqlConnection, string accountName, string password)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;
            command.CommandType = CommandType.Text;

            StringBuilder commandStr = new StringBuilder();
            commandStr.Append(" Select * from Account");
            commandStr.Append(" Where AccountName = '" + accountName + "'" + " And Password = '" + password + "'");
            command.CommandText = commandStr.ToString();

            SqlDataReader reader = command.ExecuteReader();     //执行读取，返回“流”
            if (reader.Read())
            {
                Console.WriteLine(reader["AccountName"].ToString() + " | " + reader["Password"].ToString());
            }
            else
            {
                Console.WriteLine("shit");
            }
        }

        static void ReaderSQLData2(DataSet dataSet, string tableName)
        {
            DataTable myTable = dataSet.Tables[tableName];
            foreach (DataRow myRow in myTable.Rows)
            {
                foreach (DataColumn myColumn in myTable.Columns)
                    Console.Write(myRow[myColumn] + " | "); //遍历表中的每个单元格
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static SqlDataAdapter GetDataAdapter(SqlConnection sqlConnection, string tableName)
        {
            return new SqlDataAdapter("select * from " + tableName, sqlConnection);
        }

        static DataSet FillAdapterWithDataSet(ref SqlDataAdapter dataAdapter, string tableName)
        {
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, tableName);
            return dataSet;
        }

        static void ModifySQLDataByRow<ModifyType>(SqlDataAdapter dataAdapter, DataSet dataSet, string tableName, string rowName, ModifyType value)
        {
            // 修改DataSet
            DataTable myTable = dataSet.Tables[tableName];
            foreach (DataRow myRow in myTable.Rows)
                myRow[rowName] = value;

            // 将DataSet的修改提交至“数据库”
            SqlCommandBuilder mySqlCommandBuilder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.Update(dataSet, tableName);
        }

        static void AddSQLData(SqlDataAdapter dataAdapter, DataSet dataSet, string tableName, string name, string sex, uint age)
        {
            DataTable myTable = dataSet.Tables[tableName];
            // 添加一行
            DataRow myRow = myTable.NewRow();
            myRow["Name"] = name;
            myRow["Sex"] = sex;
            myRow["Age"] = age;

            myTable.Rows.Add(myRow);

            // 将DataSet的修改提交至“数据库”
            SqlCommandBuilder mySqlCommandBuilder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.Update(dataSet, tableName);
        }

        static void AddSQLData2(SqlConnection sqlConnection, string name, string password)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;

            StringBuilder commandStr = new StringBuilder();
            commandStr.Append(" Insert Into Account(AccountName, Password) Values('" + name + "','" + password + "')");

            command.CommandText = commandStr.ToString();
            command.ExecuteScalar();
        }

        static void RemoveSQLDataByRow(SqlDataAdapter dataAdapter, DataSet dataSet, string tableName, int deleteIndex)
        {
            DataTable myTable = dataSet.Tables[tableName];
            myTable.Rows[deleteIndex].Delete();

            SqlCommandBuilder mySqlCommandBuilder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.Update(dataSet, tableName);
        }

        static void RemoveSQLData2(SqlConnection sqlConnection, string name)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;

            StringBuilder commandStr = new StringBuilder();
            commandStr.Append("  DELETE FROM Account WHERE AccountName = '" + name + "'");

            command.CommandText = commandStr.ToString();
            command.ExecuteScalar();
        }

    }
}