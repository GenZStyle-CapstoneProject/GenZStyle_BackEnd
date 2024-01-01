using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectParticipantManagement.BAL.DTOs.Authentications
{
    public class GetLoginResponse
    {
        [Key]
        public int EmployeeID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
