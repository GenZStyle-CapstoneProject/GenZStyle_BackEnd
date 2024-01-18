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
        public int PostId { get; set; }
        public int TotalComment { get; set; }
        
        public int TotalLike { get; set; }


    }
}
