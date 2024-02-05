using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Reports
{
    public class GetReportResponse
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int PostId { get; set; }
        public string ReportName { get; set; }
        public bool IsReport { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}
