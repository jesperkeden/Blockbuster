using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockbuster.Models
{
    internal class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public int Units { get; set; }
        public int UnitsSold { get; set; }
        public decimal UnitPrice { get; set; }
        public double ImdbRating { get; set; }

        public List<MovieGenre> Genres { get; set; }
        public List<CustomerMovie> Customers { get; set; }

    }
}
