using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationMicroservice.Models
{
    public class Authenticate
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
