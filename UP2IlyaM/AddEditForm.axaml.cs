using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using UP2IlyaM.Classes;

namespace UP2IlyaM;

public partial class AddEditForm : Window
{
    private List<Services> _services;
    private Services currentService;
    private MySqlConnection _connection;
    private string connectionString = "server=localhost;database=ilyam41;port=3306;User Id=root;password=root"; 
    public AddEditForm(Services  currentService, List<Services> _services) //передача листа и выбранной записи
    {
        InitializeComponent();
        this.currentService = currentService;
        this.DataContext = this.currentService;
        this._services = _services;
    }
    
    private async void File_Select(object? sender, RoutedEventArgs e)
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        //доступ только конкретным расширениям
        fileDialog.Filters.Add(new FileDialogFilter(){Name = "Image Files", Extensions = {"jpg", "jpeg", "png", "gif"}});
        //отображение диалогового окна и получение выбранных файлов
        string[]? fileNames = await fileDialog.ShowAsync(this);
        //если файл выбран, то путь к нему заносится в переменную
        if (fileNames != null && fileNames.Length > 0)
        {
            string ImagePath = System.IO.Path.GetFileName(fileNames[0]);
            Img.Text = ImagePath;
        }
    }

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        //получение номера выбранной записи с формы "Услуги"
        var currentService = _services.FirstOrDefault(x => x.ID == this.currentService.ID);
        //если запись не была выбрана, то происходит создание новой записи
        if (currentService == null)
        {
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
            string query = "INSERT INTO Services (Service, Price, Length, Description, Discount, Image_ID) VALUES ('" + ServiceName.Text + "', '" + Convert.ToDouble(PriceService.Text) + "', '" + LengthService.Text + "', '" + ServiceDescription.Text + "', '" + DiscountService.Text + "', '" + Img.Text + "');";
            MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            _connection.Close();
        }
        //если запись была выбрана, то происходит редактирование выбранной записи
        else
        {
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
            string query = $"UPDATE Services SET Service = '{ServiceName.Text}', Price = {Convert.ToDouble(PriceService.Text)}, Length = '{LengthService.Text}', Description = '{ServiceDescription.Text}', Discount = {Convert.ToDouble(DiscountService.Text)}, Image_ID = '{Img.Text}' WHERE ID = {Convert.ToInt32(ID.Text)} ";
            MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            _connection.Close();
        }
    }

    private void BacktoMain(object? sender, RoutedEventArgs e)
    {
        ServiceWindow sw = new ServiceWindow();
        sw.Show();
        this.Hide();
    }
}