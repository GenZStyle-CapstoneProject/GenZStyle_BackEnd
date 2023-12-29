using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class FashionItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int FashionId {  get; set; }

        public int CategoryId { get; set; }
        public string fashionName { get; set; }
        public string fashionDescription { get; set; }

        public Decimal Cost { get; set; }

        public string Image {  get; set; }

        public Category Category { get; set; }
        public ICollection<StyleFashion> StyleFashions { get; set; }
    }
}
