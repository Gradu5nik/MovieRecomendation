using MovieRecomendation.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecomendation.Repositories
{
    /// <summary>
    /// This class reads txt file with all the movies and saves them in a dictionary
    /// </summary>
    public class MovieRepo
    {
        public Dictionary<int,Movie> Movies { get; set; }
        public MovieRepo()
        {
            string[] lines = System.IO.File.ReadAllLines(Program.path+@"\input\Products.txt");
            Movies = new Dictionary<int, Movie>();
            foreach (string line in lines)
            {
                Movie movie = FromString(line);
                Movies.Add(movie.Id,movie);
            }
        }
        public Movie FromString(string s)
        {
            string[] data = s.Trim(' ').Split(',');
            Movie movie = new Movie()
            {
                Id = int.Parse(data[0]),
                Name = data[1],
                Year = int.Parse(data[2]),
                Keywords=new string[] { data[3], data[4], data[5], data[6], data[7] }.Where(s=>!string.IsNullOrWhiteSpace(s)).ToList(),
                Rating=double.Parse(data[8], CultureInfo.InvariantCulture),
                Price=double.Parse(data[9])
            };
            return movie;
        }
    }
}
