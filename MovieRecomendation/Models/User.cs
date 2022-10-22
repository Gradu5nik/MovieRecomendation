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
        public List<int> Bought { get => bought; set { bought = value; SetUserPreferences(); } }
        /// <summary>
        /// This property counts Keywords of movies user has bought and saves them in a dictionary as user preferences 
        /// (key is a keyword itself, value is how mony movies with that keyword user has bought)
        /// </summary>
        public Dictionary<string, int> Preferences { get; set; }

        public override string ToString()
        {
            return $"{Id,-3} {Name,-10}Viewed:{string.Join(",", Viewed),-10},Bought:{string.Join(",", Bought),-10}";
        }
        /// <summary>
        /// Counts all keyword appearances in bought movies to calculate the preference
        /// the more movies user has bought in a genre, the more points it is going to get
        /// </summary>
        /// <returns>self (can be refactored to return void)</returns>
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
        /// <summary>
        /// Benerates a score for a movie based on user preferences
        /// The more often movies with simmilar keywords have been bought by the user,
        /// the bigger the score will it get
        /// </summary>
        /// <param name="movie">Movie that gets scored</param>
        /// <returns>score</returns>
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
        /// <summary>
        /// Generates List of movie recomendations sorted by the score of each movie
        /// </summary>
        /// <param name="currentMovie"></param>
        /// <returns></returns>
        public List<Movie> GetMovieRecomendations(Movie currentMovie)
        {
            IEnumerable<Movie> BoughtMovies = Program.movieRepo.Movies.Values.Where(m => Bought.Contains(m.Id));
            //gets all movies with at least one simmilar keyword
            //except ones that user has already bought
            List<Movie> pool = Program.movieRepo.Movies.Values.Where(m => m.Keywords.Intersect(currentMovie.Keywords).Any()).Except(BoughtMovies).ToList();
            //assigns a score for each movie
            List<MovieScore> scores = new List<MovieScore>();
            foreach (var movie in pool)
            {
                scores.Add(new MovieScore { Movie = movie, Score = GetMovieScore(movie) });
            }
            //sorts movies by score then by rating
            scores = scores.OrderByDescending(m => m.Score).ThenByDescending(m=>m.Movie.Rating).ToList();
            //Returns list of movies without the score
            List<Movie> Recomendations=new List<Movie>();
            foreach (var item in scores)
            {
                Recomendations.Add(item.Movie);
            }
            return Recomendations;
        }
    }
    /// <summary>
    /// utility record, to simpify keeping track of movies and their scores
    /// </summary>
    record MovieScore
    {
        public Movie Movie;
        public int Score;
    }
}
