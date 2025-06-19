using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ApdateFilmUser.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Studio { get; set; }
        public int Type { get; set; }
        public string AgeRating { get; set; }
        public int Duration { get; set; }
        public DateTime Release { get; set; }
        public string Rating { get; set; }
        public int? Episodes { get; set; }
        public string Preview { get; set; }
        public string ContentURL { get; set; }
        public List<Footage> Footages { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Director> Directors { get; set; }
        public List<Actor> Actors { get; set; }
        public List<Review> Review { get; set; }
    }
}
