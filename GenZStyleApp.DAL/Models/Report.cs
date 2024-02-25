using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int? PostId { get; set; }

        public int? ReporterId { get; set; }
        public string ReportName { get; set; }
        public bool IsReport { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        
        public Post Post { get; set; }

    }
}
