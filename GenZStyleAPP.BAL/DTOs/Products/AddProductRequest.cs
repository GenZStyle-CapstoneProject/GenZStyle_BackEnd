using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Products
{
    public class AddProductRequest
    {
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public bool Gender { get; set; }
        public decimal Cost { get; set; }
        public IFormFile Image { get; set; }
    }
}
