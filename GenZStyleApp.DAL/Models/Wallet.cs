using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Wallet
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int WalletId { get; set; }

        public decimal Balance { get; set; }

        public DateTime CreatDate { get; set; }

        
        public Account Account { get; set; }

        
    }
}
