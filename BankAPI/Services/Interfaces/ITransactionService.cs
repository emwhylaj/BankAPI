using BankAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Services.Interfaces
{
    public interface ITransactionService
    {
        Response CreateNewTransaction(Transaction transaction);

        Response FindTransactionByDate(DateTime date);

        Response MakeDeposit(string accountNumber, decimal amount, string transactionPin);

        Response Withdrawal(string accountNumber, decimal amount, string transactioPin);

        Response MakeFundsTransfer(string fromAccount, string toAccount, decimal amount, string trasactionPin);
    }
}