using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int PostId { get; set; }

        public int AccountId { get; set; }

        
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public string Content { get; set; }
        public string Image {  get; set; }

        public Account Account { get; set; }

        public ICollection<Like> Likes { get; set;}

        public ICollection<Comment> Comments { get;}

        public ICollection<FashionItem> FashionItems { get; set;}
    }
}
