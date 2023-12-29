using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class Like
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int LikeId { get; set; } 

        public int PostId { get; set; }
        public int AccountId { get; set; }

        public Post Post { get; set; }

        public Account Account { get; set; }

    }
}
