using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecomendation.Models
{
    /// <summary>
    /// This class is used to save and manipulate important metadata on movies(view count etc)
    /// </summary>
    public class MovieStat
    {
        public Movie Movie;
        public int ViewCount;
        public int BuyCount;
        public double PurchaseRate => BuyCount / (ViewCount+1);//+1 added as a quick fix to division by zero error
        public override string ToString()
        {
            return $"{Movie.Id,-3}{Movie.Name,-50} {Movie.Rating,-8}{ViewCount,-2}{BuyCount,-2}";
        }
    }
}
