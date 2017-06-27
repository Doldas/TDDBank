using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDBank
{
    public class Account
    {
       public int AccountID { get; set; }
       public string ClearingNumber { get; set; }
       public double Balance { get; set; }
       public bool WithdrawLock { get; set; }
       //One Account can only have one User, but a User can have multiple Accounts 1 - 0..* Relation
       public int User_ID { get; set; }
       public User User { get; set; }
       //One  Account can have Many Transactions and a Transaction can only have one account  1..* - 1 Relation
       public List<Transaction> Transactions { get; set; }

       public Account()
       {
           WithdrawLock = false;
       }
       public bool Equal(Account other)
       {
           return this.AccountID == other.AccountID;
       }
    }
}
