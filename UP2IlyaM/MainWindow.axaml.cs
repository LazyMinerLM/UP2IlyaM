using System;
using System.Collections.Generic;
using System.Data;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;

namespace UP2IlyaM;

public partial class MainWindow : Window
{
    private string connectionString = "server=localhost;database=ilyam41;port=3306;User Id=root;password=root";
    private MySqlConnection connection;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void LoginApp(object? sender, RoutedEventArgs e)
    {
        connection = new MySqlConnection(connectionString);
        connection.Open();
        DataTable table = new DataTable();
        string query = $"SELECT LogIn, Password, Role_ID FROM Users WHERE LogIn LIKE '{LogIn.Text}' AND Password LIKE '{Password.Text}'";
        MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
        adapter.Fill(table);
        //если пользователь существует и имеет права администратора, то он переходит на форму для администратора
        if (table.Rows.Count > 0)
        {
            if (table.Rows[0][2].ToString() == "1")
            {
                ServiceWindow sw = new ServiceWindow();
                sw.Show();
                this.Hide();
            }
            else //иначе переходит на форму обычного пользователя
            {
                UserForm uw = new UserForm();
                uw.Show();
                this.Hide();
            }
        }
    }

    private void Exit_Program(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
}