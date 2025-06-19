using ApdateFilmUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Review
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Media { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }
}
