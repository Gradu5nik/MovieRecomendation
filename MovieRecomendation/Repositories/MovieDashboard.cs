using MovieRecomendation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecomendation.Repositories
{
    public class MovieDashboard
    {
        private IEnumerable<MovieStat> _stats;

        public List<MovieStat> Stats
        {
            get { return _stats.OrderByDescending(mov => mov.ViewCount).ThenBy(mov => mov.PurchaseRate).ToList(); }
            set { _stats = value.ToList(); }
        }

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
                ;
        }
    }
}
