using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApdateFilmUser.Models
{
    public class Studio
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        public Studio() { }

        public Studio(string name)
        {
            Name = name;
        }
    }
}
