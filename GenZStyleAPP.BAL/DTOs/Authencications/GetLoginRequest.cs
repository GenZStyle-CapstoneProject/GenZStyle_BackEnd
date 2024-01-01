using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectParticipantManagement.BAL.DTOs.Authentications
{
    public class GetLoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
