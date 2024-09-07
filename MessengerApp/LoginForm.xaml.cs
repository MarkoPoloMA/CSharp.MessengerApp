using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MessengerApp
{
    /// <summary>
    /// Логика взаимодействия для LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }


        private void connect_Click(object sender, RoutedEventArgs e)
        {
            string loginUser = loginField.Text;
            string passUser = passField.Password;
            if (loginUser != "" && passUser != "")
            {
                DataBase dataBase = new DataBase();
                dataBase.openConnection();
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();

                string sql = $"SELECT * FROM user WHERE id = '{loginUser}' and password = '{passUser}'";
                MySqlCommand command = new MySqlCommand(sql, dataBase.GetConnection());
                adapter.SelectCommand = command;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    MessageBox.Show("Пользователь авторизован");
                    MainWindow mainWindow = new MainWindow(loginUser);
                    mainWindow.Show();
                    Close();
                    dataBase.closeConnection();
                }
                else
                    MessageBox.Show("Пользователь не авторизован");
            }
            else
                MessageBox.Show("Заполните все поля!");
        }

        private void regist_Click(object sender, RoutedEventArgs e)
        {
            string loginUser = loginField.Text;
            string passUser = passField.Password.ToString();
            if (loginUser != "" && passUser != ""){
            DataBase dataBase = new DataBase();
            dataBase.openConnection();
            string sql = $"SELECT * FROM user WHERE id = '{loginUser}'";
            MySqlCommand command = new MySqlCommand(sql, dataBase.GetConnection());
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;
            DataTable table = new DataTable();
            adapter.Fill(table);
                if (table.Rows.Count > 0)
                    MessageBox.Show("Логин занят");
                else
                {
                    command = new MySqlCommand($"INSERT INTO user (id, password) VALUE('{loginUser}', '{passUser}')", dataBase.GetConnection());

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Аккаунт успешно создан");
                        dataBase.closeConnection();
                    }
                    else
                        MessageBox.Show("Аккаунт не создан!");
                }
            }
            else
                MessageBox.Show("Заполните все поля!");
        }
    
    }
}
