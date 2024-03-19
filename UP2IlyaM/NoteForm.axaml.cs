using System;
using System.Collections.Generic;
using System.Data;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using UP2IlyaM.Classes;

namespace UP2IlyaM;

public partial class NoteForm : Window
{
    private List<Services> _services;
    private MySqlConnection connection;
    string connectionString = "server=localhost;database=ilyam41;port=3306;User Id=root;password=root";

    public NoteForm()
    {
        InitializeComponent();
        FillCmb();
    }

    public void FillCmb()
    {
        _services = new List<Services>();
        connection = new MySqlConnection(connectionString);
        connection.Open();
        MySqlCommand command = new MySqlCommand("SELECT Service FROM Services", connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var CurrentService = new Services()
            {
                Service = reader.GetString("Service"),
            };
            _services.Add(CurrentService);
        }
        connection.Close();
        var servicesComboBox = this.Find<ComboBox>("CmbServices");
        //поиск комбобокса с именем CmbServices
        servicesComboBox.ItemsSource = _services;
    }

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        connection = new MySqlConnection(connectionString);
        connection.Open();
        DataTable table = new DataTable();
        string query = $"SELECT * FROM Clients WHERE Phone LIKE '{ClientPhone}'";
        MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
        var addcmb = CmbServices.SelectedIndex + 7;
        string addorder = "";
        adapter.Fill(table);
        if (table.Rows.Count > 0)
            //если клиент существует, то добавляется только заказ с заданными данными
        {
            addorder = $"INSERT INTO Orders (Client_ID, Service_ID, Start, End) VALUES ({Convert.ToInt32(table.Rows[0][0])}, {addcmb}, '{StartDate.Text}', '{EndDate.Text})'";
            MySqlCommand cmd = new MySqlCommand(addorder, connection);
            cmd.ExecuteNonQuery();
        }
        else
            //если клиент не существует, то добавляется новый клиент и новый заказ
        {
            string addclient = $"INSERT INTO Clients (Name, Surname, MiddleName, Phone) VALUES ('{ClientName.Text}', '{ClientSurname.Text}', '{ClientMiddleName.Text}', '{ClientPhone.Text}')";
            addorder = $"INSERT INTO Orders (Client_ID, Service_ID, Start, End) VALUES ((SELECT ID FROM clients WHERE Phone LIKE '{ClientPhone}'), {addcmb}, '{StartDate.Text}', '{EndDate.Text}');";
            MySqlCommand cmd = new MySqlCommand(addclient, connection);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand(addorder, connection);
            cmd.ExecuteNonQuery();
        }
        connection.Close();
        
    }

    private void GoBack(object? sender, RoutedEventArgs e)
    {
        ServiceWindow sw = new ServiceWindow();
        sw.Show();
        this.Close();
    }
}