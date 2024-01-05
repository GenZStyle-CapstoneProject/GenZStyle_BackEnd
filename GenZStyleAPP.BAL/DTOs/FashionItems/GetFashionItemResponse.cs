using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.FashionItems
{
    public class GetFashionItemResponse
    {
        [Key]
        public int FashionId { get; set; }
        public string fashionName { get; set; }
        public string fashionDescription { get; set; }

        public Decimal Cost { get; set; }

        public string Image { get; set; }
    }
}
