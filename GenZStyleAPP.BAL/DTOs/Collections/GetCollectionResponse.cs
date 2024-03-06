using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Collections
{
    public class GetCollectionResponse
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account? Account { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string Name { get; set; }
        public string Image_url { get; set; }
        public int Type { get; set; }
    }
}
