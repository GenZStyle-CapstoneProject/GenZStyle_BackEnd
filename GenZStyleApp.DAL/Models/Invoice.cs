using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Invoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int InvoiceId { get; set; }
        
        public int AccountId { get; set; }
        public int packageId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public string PaymentType { get; set; }

        public Account Account { get; set; }

        public Package Package { get; set; }

        public ICollection<Payment> Payments { get; set;}
        
    }
}
