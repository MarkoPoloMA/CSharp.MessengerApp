using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp
{
    internal class DataBase
    {
        public MySqlConnection connection = new MySqlConnection("server=89.110.79.233;port=3306;username=JOJO;password=Colobok123?;database=messengerApp;");

        public void openConnection() 
        {
            if(connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public MySqlConnection GetConnection() 
        {
            return connection; 
        }
    }
}
