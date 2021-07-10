﻿using BankAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Services
{
    public interface IAccountService
    {
        Account Authenticate(string accountNumber, string pin);

        IEnumerable<Account> GetAllAccounts();

        Account Create(Account account, string pin, string confirmPin);

        void Update(Account account, string pin = null);

        void Delete(int id);

        Account GetById(int id);

        Account GetByAccountNumber(string accountNumber);
    }
}