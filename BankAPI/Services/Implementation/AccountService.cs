using BankAPI.DAL;
using BankAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankAPI.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly BankApiDbContext _dbContext;

        public AccountService(BankApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Account Authenticate(string accountNumber, string pin)
        {
            var account = _dbContext.Accounts.Where(x => x.AccountNumberGenerated == accountNumber).SingleOrDefault();
            if (account == null) return null;

            if (!VerifiedPinHash(pin, account.PinHash, account.PinSalt)) return null;

            return account;
        }

        private static bool VerifiedPinHash(string pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(pin)) throw new ArgumentNullException("pin");

            using (var hmac = new HMACSHA512(pinSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != pinHash[i]) return false;
                }
            }
            return true;
        }

        public Account Create(Account account, string pin, string confirmPin)
        {
            if (_dbContext.Accounts.Any(x => x.Email == account.Email)) throw new ArgumentException("An Account already exist with this email");
            if (!pin.Equals(confirmPin)) throw new ArgumentException("Pins do not match", "pin");

            byte[] pinHash, pinSalt;
            CreatePinHash(pin, out pinHash, out pinSalt);

            account.PinHash = pinHash;
            account.PinSalt = pinSalt;

            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();

            return account;
        }

        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        public void Delete(int id)
        {
            var account = _dbContext.Accounts.Find(id);
            if (account != null)
            {
                _dbContext.Accounts.Remove(account);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _dbContext.Accounts.ToList();
        }

        public Account GetByAccountNumber(string accountNumber)
        {
            var account = _dbContext.Accounts.Where(x => x.AccountNumberGenerated == accountNumber).FirstOrDefault();
            if (account == null) return null;

            return account;
        }

        public Account GetById(int id)
        {
            var account = _dbContext.Accounts.Where(x => x.Id == id).FirstOrDefault();
            if (account == null) return null;

            return account;
        }

        public void Update(Account account, string pin = null)
        {
            var accountToBeUpdated = _dbContext.Accounts.Where(x => x.Email == account.Email).SingleOrDefault();
            if (accountToBeUpdated == null) throw new ApplicationException("Account does not exist");

            if (!string.IsNullOrWhiteSpace(account.Email))
            {
                if (_dbContext.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("This email" + account.Email + " already exists");
            }

            accountToBeUpdated.Email = account.Email;

            if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                if (_dbContext.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber)) throw new ApplicationException("This phone number" + account.PhoneNumber + " already exists");
            }

            accountToBeUpdated.PhoneNumber = account.PhoneNumber;

            if (accountToBeUpdated == null) throw new ApplicationException("Account odes not exist");

            if (!string.IsNullOrWhiteSpace(pin))
            {
                byte[] pinHash, pinSalt;

                CreatePinHash(pin, out pinHash, out pinSalt);

                accountToBeUpdated.PinHash = pinHash;
                accountToBeUpdated.PinSalt = pinSalt;
            }
            accountToBeUpdated.DateLastUpdated = DateTime.Now;

            _dbContext.Accounts.Update(accountToBeUpdated);
            _dbContext.SaveChanges();
        }
    }
}