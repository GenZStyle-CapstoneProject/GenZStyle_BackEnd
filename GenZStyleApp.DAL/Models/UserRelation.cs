﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class UserRelation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public int FollowerId { get; set; }

        public int FollowingId { get; set; }

        public Account Account { get; set; }
    }
}
