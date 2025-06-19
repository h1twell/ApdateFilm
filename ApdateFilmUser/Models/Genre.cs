using Flurl.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApdateFilmUser.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Genre() { }
        
        public Genre(string name)
        {
            Name = name;
        }
    }
}
