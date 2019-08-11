using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
using System.Collections.ObjectModel;

namespace MyProgaWPF
{
    public partial class MainWindow : Window
    {
        public static ObservableCollection<device> repairs = new ObservableCollection<device>(Program.ReadFromFile());
        public static ObservableCollection<device> finding = new ObservableCollection<device>();
        public MainWindow()
        {
           // MessageBox.Show(repairs[0].ID);
            try
            {
                InitializeComponent();
                listForXAML.ItemsSource = repairs;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка в главном окне");
            }
        }
        public void Add()
        {
            AddorChangeRepair repair = new AddorChangeRepair();
            repair.Show();
            listForXAML.Items.Refresh();
        }
        private void Giveback()
        {
            if (repairs.Count == 0)
            {
                MessageBox.Show("Нет ни одного ремонта");
            }
            else if (listForXAML.SelectedIndex == -1)
            {
                MessageBox.Show("Не выбран ремонт");
            }
            else
            {
                if (repairs[listForXAML.SelectedIndex].Status == 0 )
                {
                    AddorChangeRepair repair = new AddorChangeRepair(repairs[listForXAML.SelectedIndex], listForXAML.SelectedIndex, true);
                    repair.Show();
                    repair.Title = "Укажите стоимость ремонта";
                    repairs[listForXAML.SelectedIndex].Status=1;
                }
                else if (repairs[listForXAML.SelectedIndex].Status == 1)
                {
                    repairs[listForXAML.SelectedIndex].Status=2;
                }
                else if (repairs[listForXAML.SelectedIndex].Status == 2)
                {
                    repairs[listForXAML.SelectedIndex].Status=0;
                }
                Program.WriteToFile(repairs[listForXAML.SelectedIndex], Program.SearchInFile(repairs[listForXAML.SelectedIndex].ID));
            }
            listForXAML.Items.Refresh();
        }
        private void Change()
        {
            if (repairs.Count == 0)
            {
                MessageBox.Show("Нет ни одного ремонта");
            }
            else if (listForXAML.SelectedIndex == -1)
            {
                MessageBox.Show("Не выбран ремонт");
            }
            else
            {
                AddorChangeRepair change = new AddorChangeRepair(repairs[listForXAML.SelectedIndex], listForXAML.SelectedIndex,false);
                change.Show();
            }
        }
        private void Print()
        {
            if (repairs.Count == 0)
            {
                MessageBox.Show("Нет ни одного ремонта");
            }
            else if (listForXAML.SelectedIndex == -1)
            {
                MessageBox.Show("Не выбран ремонт");
            }
            else
            {
                Program.Print(repairs[listForXAML.SelectedIndex]);
            }
        }
        private void Searching(string forSearch)
        {
            if (!string.IsNullOrEmpty(txtForSearch.Text))
            {
                finding.Clear();
                for (int i = 0; i < repairs.Count; i++)
                {
                    if (repairs[i].ID.Contains(txtForSearch.Text) || repairs[i].Manufact.ToLower().Contains(txtForSearch.Text.ToLower()) || repairs[i].Model.ToLower().Contains(txtForSearch.Text.ToLower()) || repairs[i].Break.ToLower().Contains(txtForSearch.Text.ToLower()) || repairs[i].Phone.ToLower().Contains(txtForSearch.Text.ToLower()) || Convert.ToString(repairs[i].Date).Contains(txtForSearch.Text.ToLower()) || repairs[i].Serial.ToLower().Contains(txtForSearch.Text.ToLower()) || Convert.ToString(repairs[i].PrePay).Contains(txtForSearch.Text) || Convert.ToString(repairs[i].PreCost).Contains(txtForSearch.Text) || Convert.ToString(repairs[i].Cost).Contains(txtForSearch.Text) || repairs[i].Client.ToLower().Contains(txtForSearch.Text.ToLower()) || repairs[i].Comment.ToLower().ToLower().Contains(txtForSearch.Text.ToLower()))
                    {
                        finding.Add(repairs[i]);
                    }
                }
                if (finding.Count() == 0)
                {
                    MessageBox.Show("Ничего не найдено");
                }
                else
                {
                    listForXAML.ItemsSource = finding;
                }
            }
            else
            {
                listForXAML.ItemsSource = repairs;
            }
            listForXAML.Focus();
        }
        private void Searching(int Status)
        {
            finding.Clear();
            for (int i = 0; i < repairs.Count; i++)
            {
                if (repairs[i].Status == Status)
                {
                    finding.Add(repairs[i]);
                }
            }
            listForXAML.ItemsSource = finding;
            listForXAML.Focus();
        }
        private void ClickAddRepair(object sender, RoutedEventArgs e)
        {
            Add();
        }
        private void ClickDone(object sender, RoutedEventArgs e)
        {
            Giveback();
        }
        private void ClickPrint(object sender, RoutedEventArgs e)
        {
            Print();
        }
        private void ClickChange(object sender, RoutedEventArgs e)
        {
            Change();
        }
        private void ClickSet(object sender, RoutedEventArgs e)
        {
            Settings set = new Settings();
            set.Show();
        }
        private void ClickSearch(object sender, RoutedEventArgs e)
        {
            Searching(txtForSearch.Text);
        }
        private void PressKeyInMainWindow(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                {
                    switch (e.Key)
                    {
                        case Key.D1:
                            finding.Clear();
                            txtForSearch.Clear();
                            listForXAML.ItemsSource = repairs;
                            break;
                        case Key.D2:
                            Searching(1);
                            break;
                        case Key.D3:
                            Searching(2);
                            break;
                        case Key.D4:
                            Searching(0);
                            break;
                    }
                }
                else
                {
                    switch (e.Key)
                    {
                        case Key.F1:
                            Add();
                            break;
                        case Key.F2:
                            Giveback();
                            break;
                        case Key.F3:
                            Change();
                            break;
                        case Key.Enter:
                            Searching(txtForSearch.Text);
                            break;
                        case Key.Escape:
                            finding.Clear();
                            txtForSearch.Clear();
                            listForXAML.ItemsSource = repairs;
                            break;
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка при обработке нажатия клавиш");
            }
        }
        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Change();
        }
        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            finding.Clear();
            txtForSearch.Clear();
            listForXAML.ItemsSource = repairs;
        }
        private void ClickFinished(object sender, RoutedEventArgs e)
        {
            Searching(1);
        }
        private void ClickNotFinished(object sender, RoutedEventArgs e)
        {
            Searching(0);
        }
        private void ClickIssued(object sender, RoutedEventArgs e)
        {
            Searching(2);
        }
    }
}