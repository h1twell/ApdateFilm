using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApdateFilmUser.Models.Response
{
    public class UserResponse
    {
        [JsonPropertyName("user")]
        public User User { get; set; }
    }
}
