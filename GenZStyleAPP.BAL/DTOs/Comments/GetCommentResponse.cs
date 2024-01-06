using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Comments
{
    public class GetCommentResponse
    {
        [Key]
        public int CommentId { get; set; }

        public int AccountId { get; set; }
        public int PostId { get; set; }

        public DateTime CreateAt { get; set; }
        public string Content { get; set; }

        public Post Post { get; set; }

        public Account Account { get; set; }


    }
}
