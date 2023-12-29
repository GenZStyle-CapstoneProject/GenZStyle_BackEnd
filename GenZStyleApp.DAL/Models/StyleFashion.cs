using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class StyleFashion
    {   
        public int StyleId { get; set; }
        public int FashionId { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
    
        public Style Style { get; set; }

        public FashionItem FashionItem { get; set; }
    }
}
