using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

namespace WPExam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GetFiles(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(urlTextBox.Text))
            {
                MessageBox.Show("Введите URL");
                return;
            }
            using (WebClient client = new WebClient())
            {
                try
                {
                    string html = await client.DownloadStringTaskAsync(new Uri(urlTextBox.Text));
                    HtmlParser parser = new HtmlParser();
                    var document = await parser.ParseDocumentAsync(html);
                    var filesList = document.QuerySelectorAll("*");
                    foreach (var file in filesList)
                    {
                        foreach (var attribute in file.Attributes)
                        {
                            if (attribute.Name == "href")
                            {
                                if (attribute.Value.Contains(".exe") || attribute.Value.Contains(".rar") || attribute.Value.Contains(".png") || attribute.Value.Contains(".jpg") || attribute.Value.Contains(".gif"))
                                {
                                    filesListBox.Items.Add(attribute.Value);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    logTextBox.Text += "\nНе удалось подключиться к " + urlTextBox.Text;
                }
            }
        }

        private void DownloadFile(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(pathTextBox.Text))
            {
                MessageBox.Show("Введите название файла и путь");
                return;
            }
            if (filesListBox.SelectedItem == null)
            {
                MessageBox.Show("Ввыберите файл который хотите скачать");
                return;
            }
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFileAsync(new Uri(urlTextBox.Text + (filesListBox.SelectedItem as string)), pathTextBox.Text);
                    logTextBox.Text += "\n" + filesListBox.SelectedItem + "успешно скачан";
                }
                catch (Exception ex)
                {
                    logTextBox.Text += "\nНе удалось скачать " + filesListBox.SelectedItem;
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
