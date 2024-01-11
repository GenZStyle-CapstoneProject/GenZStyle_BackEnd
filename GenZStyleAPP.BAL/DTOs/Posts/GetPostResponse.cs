using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.DTOs.FashionItems;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GenZStyleAPP.BAL.DTOs.PostLike;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Posts
{
    public class GetPostResponse
    {

        [Key]
        public int PostId { get; set; }

        public int AccountId { get; set; }


        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public string Content { get; set; }
        public string Image { get; set; }
        public ICollection<HashPost> HashPosts { get; set; }
        public GetAccountResponse Account { get; set; }
        public ICollection<GetFashionItemResponse> FashionItems { get; set; }

        public ICollection<GetPostLikeResponse> Likes { get; set; }
    }
}
