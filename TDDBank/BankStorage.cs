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
    public Transaction GetTransaction(int transactionID)
    {
        foreach(Account account in accounts)
        {
            Transaction a = account.Transactions.Where(t=>t.TransactionID==transactionID).SingleOrDefault();
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
                    a.Balance -= amount;
                    b.Balance += amount;
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
