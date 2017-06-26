using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDBank
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Money { get; set; }
        public Category Category { get; set; }
        public int Account_ID { get; set; }
        public Account Account { get; set; }
        public Account Reciever { get; set; }
    }
    public enum Category
    {
        Added,
        Payment
    }
}
