using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.HashTags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.HashPosts
{
    public class GetHashPostsResponse
    {
        public int PostId { get; set; }
        [Key]
        public int HashTageId { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public GetHashTagResponse Hashtag { get; set; }
    }
}
