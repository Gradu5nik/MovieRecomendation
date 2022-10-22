using MovieRecomendation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecomendation.Repositories
{
    /// <summary>
    /// This class is an aggregation of all movie metadata (basically moviestat repo)
    /// </summary>
    public class MovieDashboard
    { 
        /// <summary>
        /// Stats when created are sorted by view count(high to low) and purchase rate (also high to low)
        /// </summary>
        public List<MovieStat> Stats{get;set;}
        public MovieDashboard()
        {
            List<int> allViewed = new List<int>();
            List<int> allBought = new List<int>();
            foreach (User user in Program.userRepo.Users.Values)
            {
                allViewed.AddRange(user.Viewed);
                allBought.AddRange(user.Bought);
            }
            var ViewCount = allViewed.GroupBy(m => m, m => m, (movie, movies) => new
            {
                Key = movie,
                Count = movies.Count()
            }).ToList();


            var BuyCount = allBought.GroupBy(m => m, m => m, (movie, movies) => new
            {
                Key = movie,
                Count = movies.Count()
            }).ToList();


            Stats =(
                from movie in Program.movieRepo.Movies.Values
                join views in ViewCount on movie.Id equals views.Key into gjv
                join buy in BuyCount on movie.Id equals buy.Key into gjb
                from view in gjv.DefaultIfEmpty()
                from buy in gjb.DefaultIfEmpty()
                select new MovieStat
                {
                    Movie = movie,
                    ViewCount = view?.Count ?? 0,
                    BuyCount = buy?.Count ?? 0
                }
                ).ToList();
            //sorting logic is here
            Stats = Stats.OrderByDescending(mov => mov.ViewCount).ThenByDescending(mov => mov.PurchaseRate).ToList();
                ;
        }
    }
}
