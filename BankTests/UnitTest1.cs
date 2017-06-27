using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDDBank;
using System.Collections.Generic;
namespace BankTests
{
    [TestClass]
    public class BankTests
    {
        //Account Class Testing
        #region Account Class - Instance Test
        [TestMethod]
        public void CreateNewAccount_Given_All_Properties_Result_The_Same_Properties()
        {            
            //Arrange
            Account account;
            //Act
            account = new Account { AccountID = 1, ClearingNumber = "NNNN-NNNN-NNNN-XXXX", User = new User { FullName = "Kalle Anka", UserID = 1, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 1, Balance = 373.54 };
            //Assert
            Assert.AreEqual("Kalle Anka", account.User.FullName);
            Assert.AreEqual("NNNN-NNNN-NNNN-XXXX", account.ClearingNumber);
            Assert.AreEqual(373.54, account.Balance);
        }
        #endregion

        #region Class Relations For Models
        [TestMethod]
        public void Account_Relations_With_Transactions_Results_Working_Relations()
        {
            //Arrange
            Account account = new Account { AccountID = 1, ClearingNumber = "GGGG-GGGG-GGGG-QQQ1", WithdrawLock = false, Balance = 345.78, Transactions = new List<Transaction>() };
            Account account2 = new Account { AccountID = 2, ClearingNumber = "GGGG-GGGG-GGGG-QQQ2", WithdrawLock = false, Balance = 12343.34, Transactions = new List<Transaction>() };
           
            Transaction transaction = new Transaction { TransactionID = 1, TransactionDate=DateTime.Now, Money=100, Category=Category.Payment, Account_ID=1, Account=account, Reciever=account2 };
            Transaction transaction2 = new Transaction { TransactionID = 2, TransactionDate = DateTime.Now, Money = 450, Category = Category.Payment, Account_ID = 1, Account = account };
            Transaction transaction3= new Transaction { TransactionID = 3, TransactionDate = DateTime.Now, Money = 125, Category = Category.Added, Account_ID = 1, Account = account };
            Transaction transaction4 = new Transaction { TransactionID = 4, TransactionDate = DateTime.Now, Money = 89, Category = Category.Payment, Account_ID = 1, Account = account };

            Transaction transaction5 = new Transaction { TransactionID = 5, TransactionDate = DateTime.Now, Money = 100, Category = Category.Added, Account_ID = 2, Account = account2, Reciever = account };
            //Act
            account.Transactions.Add(transaction);
            account.Transactions.Add(transaction2);
            account.Transactions.Add(transaction3);
            account.Transactions.Add(transaction4);

            account2.Transactions.Add(transaction5);
            //Assert
            Assert.AreEqual(Category.Payment,account.Transactions.Find(t=>t.TransactionID==1).Category);
            Assert.AreEqual(89, account.Transactions.Find(t => t.TransactionID == 4).Money);
            Assert.AreEqual(4, account.Transactions.Count);

            Assert.AreEqual(1, account2.Transactions.Count);
            Assert.AreEqual(Category.Added, account2.Transactions.Find(t => t.TransactionID == 5).Category);
        }
        [TestMethod]
        public void Account_Relations_To_User_Results_Working_Relations()
        {
            //Arrange
            User user = new User { FullName="Gommash", Contact="Test@Email.com", UserID=1, Accounts=new List<Account>()};
            User user2 = new User { FullName = "Doldas", Contact = "Doldas@Email.com", UserID = 2, Accounts=new List<Account>()};
            Account account_user1 = new Account { AccountID=1, ClearingNumber="GGGG-GGGG-GGGG-QQQ1", WithdrawLock=false, Balance=345.78, User_ID=1, User=user };
            Account account2_user1 = new Account { AccountID = 2, ClearingNumber = "GGGG-GGGG-GGGG-QQQ3", WithdrawLock = true, Balance = 12343.34, User_ID = 1, User = user };
            Account account3_user2 = new Account { AccountID = 3, ClearingNumber = "GGGG-GGGG-GGGG-QQQ4", WithdrawLock = true, Balance = 3043.34, User_ID = 2, User = user2 };
            
            //Act
            //Data-Binding
            //User 1
            user.Accounts.Add(account_user1);
            user.Accounts.Add(account2_user1);
            //User 2
            user2.Accounts.Add(account3_user2);
           
            //Assert
            //User1 - Gommash
            Assert.AreEqual(12343.34, user.Accounts.Find(a => a.AccountID==2).Balance);
            Assert.AreEqual(2, user.Accounts.Count);
            Assert.AreEqual("Gommash", account_user1.User.FullName);
            Assert.AreEqual("Gommash", account2_user1.User.FullName);
            //User2 - Doldas
            Assert.AreEqual(3043.34, user2.Accounts.Find(a => a.AccountID==3).Balance);
            Assert.AreEqual(1, user2.Accounts.Count);
            Assert.AreEqual("Doldas", account3_user2.User.FullName);

        }
        #endregion

        #region Equal Method
        [TestMethod]
        public void EqualMethod_Given_1UniqueAccount_To_Another_Account_Result_FalseValue()
        {
            //Arrange
            Account account = new Account { AccountID = 1, ClearingNumber = "NNNN-NNNN-NNNN-XXXX", User = new User { FullName = "Kalle Anka", UserID = 1, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 1, Balance = 0 };
            Account account2 = new Account { AccountID = 2, ClearingNumber = "NNNN-NNNN-NNNN-3333", User = new User { FullName = "Kalle Anka2", UserID = 2, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 2, Balance = 0 };
            bool ExpectedResult = false;
            //Act
            bool ActualResult = account.Equal(account2);
            //Assert
            Assert.AreEqual(ExpectedResult, ActualResult);
        }
        [TestMethod]
        public void EqualMethod_Given_2DublicatedAccounts_Result_TrueValue()
        {
            //Arrange
            Account account = new Account { AccountID = 1, ClearingNumber = "NNNN-NNNN-NNNN-XXXX", User = new User { FullName = "Kalle Anka", UserID = 1, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 1, Balance = 0 };
            Account account2 = new Account { AccountID = 1, ClearingNumber = "NNNN-NNNN-NNNN-XXXX", User = new User { FullName = "Kalle Anka", UserID = 1, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 1, Balance = 0 };

            bool ExpectedResult = true;
            //Act
            bool ActualResult = account.Equal(account2);
            //Assert
            Assert.AreEqual(ExpectedResult, ActualResult);
        }
        #endregion

        //AccountStorage Class Tests
        #region Add Account
        [TestMethod]
        public void AddAccount_Given_1Account_Result_1Added()
        {
            //Arrange
            BankStorage bankStorage = new BankStorage();
            Account account = new Account { AccountID = 2, ClearingNumber = "NNNN-NNNN-NNNN-4666", User = new User { FullName = "Kalle Anka3", UserID = 2, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 2, Balance = 0 };
            int ExpectedResult = 1;
            //Act
            bankStorage.Add(account);
            int ActualResult = bankStorage.Count;
            //Assert
            Assert.AreEqual(ExpectedResult,ActualResult);
        }
        [TestMethod]
        public void AddAccount_Given_1Account_Result_1Added_SameTest_As_Above_But_With_A_Much_Safer_Test()
        {
            //Arrange
            BankStorage bankStorage = new BankStorage();
            Account account = new Account { AccountID = 2, ClearingNumber = "NNNN-NNNN-NNNN-4666", User = new User { FullName = "Kalle Anka3", UserID = 2, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 2, Balance = 0 };
            int ExpectedResultBeforeAdded = 0;
            int ExpectedResultAfterAddAccount = 1;
            //Act
            int ActualResultBeforeAdded = bankStorage.Count;
            bankStorage.Add(account);
            int ActualResultAferAddAccount = bankStorage.Count;
            //Assert
            //Test Before Added
            Assert.AreEqual(ExpectedResultBeforeAdded, ActualResultBeforeAdded);
            //Test After Added
            Assert.AreEqual(ExpectedResultAfterAddAccount, ActualResultAferAddAccount);
        }
        [TestMethod]
        public void AddAccount_Given_2Accounts_Result_2Added_To_BankingStorage()
        {
            //Arrange
            BankStorage bankStorage = new BankStorage();
            Account account = new Account { AccountID = 2, ClearingNumber = "NNNN-NNNN-NNNN-3333", User = new User { FullName = "Kalle Anka2", UserID = 2, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 2, Balance = 0 };
            Account account2 = new Account { AccountID = 3, ClearingNumber = "NNNN-NNNN-NNNN-4444", User = new User { FullName = "Farbror JoakimVonAnka", UserID = 3, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 3, Balance = 0 };
            int ExpectedResult = 2;
            //Act
            bankStorage.Add(account);
            bankStorage.Add(account2);
            int ActualResult = bankStorage.Count;
            //Assert
            Assert.AreEqual(ExpectedResult, ActualResult);
        }
        [TestMethod]
        public void AddAccount_Given_1DuplicatedAccount_Result_1Added_To_BankingStorage()
        {
            //Arrange
            BankStorage bankStorage = new BankStorage();
            Account account = new Account { AccountID = 1, ClearingNumber = "NNNN-NNNN-NNNN-XXXX", User = new User { FullName = "Kalle Anka", UserID = 1, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 1, Balance = 0 };
            Account account2 = new Account { AccountID = 1, ClearingNumber = "NNNN-NNNN-NNNN-XXXX", User = new User { FullName = "Kalle Anka", UserID = 1, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 1, Balance = 0 };

            int ExpectedResult = 1;
            //Act
            bankStorage.Add(account);
            bankStorage.Add(account2);
            int ActualResult = bankStorage.Count;
            //Assert
            Assert.AreEqual(ExpectedResult, ActualResult);
        }
        [TestMethod]
        public void AddAccount_Given_1EmptyAccount_Result_0Added_To_BankingStorage()
        {
            //Arrange
            BankStorage bankStorage = new BankStorage();
            Account account = new Account{ AccountID=0, Balance=0, ClearingNumber=null, User=null, User_ID=0};
            int ExpectedResult = 0;
            //Act
            bankStorage.Add(account);
            int ActualResult = bankStorage.Count;
            //Assert
            Assert.AreEqual(ExpectedResult, ActualResult);
        }
        #endregion

        #region Get All Accounts
        [TestMethod]
        public void Get_Given_0_Input_Returns_AllAccounts()
        {
            //Arrange
            BankStorage bankStorage = new BankStorage();
            int ExpectedResult = 0;
            //Act
             int ActualResult = bankStorage.Get().Count;
            //Assert
            Assert.AreEqual(ExpectedResult,ActualResult);
        }
        [TestMethod]
        public void Get_Given_0_Input_Returns_AllAccounts_Given_TestData()
        {
            //Arrange
            BankStorage bankStorage = new BankStorage();
            List<Account> ExpectedResult = new List<Account>();
            int ExpectedCountResult = 3;
            //User 1
            User user = new User { UserID = 1, FullName = "Kalle Anka", Contact = "email@mail.com", Accounts = new List<Account>() };
            Account account = new Account { AccountID = 1, User_ID = 1, User = user, ClearingNumber = "NNNN-NNNN-NNNN-YYYY", Balance = 400, WithdrawLock = false };
            user.Accounts.Add(account);
            //User 2
            User user2 = new User { UserID = 2, FullName = "TestSubject", Contact = "email@mail.com", Accounts = new List<Account>() };
            Account account2 = new Account { AccountID = 2, User_ID = 2, User = user2, ClearingNumber = "NNNN-NNNN-NNNN-ZZZZ", Balance = 0, WithdrawLock = true };
            user2.Accounts.Add(account2);
            //User 3
            User user3 = new User { UserID = 3, FullName = "Doldas", Contact = "email@mail.com", Accounts = new List<Account>() };
            Account account3 = new Account { AccountID = 3, User_ID = 3, User = user3, ClearingNumber = "NNNN-NNNN-NNNN-RRRR", Balance = 424.70, WithdrawLock = false };
            user3.Accounts.Add(account3);
            //AddTobankAccounts
            bankStorage.Add(account);
            bankStorage.Add(account2);
            bankStorage.Add(account3);
            //Act
            List<Account> ActualResult = bankStorage.Get();
            int ActualCountResult = bankStorage.Count;
            //Assert
            Assert.AreEqual("Kalle Anka", ActualResult.Find(a => a.User_ID == 1).User.FullName);
            Assert.AreEqual("TestSubject", ActualResult.Find(a => a.User_ID == 2).User.FullName);
            Assert.AreEqual("Doldas", ActualResult.Find(a=>a.User_ID==3).User.FullName);
            Assert.AreEqual(ExpectedCountResult, ActualCountResult);
        }
        #endregion

        #region Get Specific Account
        [TestMethod]
        public void Get_Specific_Account_Based_On_ClearingNumber()
        {
            //Arrange
            BankStorage accounts = new BankStorage();
            Account account = new Account { AccountID = 1, ClearingNumber = "NNNN-NNNN-NNNN-XXXX", User = new User { FullName = "Kalle Anka", UserID = 1, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 1, Balance = 0 };
            accounts.Add(account);
            Account ExpectedResult = new Account { AccountID = 1, ClearingNumber = "NNNN-NNNN-NNNN-XXXX", User = new User { FullName = "Kalle Anka", UserID = 1, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 1, Balance = 0 };
            //Act
            Account ActualResult = accounts.Get("NNNN-NNNN-NNNN-XXXX");
            //Assert
            Assert.AreEqual(ExpectedResult.AccountID, ActualResult.AccountID);
        }
        [TestMethod]
        public void Get_Account_Given_NonExistingClearingNr_Returns_Null()
        {
            //Arrange
            BankStorage bankStorage = new BankStorage();
            Account ExpectedResult = null;
            //Act
            Account ActualResult = bankStorage.Get("NNN-NNN-NNN-FFFF");
            //Assert
            Assert.AreEqual(ExpectedResult,ActualResult);
        }
#endregion

        #region Get All Acounts From Specific User
        [TestMethod]
        public void Get_All_Accounts_From_Specific_User_Given_FullName_Gommash()
        {
            //Arrange
            BankStorage bankAccounts = new BankStorage();
            User user = new User { FullName = "Gommash", Contact = "Test@Email.com", UserID = 1, Accounts = new List<Account>() };
            User user2 = new User { FullName = "Doldas", Contact = "Doldas@Email.com", UserID = 2, Accounts = new List<Account>() };
            Account account_user1 = new Account { AccountID = 1, ClearingNumber = "GGGG-GGGG-GGGG-QQQ1", WithdrawLock = false, Balance = 345.78, User_ID = 1, User = user };
            Account account2_user1 = new Account { AccountID = 2, ClearingNumber = "GGGG-GGGG-GGGG-QQQ3", WithdrawLock = true, Balance = 12343.34, User_ID = 1, User = user };
            Account account3_user2 = new Account { AccountID = 3, ClearingNumber = "GGGG-GGGG-GGGG-QQQ4", WithdrawLock = true, Balance = 3043.34, User_ID = 2, User = user2 };
            //Data-Binding
            //User 1
            user.Accounts.Add(account_user1);
            user.Accounts.Add(account2_user1);
            //User 2
            user2.Accounts.Add(account3_user2);
            //AddTobankAccountsClass
            bankAccounts.Add(account_user1);
            bankAccounts.Add(account2_user1);
            bankAccounts.Add(account3_user2);
            //ExpectedResult
            Account[] ExpectedAccounts={account_user1,account2_user1};
           
            //Act
            Account[] Gommash_Accounts = bankAccounts.GetAccounts("Gommash").ToArray();
     
            //Assert
            Assert.AreEqual(ExpectedAccounts.Length,Gommash_Accounts.Length);
            Assert.AreEqual(ExpectedAccounts[0].ClearingNumber, Gommash_Accounts[0].ClearingNumber);
            Assert.AreEqual(ExpectedAccounts[1].ClearingNumber, Gommash_Accounts[1].ClearingNumber);
            Assert.AreEqual(2,Gommash_Accounts.Length);

        }
#endregion

        #region Transaction
        [TestMethod]
        public void Transaction_Take_100_Money_From_Account_Give_It_To_Another_User_Account() {
            //Arrange
            BankStorage accounts = new BankStorage();
            Account account = new Account { AccountID = 2, ClearingNumber = "NNNN-NNNN-NNNN-5555", User = new User { FullName = "Kalle Anka2", UserID = 2, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 2, Balance = 300 };
            Account account2 = new Account { AccountID = 3, ClearingNumber = "NNNN-NNNN-NNNN-4444", User = new User { FullName = "Farbror Joakim Von Anka", UserID = 3, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 3, Balance = 0 };
            accounts.Add(account);
            accounts.Add(account2);
            double ExpectedResultAccount1 = 200;
            double ExpectedResultAccount2 = 100;
            //Act
            accounts.Transfer("NNNN-NNNN-NNNN-5555", "NNNN-NNNN-NNNN-4444", 100);
            double ActualResultAccount1 = accounts.Get("NNNN-NNNN-NNNN-5555").Balance;
            double ActualResultAccount2 = accounts.Get("NNNN-NNNN-NNNN-4444").Balance;
            //Assert
            Assert.AreEqual(ExpectedResultAccount1, ActualResultAccount1);
            Assert.AreEqual(ExpectedResultAccount2, ActualResultAccount2);
        }
        [TestMethod]
        public void Transaction_Take_200_Money_From_Account_That_Have_100_Money_And_Give_It_To_Another_User_Account2_Result_Nothing_Happens()
        {
            //Arrange
            BankStorage accounts = new BankStorage();
            Account account = new Account { AccountID = 2, ClearingNumber = "NNNN-NNNN-NNNN-5555", User = new User { FullName = "Kalle Anka2", UserID = 2, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 2, Balance = 100 };
            Account account2 = new Account { AccountID = 3, ClearingNumber = "NNNN-NNNN-NNNN-4444", User = new User { FullName = "Farbror Joakim Von Anka", UserID = 3, Contact = "mail@email.com", Accounts = new List<Account>() }, User_ID = 3, Balance = 0 };
            accounts.Add(account);
            accounts.Add(account2);
            double ExpectedResultAccount1 = 100;
            double ExpectedResultAccount2 = 0;
            //Act
            accounts.Transfer("NNNN-NNNN-NNNN-5555", "NNNN-NNNN-NNNN-4444", 200);
            double ActualResultAccount1 = accounts.Get("NNNN-NNNN-NNNN-5555").Balance;
            double ActualResultAccount2 = accounts.Get("NNNN-NNNN-NNNN-4444").Balance;
            //Assert
            Assert.AreEqual(ExpectedResultAccount1, ActualResultAccount1);
            Assert.AreEqual(ExpectedResultAccount2, ActualResultAccount2);
        }
        [TestMethod]
        public void Transaction_Take_200_Money_From_Account_Give_It_To_Another_Account_With_WithrawalLock_On_Equal_Nothing_Happens()
        {
            //Arrange
            BankStorage bankAccounts=new BankStorage();
            //User 1
            User user = new User { UserID=2, FullName="Kalle Anka2", Contact="email@mail.com", Accounts=new List<Account>()};
            Account account = new Account { AccountID=2, User_ID=2, User=user, ClearingNumber="NNNN-NNNN-NNNN-YYYY", Balance=400 };
            user.Accounts.Add(account);
            //User 2
            User user2 = new User { UserID = 3, FullName = "TestSubject", Contact = "email@mail.com", Accounts = new List<Account>() };
            Account account2 = new Account { AccountID = 3, User_ID = 3, User = user2, ClearingNumber = "NNNN-NNNN-NNNN-ZZZZ", Balance = 0, WithdrawLock=true};
            user2.Accounts.Add(account2);
            //AddTobankAccounts
            bankAccounts.Add(account);
            bankAccounts.Add(account2);
            //Expected Result
            double ExpectedResultAccount1 = 400;
            double ExpectedResultAccount2 = 0;
            //Act
            bankAccounts.Transfer("NNNN-NNNN-NNNN-YYYY", "NNNN-NNNN-NNNN-ZZZZ",200);
            double ActualResultAccount1 = bankAccounts.Get("NNNN-NNNN-NNNN-YYYY").Balance;
            double ActualResultAccount2 = bankAccounts.Get("NNNN-NNNN-NNNN-ZZZZ").Balance;
            //Assert
            Assert.AreEqual(ExpectedResultAccount1, ActualResultAccount1);
            Assert.AreEqual(ExpectedResultAccount2, ActualResultAccount2);
        }
        [TestMethod]
        public void Transaction_Take_200_Money_From_Account_Give_It_To_Non_Existing_Account_Do_Nothing()
        {
            //Arrange
            BankStorage bankAccounts = new BankStorage();
            //User 1
            User user = new User { UserID = 2, FullName = "Kalle Anka2", Contact = "email@mail.com", Accounts = new List<Account>() };
            Account account = new Account { AccountID = 2, User_ID = 2, User = user, ClearingNumber = "NNNN-NNNN-NNNN-YYYY", Balance = 400 };
            user.Accounts.Add(account);
            //AddTobankAccounts
            bankAccounts.Add(account);
            //Expected Result
            double ExpectedResultAccount1 = 400;
            //Act
            bankAccounts.Transfer("NNNN-NNNN-NNNN-YYYY", "NNNN-NNNN-NNNN-GGGG", 200);
            double ActualResultAccount1 = bankAccounts.Get("NNNN-NNNN-NNNN-YYYY").Balance;
            //Assert
            Assert.AreEqual(ExpectedResultAccount1, ActualResultAccount1);
        }
#endregion

        #region TransactionHistory
        [TestMethod]
        public void TransactionHistory_Given_ClearingNumber_GGGGGGGGGGGGQQQ1_Results_A_Correct_History_For_That_Account()
        {
            //Arrange
            BankStorage bank = new BankStorage();
            User user = new User { FullName = "Gommash", Contact = "Test@Email.com", UserID = 1, Accounts = new List<Account>() };
            Account account = new Account { AccountID = 1, ClearingNumber = "GGGG-GGGG-GGGG-QQQ1", WithdrawLock = false, Balance = 345.78, User_ID = 1, User = user, Transactions = new List<Transaction>() };
            Transaction transaction1 = new Transaction { TransactionID=1, TransactionDate=DateTime.Now, Money = 25, Category=Category.Payment, Account=account,Account_ID=1};
            Transaction transaction2 = new Transaction { TransactionID = 2, TransactionDate = DateTime.Now.AddMonths(-8), Money = 34.22, Category = Category.Payment, Account = account, Account_ID = 1 };
            Transaction transaction3 = new Transaction { TransactionID = 3, TransactionDate = DateTime.Now.AddMonths(2), Money = 125, Category = Category.Added, Account = account, Account_ID = 1 };
            Transaction transaction4 = new Transaction { TransactionID = 4, TransactionDate = DateTime.Now, Money = 3.89, Category = Category.Payment, Account = account, Account_ID = 1 };
            Transaction transaction5 = new Transaction { TransactionID = 5, TransactionDate = DateTime.Now.AddDays(-1), Money = 1050, Category = Category.Added, Account = account, Account_ID = 1 };
            //Data-Binding
            account.Transactions.Add(transaction1);
            account.Transactions.Add(transaction2);
            account.Transactions.Add(transaction3);
            account.Transactions.Add(transaction4);
            account.Transactions.Add(transaction5);
            user.Accounts.Add(account);
            //Add Accounts To The Bank
            bank.Add(account);
            
            //Act
            Transaction[] Transactions = bank.GetTransactions("GGGG-GGGG-GGGG-QQQ1").ToArray();
            
            //Assert
            Assert.AreEqual(125,Transactions[2].Money);
            Assert.AreEqual(Category.Added, Transactions[2].Category);

            Assert.AreEqual(5, Transactions.Length);
        }
        [TestMethod]
        public void GetTransaction_Given_TransactionID_2_Returns_Transaction()
        {
            //Arrange
            BankStorage bank = new BankStorage();
            User user = new User { FullName = "Gommash", Contact = "Test@Email.com", UserID = 1, Accounts = new List<Account>() };
            Account account = new Account { AccountID = 1, ClearingNumber = "GGGG-GGGG-GGGG-QQQ1", WithdrawLock = false, Balance = 345.78, User_ID = 1, User = user, Transactions = new List<Transaction>() };

            Transaction transaction1 = new Transaction { TransactionID = 1, TransactionDate = DateTime.Now, Money = 25, Category = Category.Payment, Account = account, Account_ID = 1 };
            Transaction transaction2 = new Transaction { TransactionID = 2, TransactionDate = DateTime.Now.AddMonths(-8), Money = 34.22, Category = Category.Payment, Account = account, Account_ID = 1 };
            Transaction transaction3 = new Transaction { TransactionID = 3, TransactionDate = DateTime.Now.AddMonths(2), Money = 125, Category = Category.Added, Account = account, Account_ID = 1 };

            account.Transactions.Add(transaction1);
            account.Transactions.Add(transaction2);
            account.Transactions.Add(transaction3);

            user.Accounts.Add(account);

            bank.Add(account);
            //Act
            Transaction ActualResult = bank.GetTransaction(2);
            //Assert
            Assert.AreEqual("Gommash", ActualResult.Account.User.FullName);
            Assert.AreEqual(transaction2, ActualResult);
        }
        #endregion

        #region AddMoney
        [TestMethod]
        public void Add_Magical_Money_Given_100_Result_100_Added()
        {
            //Arrange
            BankStorage bank = new BankStorage();
            User user = new User { FullName = "Gommash", Contact = "Test@Email.com", UserID = 1, Accounts = new List<Account>() };
            Account account = new Account { AccountID = 1, ClearingNumber = "GGGG-GGGG-GGGG-QQQ1", WithdrawLock = false, Balance = 0, User_ID = 1, User = user, Transactions = new List<Transaction>() };

            user.Accounts.Add(account);

            bank.Add(account);

            double ExpectedResult = 100;
            //Act
            bank.AddMagicalMoney("GGGG-GGGG-GGGG-QQQ1",100);
            
            //Assert
            Assert.AreEqual(ExpectedResult, bank.Get("GGGG-GGGG-GGGG-QQQ1").Balance);
        }
        [TestMethod]
        public void Add_Magical_Money_Given_200_Result_200_Added()
        {
            //Arrange
            BankStorage bank = new BankStorage();
            User user = new User { FullName = "Gommash", Contact = "Test@Email.com", UserID = 1, Accounts = new List<Account>() };
            Account account = new Account { AccountID = 1, ClearingNumber = "GGGG-GGGG-GGGG-QQQ1", WithdrawLock = false, Balance = 0, User_ID = 1, User = user, Transactions = new List<Transaction>() };

            user.Accounts.Add(account);

            bank.Add(account);

            double ExpectedResult = 200;
            //Act
            bank.AddMagicalMoney("GGGG-GGGG-GGGG-QQQ1", 200);

            //Assert
            Assert.AreEqual(ExpectedResult, bank.Get("GGGG-GGGG-GGGG-QQQ1").Balance);
        }
        //Limit 200000 Tests
        [TestMethod]
        public void Add_Magical_Money_Given_200000_Result_200000_Added()
        {
            //Arrange
            BankStorage bank = new BankStorage();
            User user = new User { FullName = "Gommash", Contact = "Test@Email.com", UserID = 1, Accounts = new List<Account>() };
            Account account = new Account { AccountID = 1, ClearingNumber = "GGGG-GGGG-GGGG-QQQ1", WithdrawLock = false, Balance = 0, User_ID = 1, User = user, Transactions = new List<Transaction>() };

            user.Accounts.Add(account);

            bank.Add(account);

            double ExpectedResult = 200000;
            //Act
            bank.AddMagicalMoney("GGGG-GGGG-GGGG-QQQ1", 200000);

            //Assert
            Assert.AreEqual(ExpectedResult, bank.Get("GGGG-GGGG-GGGG-QQQ1").Balance);
        }
        [TestMethod]
        public void Add_Magical_Money_Given_200001_Result_0_Added()
        {
            //Arrange
            BankStorage bank = new BankStorage();
            User user = new User { FullName = "Gommash", Contact = "Test@Email.com", UserID = 1, Accounts = new List<Account>() };
            Account account = new Account { AccountID = 1, ClearingNumber = "GGGG-GGGG-GGGG-QQQ1", WithdrawLock = false, Balance = 0, User_ID = 1, User = user, Transactions = new List<Transaction>() };

            user.Accounts.Add(account);

            bank.Add(account);

            double ExpectedResult = 0;

            //Act
            bank.AddMagicalMoney("GGGG-GGGG-GGGG-QQQ1", 200001);

            //Assert
            Assert.AreEqual(ExpectedResult, bank.Get("GGGG-GGGG-GGGG-QQQ1").Balance);
        }
        #endregion
    }
}
