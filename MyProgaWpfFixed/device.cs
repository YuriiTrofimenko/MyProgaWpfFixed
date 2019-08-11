using MyProgaWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace MyProgaWPF
{
    public class device
    {
        private string ID_ = "#";
        private string type_ = "Другое";
        private string manufact_ { get; set; } = "unknown";
        private string model_ { get; set; } = "no_model";
        private string break_ { get; set; } = "Все плохо";
        private string phone_ { get; set; } = "no_phone";
        private DateTime date_ { get; set; }
        string serial_ { get; set; } = "no_serial";
        string komplekt_ { get; set; } = "Без комплекта";
        string external_ { get; set; } = "Потертости";
        private int prePay_ { get; set; } = 0;
        private int preCost_ { get; set; } = 0;
        private int cost_ { get; set; } = 0;
        private string client_ { get; set; } = "Иванов Иван Иваныч";
        int status_ { get; set; } = 0;
        private string comment_ { get; set; } = "no comment";
        private string whatIsDone_ { get; set; } = "nothing";

        // Prop Delegate Sample
        public delegate string whatIsDoneDelegate(string value);
        private whatIsDoneDelegate widd1;

        public device()
        {
            widd1 += (s) => { return whatIsDone_ = s; };
            Console.WriteLine(widd1("TestText"));
        }
        public device(string id, string STATUS,string TYPE,string MANUFACT, string MODEL, string BREAK, string PHONE, string SERIAL, string KOMPLEKT, string EXTERNAL, string PREPAY, string PRECOST, string CLIENT, string DATE, string COST, string COMMENT, string WHATISDONE)
        {
            ID = id;
            Type = TYPE;
            Status = Convert.ToInt32(STATUS);
            Manufact = MANUFACT;
            Model = MODEL;
            Break = BREAK;
            Phone = PHONE;
            Serial=SERIAL;
            Komplekt=KOMPLEKT;
            External= EXTERNAL;
            PrePay = Convert.ToInt32(PREPAY);
            PreCost = Convert.ToInt32(PRECOST);
            Client = CLIENT;
            DateTime dateBuf;
            DateTime.TryParse(DATE, out dateBuf);
            Date = dateBuf;
            Cost = Convert.ToInt32(COST);
            Comment = COMMENT;
            WhatIsDone = WHATISDONE;
        }
        ~device() { }

        public string ID
        {
            get { return ID_; }
            set { ID_ = value; }
        }
        public string Type
        {
            get { return type_; }
            set { type_ = value; }
        }
        public string Manufact
        {
            get { return manufact_; }
            set { manufact_ = value; }
        }
        public string Model
        {

            get { return model_; }
            set { model_ = value; }
        }
        public string Break
        {
            get { return break_; }
            set { break_ = value; }
        }
        public string Phone
        {
            get { return phone_; }
            set { phone_ = value; }
        }
        public DateTime Date
        {
            get { return date_; }
            set { date_ = value; }
        }
        public string Serial
        {
            get { return serial_; }
            set { serial_ = value; }
        }
        public string Komplekt
        {
            get { return komplekt_; }
            set { komplekt_ = value; }
        }
        public string External
        {
            get { return external_; }
            set { external_ = value; }
        }
        public int PrePay
        {
            get { return prePay_; }
            set { prePay_ = value; }
        }
        public int PreCost
        {
            get { return preCost_; }
            set { preCost_ = value; }
        }
        public string Client
        {
            get { return client_; }
            set { client_ = value; }
        }
        public int Status
        {
            get { return status_; }
            set { status_ = value; }
        }
        public int Cost
        {
            get { return cost_; }
            set { cost_ = value; }
        }
        public string Comment
        {
            get { return comment_; }
            set { comment_ = value; }
        }
        public string WhatIsDone
        {
            get { return whatIsDone_; }
            set { whatIsDone_ = value; }
        }
    }
}