using GenZStyleAPP.BAL.DTOs.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Users
{
    public class GetUserResponse
    {
        [Key]
        public int UserID { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public decimal? Height { get; set; }
        public string? AvatarUrl { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public ICollection<GetAccountResponse> Accounts { get; set; }
    }
}
