using GenZStyleApp.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.HashTag
{
    public class GetHashTagRequest
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<HashPost> HashPosts { get; set; }
    }
}
