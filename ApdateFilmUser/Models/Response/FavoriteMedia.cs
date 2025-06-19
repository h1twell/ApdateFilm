using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApdateFilmUser.Models.Response
{
    public class FavoriteMedia
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("media")]
        public Media Media { get; set; }
    }
}
