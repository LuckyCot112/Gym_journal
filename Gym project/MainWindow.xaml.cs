using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gym_project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path = "Data.dat";
        AddWindow aw = new AddWindow();

        List<string[]> data = new List<string[]>();
        string[] info = new string[3];


        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists("Data.dat") != true)
                File.Create("Data.dat");
            Show_List("");
        }
        class data_grid
        {
            public data_grid(string Name, string Training, string Date, bool Confirm)
            {
                this.Name = Name;
                this.Training = Training;
                this.Date = Date;
                this.Confirm = Confirm;
            }
            public string Name { get; set; }
            public string Training { get; set; }
            public string Date { get; set; }
            public bool Confirm { get; set; }
        }

        public void Show_List(string filter)
        {
            grid1.ItemsSource = null;
            List<data_grid> result = new List<data_grid>(3);

            if (File.Exists(path))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        String line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            info = line.Split('|');
                            if (info[0].Trim() != "" && info[1].Trim() != "" && info[2].Trim() != "" && info[3].Trim() != "" && info[0].Contains(filter))
                                result.Add(new data_grid(info[0], info[1], info[2], Convert.ToBoolean(info[3])));
                        }
                        sr.Close();
                    }
                    grid1.Items.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                }
                catch 
                {
                    MessageBox.Show("Возникла ошибка при чтении файла", "Критическая ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            grid1.ItemsSource = result;//*/
        }

        private void button_add_Click(object sender, RoutedEventArgs e)
        {
            aw.Owner = this;
            aw.ShowDialog();
            aw.table.Text = "";
            aw.textbox_date.Text = "";
            aw.textbox_name.Text = "";
            aw.type.Text = "";
            aw.button_add.Visibility = Visibility.Visible;
            aw.button_edit.Visibility = Visibility.Hidden;
            //aw.button_edit.IsEnabled = false;
        }

        public void button_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ind = grid1.SelectedIndex;
                aw.textbox_name.Text = (grid1.Columns[0].GetCellContent(grid1.Items[ind]) as TextBlock).Text.ToString();
                aw.type.Text = (grid1.Columns[1].GetCellContent(grid1.Items[ind]) as TextBlock).Text.ToString();
                aw.textbox_date.Text = (grid1.Columns[2].GetCellContent(grid1.Items[ind]) as TextBlock).Text.ToString();
                aw.calendar1.SelectedDate = DateTime.Parse(aw.textbox_date.Text.Remove(16, 8));
                aw.table.Text = aw.textbox_date.Text.Remove(0, 11);
                aw.checkbox_confirm.IsChecked = (grid1.Columns[3].GetCellContent(grid1.Items[ind]) as CheckBox).IsChecked;
                aw.Owner = this;
                aw.button_add.Visibility = Visibility.Hidden;
                aw.button_edit.Visibility = Visibility.Visible;
                aw.textbox_ident.Text = $"{(grid1.Columns[0].GetCellContent(grid1.Items[ind]) as TextBlock).Text}|{(grid1.Columns[1].GetCellContent(grid1.Items[ind]) as TextBlock).Text}|{(grid1.Columns[2].GetCellContent(grid1.Items[ind]) as TextBlock).Text}";
                aw.checkbox_confirm.Visibility = Visibility.Visible;
                aw.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Возникла ошибка при попытке отредактировать запись.", "Ошибка чтения!", MessageBoxButton.OK, MessageBoxImage.Error);
            }//*/

        }

        private void button_del_Click(object sender, RoutedEventArgs e)
        {
            int ind = grid1.SelectedIndex;
            try
            {
                string txt_new = null;
                string[] meetings = File.ReadAllText(path).Split('\n');
                //*
                if (MessageBox.Show($"Вы уверены, что хотите удалить запись\n{(grid1.Columns[0].GetCellContent(grid1.Items[ind]) as TextBlock).Text}|{(grid1.Columns[1].GetCellContent(grid1.Items[ind]) as TextBlock).Text}|{(grid1.Columns[2].GetCellContent(grid1.Items[ind]) as TextBlock).Text}?", "Удаление!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    for (int i = 0; i < meetings.Length; i++)
                    {
                        if (meetings[i].Contains($"{(grid1.Columns[0].GetCellContent(grid1.Items[ind]) as TextBlock).Text}|{(grid1.Columns[1].GetCellContent(grid1.Items[ind]) as TextBlock).Text}|{(grid1.Columns[2].GetCellContent(grid1.Items[ind]) as TextBlock).Text}"))
                        {
                            meetings[i] = String.Empty;
                        }
                        else
                        {
                            txt_new += meetings[i] + "\n";
                        }
                    }//*/
                    File.WriteAllText(path, string.Empty);
                    while (txt_new.Contains($"\n\n"))
                    {
                        txt_new = txt_new.Replace($"\n\n", $"\n");
                    }
                    using (StreamWriter file = new StreamWriter(path))
                    {
                        file.Write(txt_new);
                    }
                }
            }
            catch
            {

            }
            Show_List("");
        }

        private void button_stat_Click(object sender, RoutedEventArgs e)
        {
            StatWindow stw = new StatWindow();
            stw.Owner = this;
            stw.ShowDialog();
        }

        private void button_search_Click(object sender, RoutedEventArgs e)
        {
            string search_text = "";
            if (grid1.SelectedIndex != -1)
                search_text = (grid1.Columns[0].GetCellContent(grid1.Items[grid1.SelectedIndex]) as TextBlock).Text.ToString();
            SearchWindow sw = new SearchWindow(search_text);
            sw.Owner = this;
            sw.ShowDialog();
        }

        private void button_table_Click(object sender, RoutedEventArgs e)
        {
            TableWindow tb = new TableWindow();
            tb.Owner = this;
            tb.ShowDialog();
        }

        private void closing1(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                e.Cancel = true;
            else
            {
                aw.Close();
                TableWindow tw = new TableWindow();
                tw.Close();
            }
        }

        private void button_reset_Click(object sender, RoutedEventArgs e)
        {
            Show_List("");
        }
    }
}
