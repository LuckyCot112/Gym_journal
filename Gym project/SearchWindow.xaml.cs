using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace Gym_project
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        string path = "Data.dat";
        public SearchWindow(string search_text)
        {
            InitializeComponent();
            textbox_search.Text = search_text;

            search_name(search_text);
        }

        class data_grid
        {
            public data_grid(string Name)
            {
                this.Name = Name;
            }
            public string Name { get; set; }
        }

        public void search_name(string search_text)
        {
            List<string[]> data = new List<string[]>();
            string[] info = new string[3];

            //grid_result.ItemsSource = null;
            List<data_grid> result = new List<data_grid>(3);
            List<string> container1 = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        info = line.Split('|');
                        if (info[0].Trim() != "" && info[0].Contains(search_text))
                        {
                            if (container1.Exists(x => x.Contains(info[0])))
                            {
                                
                            }
                            else
                            {
                                result.Add(new data_grid(info[0]));
                                container1.Add(info[0]);
                            }
                        }
                    }
                    sr.Close();
                }
                grid_result.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));
            }

            catch
            {
                MessageBox.Show("Возникла ошибка при чтении файла", "Критическая ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            grid_result.ItemsSource = result;//*/
        }


        private void button_search_name_Click(object sender, RoutedEventArgs e)
        {
            if (grid_result.SelectedIndex == -1)
                search_name(textbox_search.Text);
            else
            {
                (this.Owner as MainWindow).Show_List((grid_result.Columns[0].GetCellContent(grid_result.Items[grid_result.SelectedIndex]) as TextBlock).Text.ToString());
                this.Hide();
            }

        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}