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
        public int AccountId { get; set; } // người report

        public int? ReporterId { get; set; } //người bị report
        public int? PostId { get; set; }
        public string ReportName { get; set; }
        public bool IsReport { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }

    }
}
