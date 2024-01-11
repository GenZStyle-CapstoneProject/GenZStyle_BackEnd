using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.UserRelations
{
    public class GetUserRelationResponse
    {
        public int FollowerId { get; set; }

        public int FollowingId { get; set; }

        
    }
}
