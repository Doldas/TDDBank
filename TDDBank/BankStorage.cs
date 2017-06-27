using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDBank
{
    public class BankStorage
    {
        private List<Account> accounts = new List<Account>();

        public int Count { get { return accounts.Count(); } }
        //Add method
        public void Add(Account account)
        {
            /* Old Version
            if (account.ClearingNumber != null)
            {
                bool Exists = false;
                foreach (var a in accounts)
                {
                    if (a.Equal(account) == true)
                    {
                        Exists = true;
                        break;
                    }
                }
                if (Exists == false)
                {
                    accounts.Add(account);
                }
            }
            */
                //Refractor - New Version
            if (account.ClearingNumber != null)
            {
                var a = Get(account.ClearingNumber) as Account;
                if (a == null)
                {
                    accounts.Add(account);
                }
            }
        }
    public Account Get(string clearingNumber)
    {
        return accounts.SingleOrDefault(a => a.ClearingNumber == clearingNumber);
    }
    public List<Account> Get()
    {
        return accounts;
    }
    public List<Account> GetAccounts(string FullName)
    {
        return accounts.Where(a => a.User.FullName == FullName).ToList();
    }
    public List<Transaction> GetTransactions(string ClearingNumber)
    {
        return Get(ClearingNumber).Transactions;
    }
    public Transaction GetTransaction(int id)
    {
        foreach(Account account in accounts)
        {
            Transaction a = account.Transactions.Where(t=>t.ID==id).SingleOrDefault();
            if(a!=null)
            {
                return a;
            }
        }
        return null;
    }
    public void Transfer(string cNr,string cNr2, double amount)
    {
        var a = Get(cNr) as Account;
        var b = Get(cNr2) as Account;

        if (a != null && b != null)
        {
            if (a.Balance>=amount)
            {
                if (a.WithdrawLock != true && b.WithdrawLock != true)
                {

                    int TotalTransactions=0;
                    foreach(Account ac in accounts)
                    {
                        TotalTransactions += ac.Transactions.Count;
                    }
                    
                    a.Balance -= amount;
                    Transaction tA = new Transaction { ID=TotalTransactions+=1,TransactionID = a.Transactions.Count, Account = a, Account_ID = a.AccountID, Category = Category.Payment, Money = amount, Reciever = b, TransactionDate = DateTime.Now };
                    a.Transactions.Add(tA);

                    b.Balance += amount;

                    b.Transactions = new List<Transaction>();
                    Transaction tB = new Transaction { ID = TotalTransactions += 1, TransactionID = a.Transactions.Count, Account = b, Account_ID = b.AccountID, Category = Category.Added, Money = amount, Reciever = a, TransactionDate = DateTime.Now };
                    a.Transactions.Add(tB);
                }
            }
        }
    }
    public void AddMagicalMoney(string clearingNr,double Money)
    {
        //Get(clearingNr).Balance+=100;

        //Get(clearingNr).Balance += Money;
        if (Money <= 200000)
        {
            Get(clearingNr).Balance += Money;
        }
    }
    }
}
