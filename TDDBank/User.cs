using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDBank
{
    public class User
    {
        public int UserID { get; set; }
        public string FullName {get; set;}
        public string Contact { get; set; }
        public int CountAccounts { get { return Accounts.Count(); } }
        public List<Account> Accounts { get; set; }
    }
}
