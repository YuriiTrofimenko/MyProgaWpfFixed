using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Text.RegularExpressions;

namespace MyProgaWPF
{
    /// <summary>
    /// Логика взаимодействия для AddRepair.xaml
    /// </summary>
    public partial class AddorChangeRepair : Window
    {
        device dev = new device();
        int indexBuf;
        //Добавление ремонта
        public AddorChangeRepair()
        {
            try
            {
                InitializeComponent();
                LabelCost.Visibility = Visibility.Collapsed;
                TXcost.Visibility = Visibility.Collapsed;
                TXmanufact.ItemsSource = readFromList("InManufact.txt");
                TXbreak.ItemsSource = readFromList("InBreak.txt");
                dev.ID += Convert.ToString(MainWindow.repairs.Count + 1);
                Add.Click -= ClickChange;
                Add.Click += ClickAdd;
                Add.Click -= ClickSave;
                //UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                // MessageBox.Show(Convert.ToString(elementWithFocus));

                this.Loaded += (sender, e) =>
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка в окне добавления ремонта");
            }
        }
        public AddorChangeRepair(device obj, int index, bool trigger)
        {
            indexBuf = index;
            dev = obj;
            try
            {
                InitializeComponent();
                //System.Windows.Input.Keyboard.Focus(TXclient.Text);
                TXclient.Focus();
                if (trigger)
                {
                    TXcost.Focus();
                    LabelClient.Visibility = Visibility.Collapsed;
                    TXclient.Visibility = Visibility.Collapsed;
                    LabelPhone.Visibility = Visibility.Collapsed;
                    TXphone.Visibility = Visibility.Collapsed;
                    LabelManufact.Visibility = Visibility.Collapsed;
                    TXmanufact.Visibility = Visibility.Collapsed;
                    LabelModel.Visibility = Visibility.Collapsed;
                    TXmodel.Visibility = Visibility.Collapsed;
                    LabelSerial.Visibility = Visibility.Collapsed;
                    TXserial.Visibility = Visibility.Collapsed;
                    LabelBreak.Visibility = Visibility.Collapsed;
                    TXbreak.Visibility = Visibility.Collapsed;
                    LabelPrePay.Visibility = Visibility.Collapsed;
                    TXprePay.Visibility = Visibility.Collapsed;
                    LabelPreCost.Visibility = Visibility.Collapsed;
                    TXpreCost.Visibility = Visibility.Collapsed;
                    LabelType.Visibility = Visibility.Collapsed;
                    TXtype.Visibility = Visibility.Collapsed;
                    LabelExternal.Visibility = Visibility.Collapsed;
                    TXexternal.Visibility = Visibility.Collapsed;
                    LabelKomplekt.Visibility = Visibility.Collapsed;
                    TXkomplekt.Visibility = Visibility.Collapsed;

                    Add.Click += ClickSave;
                    Add.Click -= ClickChange;
                    Add.Click -= ClickAdd;
                }
                else
                {
                    TXclient.Text = obj.Client;
                    TXphone.Text = obj.Phone;
                    TXmanufact.Text = obj.Manufact;
                    TXmodel.Text = obj.Model;
                    TXserial.Text = obj.Serial;
                    TXbreak.Text = obj.Break;
                    TXprePay.Text = Convert.ToString(obj.PrePay);
                    TXpreCost.Text = Convert.ToString(obj.PreCost);
                    TXcomment.Text = obj.Comment;

                    TXmanufact.ItemsSource = readFromList("InManufact.txt");
                    TXbreak.ItemsSource = readFromList("InBreak.txt");

                    Add.Click += ClickChange;
                    Add.Click -= ClickAdd;
                    Add.Click -= ClickSave;
                }

                TXcost.Text = Convert.ToString(obj.Cost);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка в окне добавления ремонта");
            }
        }
        private void ClickChange(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TXbreak.Text == "" || TXclient.Text == "" || TXexternal.Text == "" || TXkomplekt.Text == "" || TXmanufact.Text == "" || TXmodel.Text == "" || TXphone.Text == "" || TXpreCost.Text == "" || TXprePay.Text == "" || TXserial.Text == "" || TXtype.Text == "")
                {
                    MessageBox.Show("Все поля должны быть заполнены!!!");
                }
                else
                {
                    dev.Client = TXclient.Text;
                    dev.Phone = TXphone.Text;
                    dev.Type = TXtype.Text;
                    dev.Manufact = TXmanufact.Text;
                    dev.Model = TXmodel.Text;
                    dev.Serial = TXserial.Text;
                    dev.Break = TXbreak.Text;
                    dev.Komplekt = TXkomplekt.Text;
                    dev.External = TXexternal.Text;
                    dev.PrePay = Convert.ToInt32(TXprePay.Text);
                    dev.PreCost = Convert.ToInt32(TXpreCost.Text);
                    Program.WriteToFile(dev, Program.SearchInFile(dev.ID));
                    checkAndAdd(TXtype.Text, "InType.txt");
                    checkAndAdd(TXmanufact.Text, "InManufact.txt");
                    checkAndAdd(TXbreak.Text, "InBreak.txt");
                    MainWindow.repairs[(MainWindow.repairs.Count - 1) - indexBuf] = dev;

                    this.Close();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка при изменении ремонта");
            }
        }
        List<string> readFromList(string nameOfFile)
        {
            List<string> listFromFile = new List<string>();
            try
            {
                if (File.Exists(nameOfFile))
                {
                    using (StreamReader fromFile = new StreamReader(nameOfFile))
                    {
                        if (fromFile.Peek() != -1)
                        {
                            for (int i = 0; fromFile.Peek() != -1; i++)
                            {
                                listFromFile.Add(fromFile.ReadLine());
                            }
                        }
                    }
                }
                else
                {
                    FileStream file = File.Create(nameOfFile);
                    file.Close();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка при чтении из списка");
            }
            return listFromFile;
        }
        void checkAndAdd(string paramForComparsion, string nameOfFile)
        {
            try
            {
                bool find = false;
                List<string> bufList = readFromList(nameOfFile);
                for (int i = 0; i < bufList.Count; i++)
                {
                    if (bufList[i] == paramForComparsion)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    bool empty = false;
                    using (StreamReader fromFile = new StreamReader(nameOfFile, true))
                    {
                        if (fromFile.Peek() == -1)
                        {
                            empty = true;
                        }
                    }
                    using (StreamWriter toFile = new StreamWriter(nameOfFile, true))
                    {
                        if (!empty)
                        {
                            toFile.Write(Environment.NewLine);
                        }
                        toFile.Write(paramForComparsion);
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка при сравнение ввода со списком");
            }
        }
        private void AutoClient(object sender, PopulatingEventArgs e)
        {
            try
            {
                string txt = TXclient.Text;
                List<string> autoList = new List<string>();
                autoList.Clear();
                if (MainWindow.repairs != null)
                {
                    for (int i = 0; i < MainWindow.repairs.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(TXclient.Text))
                        {
                            //contains срабатывает как startwith, почему?
                            if (MainWindow.repairs[i].Client.ToLower().Contains(TXclient.Text.ToLower()))
                            {
                                bool ok = true;
                                //Проверяем есть ли уже в списке
                                if (autoList != null)
                                {
                                    for (int y = 0; y < autoList.Count; y++)
                                    {
                                        if (MainWindow.repairs[i].Client == autoList[y])
                                        {
                                            ok = false;
                                            break;
                                        }
                                    }
                                }
                                //Если нету - добавляем
                                if (ok)
                                {
                                    autoList.Add(MainWindow.repairs[i].Client);
                                }
                            }
                        }
                    }
                }
                TXclient.ItemsSource = autoList;
                TXclient.PopulateComplete();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка при вводе имени клиента");
            }
        }
        private void AutoPhone(object sender, PopulatingEventArgs e)
        {
            try
            {
                //string txt = TXphone.Text;
                List<string> autoList = new List<string>();
                //autoList.Clear();
                if (MainWindow.repairs != null)
                {
                    for (int i = 0; i < MainWindow.repairs.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(TXphone.Text))
                        {
                            if (MainWindow.repairs[i].Phone.ToLower().StartsWith(TXphone.Text))
                            {
                                bool ok = true;
                                //Проверяем есть ли уже в списке
                                for (int y = 0; y < autoList.Count; y++)
                                {
                                    if (MainWindow.repairs[i].Client == autoList[y])
                                    {
                                        ok = false;
                                        break;
                                    }
                                }
                                //Если нету - добавляем
                                if (ok)
                                {
                                    autoList.Add(MainWindow.repairs[i].Phone);
                                }
                            }
                        }
                    }
                }
                TXphone.ItemsSource = autoList;
                TXphone.PopulateComplete();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка при вводе имени клиента");
            }
        }
        private void ClickAdd(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TXbreak.Text == "" || TXclient.Text == "" || TXexternal.Text == "" || TXkomplekt.Text == "" || TXmanufact.Text == "" || TXmodel.Text == "" || TXphone.Text == "" || TXpreCost.Text == "" || TXprePay.Text == "" || TXserial.Text == "" || TXtype.Text == "")
                {
                    MessageBox.Show("Все поля должны быть заполнены!!!");
                }
                else
                {
                    dev.Date=DateTime.Now ;
                    dev.Client=TXclient.Text ;
                    dev.Phone=TXphone.Text ;
                    dev.Type=TXtype.Text ;
                    dev.Manufact=TXmanufact.Text ;
                    dev.Model=TXmodel.Text ;
                    dev.Serial=TXserial.Text ;
                    dev.Break=TXbreak.Text ;
                    dev.Komplekt=TXkomplekt.Text ;
                    dev.External=TXexternal.Text ;
                    dev.PrePay=Convert.ToInt32(TXprePay.Text);
                    dev.PreCost=Convert.ToInt32(TXpreCost.Text);
                    Program.WriteToFile(dev);
                    checkAndAdd(TXtype.Text, "InType.txt");
                    checkAndAdd(TXmanufact.Text, "InManufact.txt");
                    checkAndAdd(TXbreak.Text, "InBreak.txt");
                    Program.Print(dev);
                    MainWindow.repairs.Insert(0, dev);

                    this.Close();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка при добавлении ремонта");
            }
        }
        private void ClickSave(object sender, RoutedEventArgs e)
        {
            dev.Cost = Convert.ToInt32(TXcost.Text);
            Program.WriteToFile(dev, Program.SearchInFile(dev.ID));
            MainWindow.repairs.RemoveAt(indexBuf);
            MainWindow.repairs.Insert(indexBuf, dev);
            this.Close();
        }
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }
        private void previewTilde(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.OemPipe:
                case Key.OemTilde:
                    e.Handled = true;
                    break;
            }

        }
        private void previewSpace(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.OemPipe:
                case Key.OemTilde:
                case Key.Space:
                    e.Handled = true;
                    break;
            }
        }
        private void pressKeyInAddOrChange(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.OemPipe:
                case Key.OemTilde:
                    e.Handled = true;
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }
    }
}
