using Se130RPGGame.Data.TestingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Se130RPGGame.Tests
{
    public class BankAccountTests
    {
        [Fact]
        public void Deposit_WhenGivenPositiveAmount_IncreaseBalance()
        {
            // arrange
            BankAccount bankAccount = new BankAccount();
            var initialBalance = bankAccount.Balance;

            // act
            bankAccount.Deposit(100);

            //assert
            Assert.Equal(initialBalance + 100, bankAccount.Balance);
        }


        [Fact]
        public void Withdraw_WhenSufficientFunds_DecreaseBalance()
        {
            // arrange
            BankAccount bankAccount = new BankAccount();
            bankAccount.Deposit(100);
            var initialBalance = bankAccount.Balance;

            // act
            bankAccount.WithDraw(50);

            //assert
            Assert.Equal(initialBalance - 50, bankAccount.Balance);
        }

        [Fact]
        public void Withdraw_WhenInsufficientFunds_ThrowsException()
        {
            // arrange
            BankAccount bankAccount = new BankAccount();
            bankAccount.Deposit(100);

            // act & assert

            Assert.Throws<ArgumentException>(() => bankAccount.WithDraw(150));
        }
    }
}
