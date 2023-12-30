using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Transaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int PaymentId { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        public string TransStyle { get; set; }
        public int status { get; set; }
        public Payment Payment { get; set; }

        public Wallet wallet { get; set; }

    }
}
