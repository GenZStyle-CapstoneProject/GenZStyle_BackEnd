﻿using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Invoices
{
    public class GetInvoiceResponse
    {

        [Key]

        public int InvoiceId {  get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; }
        public string? PaymentType { get; set; }

        public string? PayUrl { get; set; }
        public string? Deeplink { get; set; }
        public string? QrCodeUrl { get; set; }
        public string? Applink { get; set; }
    }
}
