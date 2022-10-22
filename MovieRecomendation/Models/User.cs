using MovieRecomendation.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecomendation.Models
{
    public class User
    {
        private List<int> bought;

        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Viewed { get; set; }
        public List<int> Bought { get => bought; set { bought = value; this.SetUserPreferences(); } }
        public Dictionary<string, int> Preferences { get; set; }

        public override string ToString()
        {
            return $"{Id,-3} {Name,-10}Viewed:{string.Join(",", Viewed),-10},Bought:{string.Join(",", Bought),-10}";
        }
        public User SetUserPreferences()
        {
            List<string> Kwords = new List<string>();
            foreach (var i in Bought)
            {
                Kwords.AddRange(Program.movieRepo.Movies[i].Keywords);
            }
            Preferences = Kwords.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
            return this;
        }
        public int GetMovieScore(Movie movie)
        {

            int sum = 0;
            foreach (var item in movie.Keywords)
            {
                if (Preferences.ContainsKey(item))
                {
                    sum += Preferences[item];
                }
            }
            return sum;
        }
        public List<Movie> GetMovieRecomendations(Movie currentMovie)
        {
            IEnumerable<Movie> BoughtMovies = Program.movieRepo.Movies.Values.Where(m => Bought.Contains(m.Id));
            List<Movie> pool = Program.movieRepo.Movies.Values.Where(m => m.Keywords.Intersect(currentMovie.Keywords).Any()).Except(BoughtMovies).ToList();
            List<MovieScore> scores = new List<MovieScore>();
            foreach (var movie in pool)
            {
                scores.Add(new MovieScore { Movie = movie, Score = GetMovieScore(movie) });
            }
            scores = scores.OrderByDescending(m => m.Score).ThenByDescending(m=>m.Movie.Rating).ToList();
            List<Movie> Recomendations=new List<Movie>();
            foreach (var item in scores)
            {
                Recomendations.Add(item.Movie);
            }
            return Recomendations;
        }
    }
    record MovieScore
    {
        public Movie Movie;
        public int Score;
    }
}
