using System;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models
{
    public class UpdateAccountModel
    {
        [Key]
        public int Id { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must not be more than four digits")]
        public string Pin { get; set; }

        [Compare("Pin", ErrorMessage = "Pin does not match")]
        public string ConfirmPin { get; set; }

        public DateTime DateLastUpdated { get; set; }
    }
}