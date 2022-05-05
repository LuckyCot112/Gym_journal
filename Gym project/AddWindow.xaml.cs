using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Gym_project
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public string ident;
        string path = "Data.dat";
        public int count = 0;
        //string[] info = new string[3];

        public AddWindow()
        {
            InitializeComponent();
            button_edit.Visibility = Visibility.Hidden;
            prestart();
        }

        public void prestart()
        {
            for (int i = 10; i < 21; i += 2)
                table.Items.Add($"{i}:00 - {i+1}:30");
            type.Items.Add("Силовая тренировка");
            type.Items.Add("Акробатика");
            type.Items.Add("Свободное занятие");
            type.Items.Add("Атлетика");
        }

        public void date_to_text()
        {
            try
            {
                DateTime date = (DateTime)calendar1.SelectedDate;
                string s = $"{date} {table.Text}";
                MessageBox.Show(s + " " + s.Length);
                if (s.Length == 32)
                    s = s.Remove(11, 8);
                else if (s.Length == 33)
                    s = s.Remove(11, 9);
                MessageBox.Show(s + " " + s.Length);
                textbox_date.Text = s;
            }
            catch
            {
                MessageBox.Show("Не удалось ввести дату! Проверьте, указаны ли у вас дата и время встречи.", "Ошибка данных!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void free_table(string search_text)
        {
            using (StreamReader sr = new StreamReader(path))
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

        public void write_string(bool confirmed)
        {
            if (File.ReadAllText(path).Contains($"{textbox_name.Text}|{type.Text}|{textbox_date.Text}") != true)
            {
                free_table($"{type.Text}|{textbox_date.Text}");
                if (count < 6)
                {
                    FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    {
                        if (textbox_name.Text.Trim() != "" && type.Text.Trim() != "" && textbox_date.Text.Trim() != "" && textbox_date.Text.Length == 24)
                        {
                            StreamWriter sw = new StreamWriter(fs);
                            sw.WriteLine($"{textbox_name.Text.Trim()}|{type.Text.Trim()}|{textbox_date.Text.Trim()}|{confirmed}");
                            sw.Close();
                        }
                        else
                        {
                            MessageBox.Show("Не все данные введены! Пожалуйста, введите данные.", "Ошибка данных!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                    fs.Close();
                }
                else
                    MessageBox.Show($"Нет свободных мест на занятие {textbox_date.Text}");
            }
            else
                MessageBox.Show("Данная запись уже существует", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void button_add_click(object sender, RoutedEventArgs e)
        {
            try
            {
                {
                    bool confirmed = false;
                    DateTime time_training;
                    date_to_text();
                    time_training = DateTime.Parse(textbox_date.Text.Remove(16, 8));

                    if ((DateTime.Now.AddMinutes(-30)) > time_training)
                    {
                        MessageBox.Show("Нельзя записать клиента раньше, чем на пол часа до текущего времени.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else if (DateTime.Now.AddMinutes(-30) <= time_training && DateTime.Now.AddMinutes(30) >= time_training)
                    {
                        if (MessageBox.Show("Ввести клиента на текущую тренировку?", "Ввод клиента", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            confirmed = true;
                            write_string(confirmed);
                            (this.Owner as MainWindow).Show_List("");
                            this.Hide();
                        }
                    }
                    else if (DateTime.Now.AddMinutes(30) < time_training)
                    {
                        if (MessageBox.Show("Записать клиента на тренировку?", "Запись клиента", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            write_string(confirmed);
                            (this.Owner as MainWindow).Show_List("");
                            this.Hide();
                        }
                    }
                    else
                        MessageBox.Show("Возникла неизвестная ошибка при определении даты и времени.", "Ошибка данных!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show($"Произошла ошибка во время записи клиента. Проверьте правильность введённых данных.", "Ошибка данных!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_edit_click(object sender, RoutedEventArgs e)
        {
            try
            {
                string txt_new = null;
                string[] client = File.ReadAllText(path).Split('\n');
                date_to_text();
                for (int i = 0; i < client.Length; i++)
                {
                    if (client[i].Contains(textbox_ident.Text))
                    {
                        txt_new += $"{textbox_name.Text}|{type.Text}|{textbox_date.Text}|{checkbox_confirm.IsChecked}\n";
                    }
                    else
                    {
                        txt_new += client[i] + "\n";
                    }
                }
                File.WriteAllText(path, string.Empty);
                while (txt_new.Contains($"\n\n"))
                {
                    txt_new = txt_new.Replace($"\n\n", $"\n");
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
                {
                    file.Write(txt_new);
                }
            (this.Owner as MainWindow).Show_List("");
                button_cancel_Click(this, e);
                //this.Hide();
            }
            catch
            {
                MessageBox.Show("Не удалось изменить запись.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            textbox_date.Text = "";
            textbox_name.Text = "";
            type.Text = "";
            table.Text = "";
            checkbox_confirm.IsChecked = false;
            checkbox_confirm.Visibility = Visibility.Hidden;
            button_edit.Visibility = Visibility.Hidden;
            button_add.Visibility = Visibility.Visible;
        }
    }
}
