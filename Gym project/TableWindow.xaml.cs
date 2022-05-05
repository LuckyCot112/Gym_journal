using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для TableWindow.xaml
    /// </summary>
    public partial class TableWindow : Window
    {
        public string[] names = new string[7];
        
        public TableWindow()
        {
            InitializeComponent();
            for (int i = 10, j = 1; j <=6; i += 2, j++)
                names[j] = $"{i}:00 - {i + 1}:30";
            names[0] = "День недели";
            grid1.ItemsSource = null;
            combobox1.Items.Add("Силовая тренировка");
            combobox1.Items.Add("Акробатика");
            combobox1.Items.Add("Свободное занятие");
            combobox1.Items.Add("Атлетика");
        }

        public int count = 1;

        class table_data
        {
            public table_data(string Day, int clock10, int clock12, int clock14, int clock16, int clock18, int clock20)
            {
                this.Day = Day;
                this.clock10 = clock10;
                this.clock12 = clock12;
                this.clock14 = clock14;
                this.clock16 = clock16;
                this.clock18 = clock18;
                this.clock20 = clock20;
            }
            public string Day { get; set; }
            public int clock10 { get; set; }
            public int clock12 { get; set; }
            public int clock14 { get; set; }
            public int clock16 { get; set; }
            public int clock18 { get; set; }
            public int clock20 { get; set; }
        }

        public void free_table(string search_text)
        {
            using (StreamReader sr = new StreamReader("Data.dat"))
            {
                count = 0;
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "" && line.Contains(search_text))
                        count++;
                }
                sr.Close();
            }
        }

        public void matching()
        {
            List<table_data> result = new List<table_data>(7);
            int[] info = new int[7];
            for (int i = 0; i <= 14; i++)
            {
                for (int j = 1; j <= 6; j++)
                {
                    string search_text = $"{DateTime.Now.Date.AddDays(i)} {names[j]}".Remove(11, 8);
                    search_text = $"{combobox1.Text}|{search_text}";
                    free_table(search_text);
                    info[j] = 6 - count;
                }
                //MessageBox.Show($"{DateTime.Now.Date.AddDays(i).ToString()} {info[1]} {info[2]} {info[3]} {info[4]} {info[5]} {info[6]}");
                result.Add(new table_data(DateTime.Now.Date.AddDays(i).ToString().Remove(11, 7), info[1], info[2], info[3], info[4], info[5], info[6]));
            }
            grid1.ItemsSource = result;

            grid1.Columns[1].Header = "10:00-11:30";
            grid1.Columns[2].Header = "12:00-13:30";
            grid1.Columns[3].Header = "14:00-15:30";
            grid1.Columns[4].Header = "16:00-17:30";
            grid1.Columns[5].Header = "18:00-19:30";
            grid1.Columns[6].Header = "20:00-21:30";
        }

        private void combobox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            matching();
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
