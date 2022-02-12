using CarsData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cs_HttpClientDekstopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string url = null;
        HttpResponseMessage response = null;
        HttpClient client = null;
        string json = null;
        StringContent content = null;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                url = "http://localhost:45001/";
                client = new HttpClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void BtnGetCars_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                response = await client.GetAsync(url);
                json = response.Content.ReadAsStringAsync().Result;
                var cars = JsonSerializer.Deserialize<List<Car>>(json);
                lbCars.ItemsSource = cars;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAddCar_Click(object sender, RoutedEventArgs e)
        {
            Command.CurrentCmd = "POST";
            gridColumn1_0.Width = new GridLength(3, GridUnitType.Star);
            txbId.Text = "New car id";
        }

        private async void BtnConfirm_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txbColor.Text)
                && !string.IsNullOrWhiteSpace(txbMake.Text)
                && !string.IsNullOrWhiteSpace(txbModel.Text)
                && !string.IsNullOrWhiteSpace(txbVolume.Text)
                && !string.IsNullOrWhiteSpace(txbYear.Text))
                {
                    var newCar = new Car()
                    {
                        Make = txbMake.Text,
                        Model = txbModel.Text,
                        Color = txbColor.Text,
                        Volume = decimal.Parse(txbVolume.Text),
                        Year = int.Parse(txbYear.Text)
                    };

                    if (Command.CurrentCmd == "PUT")
                        newCar.Id = int.Parse(txbId.Text);

                    await Task.Run(() =>
                    {
                        json = JsonSerializer.Serialize(newCar);
                        content = new StringContent(json);
                    });

                    switch (Command.CurrentCmd)
                    {
                        case "POST":
                            response = client.PostAsync(url, content).Result;
                            if (response.IsSuccessStatusCode) MessageBox.Show("Car ugurla add olundu!");
                            else MessageBox.Show("Car add olunmasinda problem yarandi!");
                            break;
                        case "PUT":
                            response = await client.PutAsync(url, content);
                            if (response.IsSuccessStatusCode) MessageBox.Show("Car ugurla update olundu!");
                            else MessageBox.Show("Car update olunmasinda problem yarandi!");
                            break;
                        default:
                            break;
                    }

                    BtnGetCars_ClickAsync(sender, e);
                    gridColumn1_0.Width = new GridLength(100000, GridUnitType.Star);
                    txbId.Text = "";
                    txbMake.Text = "";
                    txbColor.Text = "";
                    txbModel.Text = "";
                    txbVolume.Text = "";
                    txbYear.Text = "";
                }
                else
                    MessageBox.Show("Melumatlari tam doldurun!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCloseSubMenu_Click(object sender, RoutedEventArgs e)
        {
            gridColumn1_0.Width = new GridLength(100000, GridUnitType.Star);
        }

        private void TxbYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txbYear.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                txbYear.Text = txbYear.Text.Remove(txbYear.Text.Length - 1);
                txbYear.CaretIndex = txbYear.Text.Length;
            }
        }

        private void TxbVolume_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txbVolume.Text, @"[^(?:\d{1,2})?(?:\.\d{1,2})?$]"))
            {
                MessageBox.Show("Please enter float numbers.");
                txbVolume.Text = txbVolume.Text.Remove(txbVolume.Text.Length - 1);
                txbVolume.CaretIndex = txbVolume.Text.Length;
            }
        }

        private void BtnUpdateCar_Click(object sender, RoutedEventArgs e)
        {
            if (lbCars.SelectedItem != null)
            {
                Command.CurrentCmd = "PUT";
                var car = lbCars.SelectedItem as Car;
                gridColumn1_0.Width = new GridLength(3, GridUnitType.Star);
                txbId.Text = car?.Id.ToString();
                txbMake.Text = car?.Make;
                txbModel.Text = car?.Model;
                txbColor.Text = car?.Color;
                txbVolume.Text = car?.Volume.ToString();
                txbYear.Text = car?.Year.ToString();
            }
            else
            {
                MessageBox.Show("Xais edirik bir masin secin!");
            }
        }

        private async void BtnDeleteCar_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (lbCars.SelectedItem != null)
            {
                try
                {
                    var selectedCar = lbCars.SelectedItem as Car;
                    url += $"?deletedCarId={selectedCar?.Id}";
                    response = await client.DeleteAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Car ugurla delete olundu!");
                        BtnGetCars_ClickAsync(sender, e);
                    }
                    else MessageBox.Show("Car delete olunmasinda problem yarandi!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Xais edirik bir masin secin!");
            }
        }
    }

    static class Command
    {
        public static string CurrentCmd { get; set; } = "GET";



    }
}
