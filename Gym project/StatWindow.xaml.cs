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
    /// Логика взаимодействия для StatWindow.xaml
    /// </summary>
    public partial class StatWindow : Window
    {
        public StatWindow()
        {
            InitializeComponent();
        }

        public void money()
        {
            try
            {
                using (StreamReader sr = new StreamReader("Data.dat"))
                {
                    string[] info = new string[3];
                    int count = 0;
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        info = line.Split('|');
                        if (line != "" && DateTime.Parse(info[2].Remove(16, 8)) > calendar1.SelectedDate && DateTime.Parse(info[2].Remove(16, 8)) < calendar2.SelectedDate && info[3] == "True")
                            count++;
                    }
                    sr.Close();
                    label_result.Content = $"Доход составляет {count * 300} рублей.";
                }
            }
            catch 
            {
                label_result.Content = "Промежуток не установлен";
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void calendar_changed1(object sender, SelectionChangedEventArgs e)
        {
            money();
        }
    }
}
