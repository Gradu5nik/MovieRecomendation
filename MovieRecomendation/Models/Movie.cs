using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecomendation.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public List<string> Keywords { get; set; }
        public double Rating { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            return $"{Id,-3} {Name,-20} - {Year,-5}{string.Join(",", Keywords)}{Rating,4} stars,{Price,4} dkk";
        }
    }
}
