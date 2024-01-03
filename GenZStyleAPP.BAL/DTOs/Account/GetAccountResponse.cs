using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Inboxs;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleAPP.BAL.DTOs.Wallets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Account
{
    public class GetAccountResponse
    {
        [Key]
        public int AccountId { get; set; }
        public int UserId { get; set; }

        public int? InboxId { get; set; }

        public int WalletId { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsVip { get; set; }
        public bool IsActive { get; set; }

        
    }
}
