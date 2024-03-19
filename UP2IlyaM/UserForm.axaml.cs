using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MySql.Data.MySqlClient;
using UP2IlyaM.Classes;

namespace UP2IlyaM;

public partial class UserForm : Window
{
    private List<Services> _services;
    private string connectionString = "server=localhost;database=ilyam41;port=3306;User Id=root;password=root";
    private MySqlConnection _connection;
    public UserForm()
    {
        InitializeComponent();
        string FullTable = "SELECT * FROM Services";
        ShowTable(FullTable);
        FillCmb();
    }
    
    public void ShowTable(string query)
    {
        _services = new List<Services>();
        _connection = new MySqlConnection(connectionString);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(query, _connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var CurrentService = new Services()
            {
                ID = reader.GetInt32("ID"),
                Service = reader.GetString("Service"),
                Price = reader.GetInt32("Price"),
                Length = reader.GetString("Length"),
                Description = reader.GetString("Description"),
                Discount = reader.GetDouble("Discount"),
                ImagePath = reader.GetString("Image_ID"),
                Image_ID = LoadImage("avares://UP2IlyaM/Images/" + reader.GetString("Image_ID"))
            };
            _services.Add(CurrentService);
        }
            
        _connection.Close();
        DataGrid.ItemsSource = _services;
    }

    public Bitmap LoadImage(string Uri)
    {
        return new Bitmap(AssetLoader.Open(new Uri(Uri)));
    }

    public void FillCmb()
    {
        var discountComboBox = this.Find<ComboBox>("DiscountComboBox");
        discountComboBox.ItemsSource = new List<string>
        {
            "Все скидки",
            "От 0% до 5%",
            "От 5% до 15%",
            "От 15% до 30%",
            "От 30% до 70%"
        };
    }

    private void SearchService(object? sender, TextChangedEventArgs e)
    {
        string query =
            $"SELECT * FROM Services WHERE Service LIKE '%{ServSearch.Text}%'";
        ShowTable(query);
    }

    private void SortAsscending(object? sender, RoutedEventArgs e)
    {
        string query =
            $"SELECT * FROM Services ORDER BY Price ASC ;";
        ShowTable(query);
    }

    private void DiscountFilter(object? sender, SelectionChangedEventArgs e)
    {
        string txt = "";
        string query = "SELECT * FROM Services ";
        var discountComboBox = (ComboBox)sender;
        var selectedDiscount = discountComboBox.SelectedIndex;
        if (selectedDiscount == 0)
        {
            ShowTable(query);
        }

        if (selectedDiscount == 1)
        {
            txt = "WHERE Discount >= 0 AND Discount < 5";
            ShowTable(query+txt);
        }

        if (selectedDiscount == 2)
        {
            txt = "WHERE Discount >= 5 AND Discount < 15";
            ShowTable(query+txt);
        }

        if (selectedDiscount == 3)
        {
            txt = "WHERE Discount >=15 AND Discount < 30";
            ShowTable(query+txt);
        }
        
        if (selectedDiscount == 4)
        {
            txt = "WHERE Discount >=30 AND Discount < 70";
            ShowTable(query+txt);
        }
    }

    private void SortDescending(object? sender, RoutedEventArgs e)
    {
        string query =
            $"SELECT * FROM Services ORDER BY services.Price DESC ;";
        ShowTable(query);
    }

    private void LogOut(object? sender, RoutedEventArgs e)
    {
        MainWindow mw = new MainWindow();
        mw.Show();
        this.Close();
    }

    private void Note(object? sender, RoutedEventArgs e)
    {
        NoteForm nf = new NoteForm();
        nf.Show();
        this.Close();
    }
}