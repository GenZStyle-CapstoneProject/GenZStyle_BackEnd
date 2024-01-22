using GenZStyleApp.DAL.Models;
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

        public int CategoryId { get; set; }

        public int PostId { get; set; }
        public string fashionName { get; set; }
        public string fashionDescription { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public bool Gender { get; set; }
        public Decimal Cost { get; set; }

        public string Image { get; set; }

        public Post Post { get; set; }

        public Category Category { get; set; }
        public ICollection<StyleFashion> StyleFashions { get; set; }
    }
}
