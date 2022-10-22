// See https://aka.ms/new-console-template for more information
using MovieRecomendation;
using MovieRecomendation.Models;
using MovieRecomendation.Repositories;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;

internal class Program
{
    public static string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
    public static MovieRepo movieRepo = new MovieRepo();
    public static UserRepo userRepo = new UserRepo();
    public static MovieDashboard dashboard = new MovieDashboard();
    static void Main(string[] args)
    {
        
        Console.WriteLine("Here are most popular products -(most viewed and frequently purchased)");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine(dashboard.Stats[i].Movie);
        }
        string[] lines = System.IO.File.ReadAllLines(path + @"\input\CurrentUserSession.txt");
        List<(User,Movie)> Sessions = new List<(User, Movie)>();
        foreach (string line in lines)
        {
            string[] data = line.Trim(' ').Split(',');
            Sessions.Add((userRepo.Users[int.Parse(data[0])], movieRepo.Movies[int.Parse(data[1])] ));
            
        }
        List<Movie> recomendations = new List<Movie>();
        foreach (var item in Sessions)
        {
            recomendations = item.Item1.GetMovieRecomendations(item.Item2);
            Console.WriteLine($"{item.Item1.Name} is viewing {item.Item2.Name}");
            Console.WriteLine("His/her recomendations are");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(recomendations[i]);
            }
            Console.WriteLine();
        }
    }
}
