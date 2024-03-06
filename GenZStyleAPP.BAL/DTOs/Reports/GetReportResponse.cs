﻿using GenZStyleApp.DAL.Models;
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
        public int? PostId { get; set; }
        public int? ReporterId { get; set; }
        public string ReportName { get; set; }
        public string Status { get; set; }
        public int IsStatusReport { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}
