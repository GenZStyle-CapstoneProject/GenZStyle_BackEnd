using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int CategoriesId { get; set; }
        
        public string CategoryName { get; set; }
        
        public string CategoryDescription { get; set; }

        public ICollection<FashionItem> FashionItems { get; set;}
    }
}
