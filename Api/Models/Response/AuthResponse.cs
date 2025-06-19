using ApdateFilmUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Models.Response
{
    class AuthResponse
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
