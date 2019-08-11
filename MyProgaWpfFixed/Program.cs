using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace MyProgaWPF
{
    class Program
    {
        public static int SearchInFile(string whatSearch)
        {
		/*
-Не могу разобраться как печатать на принтер.
-Возможно ли установить делегат на свойство класса?
-по каким причинам программа может произвольно ребутиться(на моем ноуте работает без проблем, на 2-х компах проверял-ребутится)?
-как установить фокус на ввод в combobox?

*/

            int poz = -1;
            try
            {
                string bufLine;
                StreamReader fromFile = new StreamReader("repairs.txt");
                bufLine = fromFile.ReadToEnd();
                poz = bufLine.IndexOf(whatSearch);
                fromFile.Close();
            }
            catch (Exception er)
            {
                System.Windows.MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка поиска в файле");
            }
            return poz;
        }
        public static List<device> ReadFromFile()
        {
            List<device> devList = new List<device>();
            try
            {
                //Если файла нет - создать
                if (!File.Exists("repairs.txt"))
                {
                    FileStream file = File.Create("repairs.txt");
                    file.Close();
                }
                //Иначе открываем файл
                else
                {
                    StreamReader fromFile = new StreamReader("repairs.txt", System.Text.Encoding.Default);
                    //Если файл пустой - закрываем без чтения
                    if (fromFile.Peek() == -1)
                    {
                        fromFile.Close();
                    }
                    //Если есть записи - считываем
                    else
                    {
                        const int length = 17;
                        int y = 0;
                        string bufLine, buf = "";
                        string[] buffer = new string[17];
                        while ((bufLine = fromFile.ReadLine()) != null)
                        {
                            for (int i = 0; y < length; i++)
                            {
                                //Посимвольно считываем файл пока не встретим разделитель
                                if (bufLine[i] != '|' && bufLine[i]!='~')
                                {
                                    buf += bufLine[i];
                                }
                                //После встречи разделителя записываем объект в буферный массив
                                else
                                {
                                    //MessageBox.Show(buf);
                                    buffer[y++]=buf;
                                    buf = "";
                                    if (bufLine[i]=='~')
                                    {
                                        y = length;
                                        break;
                                    }
                                    //Перепрыгиваем разделитель
                                    i++;
                                }
                            }
                            y = 0;
                            device bufObj = new device(buffer[0], buffer[1], buffer[2], buffer[3], buffer[4], buffer[5], buffer[6], buffer[7], buffer[8], buffer[9], buffer[10], buffer[11], buffer[12], buffer[13], buffer[14], buffer[15], buffer[16]);
                            //MessageBox.Show(bufObj.ID);
                            devList.Insert(0, bufObj);
                        }
                        fromFile.Close();
                    }
                }
            }
            catch (Exception er)
            {
                System.Windows.MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка считывания ремонтов из файла");
            }
            return devList;
        }
        public static void WriteToFile(device dev, int poz = -1)
        {
            try
            {
                using (FileStream toFile = new FileStream("repairs.txt", FileMode.Open))
                {
                    byte[] textToByte;
                    //Если не передано координат для записи
                    if (poz == -1)
                    {
                        toFile.Seek(0, SeekOrigin.End);
                        //Если уже есть записи - записать знак перехода на новую строку
                        if (toFile.Length > 0)
                        {
                            textToByte = System.Text.Encoding.Default.GetBytes(Environment.NewLine);
                            toFile.Write(textToByte, 0, textToByte.Length);
                        }
                    }
                    //Если переданы координаты для записи - установить курсор
                    else
                    {
                        toFile.Seek(poz, SeekOrigin.Begin);
                    }
                    //Записать объект в файл
                    textToByte = System.Text.Encoding.Default.GetBytes(dev.ID + "| " + Convert.ToInt32(dev.Status) + "| " + dev. Type + "| " + dev. Manufact + "| " + dev. Model + "| " + dev. Break + "| " + dev. Phone + "| " + dev. Serial + "| " + dev. Komplekt + "| " + dev. External + "| " + dev. PrePay + "| " + dev. PreCost + "| " + dev. Client + "| " + dev. Date + "| " + dev.Cost + "| " + dev.Comment+ "| " + dev.WhatIsDone + "~                                               ");
                    toFile.Write(textToByte, 0, textToByte.Length);
                }
            }
            catch (Exception er)
            {
                System.Windows.MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка записи ремонта в файл");
            }
        }
        // static void aboutRepair(device dev)
        static void AboutRepair(device dev, StringBuilder htmlText)
        {
            try
            {
                /* StreamWriter toFile = new StreamWriter("receipt.html", true, System.Text.Encoding.Default);
                toFile.Write("<table><tr><th><b>Квитанция о приеме оборудования на ремонт</b></th></tr></table>");
                toFile.Write("<table border =\"1\"><tr><td align =\"right\" width =\"20 %\"><b>Устройство</b></td><td>&nbsp;" + dev. Type + " " + dev. Manufact + " " + dev. Model + "</td></tr>");
                toFile.Write("<tr><td align =\"right\" width =\"20%\"><b>Серийный номер</b></td> <td>&nbsp;" + dev. Serial + " </td> </tr>");
                toFile.Write("<tr><td align =\"right\" width =\"20 %\"><b>Комплектность</b></td><td>&nbsp;" + dev. Komplekt + "</td></tr>");
                toFile.Write("<tr><td align =\"right\" width =\"20 %\"><b>Внешний вид</b></td><td>&nbsp;" + dev. External + "</td></tr>");
                toFile.Write("<tr><td align =\"right\" width =\"20 %\"><b>Предоплата</b></td><td>&nbsp;");
                if (dev. PrePay > 0)
                {
                    toFile.Write(dev.PrePay);
                }
                else
                {
                    toFile.WriteLine("Без предоплаты");
                }
                toFile.Write("</td></tr>");
                toFile.Write("<tr><td align =\"right\" width =\"20 %\"><b>Предварительная стоимость</b></td><td>&nbsp;");
                if (dev. PreCost > 0)
                {
                    toFile.WriteLine(dev. PreCost);
                }
                else
                {
                    toFile.WriteLine("После диагностики");
                }
                toFile.Write("</td></tr>");
                toFile.Write("<tr><td align =\"right\" width =\"20 %\"><b>Клиент</b></td><td>&nbsp;" + dev. Client + "</td></tr>");
                toFile.Write("<tr><td align =\"right\" width =\"20 %\"><b>Телефон клиента</b></td><td>&nbsp;" + dev. Phone + "</td></tr>");
                toFile.Write("<tr><td align =\"right\" width =\"20 %\"><b>Дата приема</b></td><td>&nbsp;" + dev. Date + "</td></tr>");
                toFile.WriteLine("<table><tr><th><b>Неисправность со слов клиента</b></th></tr></table>");
                toFile.WriteLine("<table border =\"1\"><tr align =\"center\"><td><br>" + dev. Break + "<br><br></td></tr></table>");
                toFile.Close(); */

                htmlText.Append("<table><tr><th><b>Квитанция о приеме оборудования на ремонт</b></th></tr></table>");
                htmlText.Append("<table border =\"1\"><tr><td align =\"right\" width =\"20 %\"><b>Устройство</b></td><td>&nbsp;" + dev.Type + " " + dev.Manufact + " " + dev.Model + "</td></tr>");
                htmlText.Append("<tr><td align =\"right\" width =\"20%\"><b>Серийный номер</b></td> <td>&nbsp;" + dev.Serial + " </td> </tr>");
                htmlText.Append("<tr><td align =\"right\" width =\"20 %\"><b>Комплектность</b></td><td>&nbsp;" + dev.Komplekt + "</td></tr>");
                htmlText.Append("<tr><td align =\"right\" width =\"20 %\"><b>Внешний вид</b></td><td>&nbsp;" + dev.External + "</td></tr>");
                htmlText.Append("<tr><td align =\"right\" width =\"20 %\"><b>Предоплата</b></td><td>&nbsp;");
                if (dev.PrePay > 0)
                {
                    htmlText.Append(dev.PrePay);
                }
                else
                {
                    htmlText.Append("Без предоплаты");
                }
                htmlText.Append("</td></tr>");
                htmlText.Append("<tr><td align =\"right\" width =\"20 %\"><b>Предварительная стоимость</b></td><td>&nbsp;");
                if (dev.PreCost > 0)
                {
                    htmlText.Append(dev.PreCost);
                }
                else
                {
                    htmlText.Append("После диагностики");
                }
                htmlText.Append("</td></tr>");
                htmlText.Append("<tr><td align =\"right\" width =\"20 %\"><b>Клиент</b></td><td>&nbsp;" + dev.Client + "</td></tr>");
                htmlText.Append("<tr><td align =\"right\" width =\"20 %\"><b>Телефон клиента</b></td><td>&nbsp;" + dev.Phone + "</td></tr>");
                htmlText.Append("<tr><td align =\"right\" width =\"20 %\"><b>Дата приема</b></td><td>&nbsp;" + dev.Date + "</td></tr>");
                htmlText.Append("<table><tr><th><b>Неисправность со слов клиента</b></th></tr></table>");
                htmlText.Append("<table border =\"1\"><tr align =\"center\"><td><br>" + dev.Break + "<br><br></td></tr></table>");
            }
            catch (Exception er)
            {
                System.Windows.MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка при создании квитанции (описание ремонта)");
            }
        }
        // static void termOfService()
        static void TermOfService(StringBuilder htmlText)
        {
            try
            {
                /*StreamWriter toFile = new StreamWriter("receipt.html", true, System.Text.Encoding.Default);
                toFile.Write("<style>table{width: 100% ;border-collapse:collapse;}th{align:center;background:#ccc;}</style>");
                toFile.Write("<table><tr><th><b>Условия обслуживания</b></th></tr></table>");
                toFile.Write("<table border =\"1\"><tr><td> <font size =\"3\">");
                toFile.Write("-Мастерская устраняет только заявленную неисправность со слов клиента.<br>");
                toFile.Write("-Сроки ремонта устанавливаются  в зависимости от наличия запчастей и сложности выполнения работ.<br>");
                toFile.Write("-Оборудование с согласия клиента принято без разборки и проверки неисправностей.");
                toFile.Write("Повреждения, которые не были указаны при приёме оборудования считаются, что они были в аппарате до его сдачи в ремонт.<br>");
                toFile.Write("-Установленные узлы или расходные материалы возврату не подлежат.<br>");
                toFile.Write("-Гарантия предоставляется только на проделанную работу и только на замененные запчасти.<br>");
                toFile.Write("-Гарантия не распространяется на : <br>");
                toFile.Write("&nbsp&nbsp - программное обеспечение; <br>");
                toFile.Write("&nbsp&nbsp - аппараты, которые подвергались воздействию влаги, механическим повреждениям; <br>");
                toFile.Write("<b>-В случае отказа от ремонта оплачивается диагностика</b></font></td></tr></table>");
                toFile.Close();*/

                htmlText.Append("<style>table{width: 100% ;border-collapse:collapse;}th{align:center;background:#ccc;}</style>");
                htmlText.Append("<table><tr><th><b>Условия обслуживания</b></th></tr></table>");
                htmlText.Append("<table border =\"1\"><tr><td> <font size =\"3\">");
                htmlText.Append("-Мастерская устраняет только заявленную неисправность со слов клиента.<br>");
                htmlText.Append("-Сроки ремонта устанавливаются  в зависимости от наличия запчастей и сложности выполнения работ.<br>");
                htmlText.Append("-Оборудование с согласия клиента принято без разборки и проверки неисправностей.");
                htmlText.Append("Повреждения, которые не были указаны при приёме оборудования считаются, что они были в аппарате до его сдачи в ремонт.<br>");
                htmlText.Append("-Установленные узлы или расходные материалы возврату не подлежат.<br>");
                htmlText.Append("-Гарантия предоставляется только на проделанную работу и только на замененные запчасти.<br>");
                htmlText.Append("-Гарантия не распространяется на : <br>");
                htmlText.Append("&nbsp&nbsp - программное обеспечение; <br>");
                htmlText.Append("&nbsp&nbsp - аппараты, которые подвергались воздействию влаги, механическим повреждениям; <br>");
                htmlText.Append("<b>-В случае отказа от ремонта оплачивается диагностика</b></font></td></tr></table>");
            }
            catch (Exception er)
            {
                System.Windows.MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка при создании квитанции (условия ремонта)");
            }
        }
        public static void Print(device dev)
        {
            try
            {
                //Если файл существует - затираем
                /* StreamWriter toFile = new StreamWriter("receipt.html", false, System.Text.Encoding.Default);
                List<string> bufSet = Settings.readSettings();
                toFile.Write("<!DOCTYPE HTML><html><body><b><font size =\"5\">\"" + bufSet[0] + "\"</font><br>т.&nbsp<font size =\"4\">" + bufSet[1] + "</font> <br>" + bufSet[2] + " <br>" + bufSet[3] + "<br> <br></b>");
                toFile.Close();
                aboutRepair(dev);
                termOfService();
                aboutRepair(dev); */

                List<string> bufSet = Settings.readSettings();
                StringBuilder htmlText = new StringBuilder("<!DOCTYPE HTML><html><body><b><font size =\"5\">\"" + bufSet[0] + "\"</font><br>т.&nbsp<font size =\"4\">" + bufSet[1] + "</font> <br>" + bufSet[2] + " <br>" + bufSet[3] + "<br> <br></b>");
                AboutRepair(dev, htmlText);
                TermOfService(htmlText);
                AboutRepair(dev, htmlText);
                WebBrowser webBrowserForPrinting = new WebBrowser();
                webBrowserForPrinting.DocumentCompleted +=
                    new WebBrowserDocumentCompletedEventHandler(PrintDocument);
                webBrowserForPrinting.DocumentText = htmlText.ToString();
            }
            catch (Exception er)
            {
                System.Windows.MessageBox.Show(er.ToString() + Environment.NewLine + "Ошибка при печати документа");
            }
        }

        private static void PrintDocument(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Print the document now that it is fully loaded.
            ((WebBrowser)sender).Print();

            // Dispose the WebBrowser now that the task is complete. 
            ((WebBrowser)sender).Dispose();
        }
    }
}
