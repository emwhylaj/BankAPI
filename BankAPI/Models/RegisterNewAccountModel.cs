﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models
{
    public class RegisterNewAccountModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //public string AccountName { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        //public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }

        //public string AccountNumberGenerated { get; set; }

        //public byte[] PinHash { get; set; }
        //public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }

        public DateTime DateLastUpdated { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]/d{4}$", ErrorMessage = "Pin must not be more than four digits")]
        public string Pin { get; set; }

        [Compare("Pin", ErrorMessage = "Pin does not match")]
        public string ConfirmPin { get; set; }
    }
}