using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApdateFilmUser.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Bio { get; set; }
        public string Photo { get; set; }

        [JsonPropertyName("media")]

        public List<Media> Media { get; set; }
    }
}
