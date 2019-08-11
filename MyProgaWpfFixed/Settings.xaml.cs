using System;
using System.Collections.Generic;
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
using System.IO;

namespace MyProgaWPF
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    /// 
    public partial class Settings : Window
    {
        public Settings()
        {
            try
            {
                InitializeComponent();
                List<string> bufList = readSettings();
                NameOfServiceCenter.Text = bufList[0];
                PhoneOfServiceCenter.Text = bufList[1];
                WorkHours.Text = bufList[2];
                AddressOfServiceCenter.Text = bufList[3];
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка в окне настроек");
            }
        }
        public static List<string> readSettings()
        {
            List<string> settings = new List<string>() { "Укажите название в настройках", "Укажите телефон в настройках", "Укажите расписание в настройках", "Укажите адрес в настройках" };
            try
            {
                if (File.Exists("settings.txt"))
                {
                    using (StreamReader fromFile = new StreamReader("settings.txt", System.Text.Encoding.Default))
                    {
                        if (fromFile.Peek() != -1)
                        {
                            for (int i = 0; i < settings.Count && (settings[i] = fromFile.ReadLine()) != null; i++)
                            {
                                //MessageBox.Show(settings[i]);
                            }
                        }
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка при чтении настроек");
            }
            return settings;
        }
        private void ClickSave(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamWriter toFile = new StreamWriter("settings.txt", false, System.Text.Encoding.Default);
                toFile.Write(NameOfServiceCenter.Text + Environment.NewLine + PhoneOfServiceCenter.Text + Environment.NewLine + WorkHours.Text + Environment.NewLine + AddressOfServiceCenter.Text);
                toFile.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка при записи настроек");
            }
            this.Close();
        }
    }
}
