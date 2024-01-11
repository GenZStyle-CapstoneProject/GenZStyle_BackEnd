using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.HashTag
{
    public class GetHashTagResponse
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<HashPost> HashPosts { get; set; }
    }
}
