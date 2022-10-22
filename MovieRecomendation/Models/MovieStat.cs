using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecomendation.Models
{
    public class MovieStat
    {
        public Movie Movie;
        public int ViewCount;
        public int BuyCount;
        public double PurchaseRate => ViewCount / (BuyCount + 1);
        public override string ToString()
        {
            return $"{Movie.Id,-3}{Movie.Name,-50} {Movie.Rating,-8}{ViewCount,-2}{BuyCount,-2}";
        }
    }
}
