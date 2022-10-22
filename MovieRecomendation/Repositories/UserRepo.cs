using MovieRecomendation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecomendation.Repositories
{
    /// <summary>
    /// This class reads all users from txt file and saves them as a dictionary
    /// </summary>
    public class UserRepo
    {
        public Dictionary<int,User> Users { get; set; }
        public UserRepo()
        {
            string[] lines = System.IO.File.ReadAllLines(Program.path + @"\input\Users.txt");
            Users = new Dictionary<int, User>();
            foreach (string line in lines)
            {
                User user = FromString(line);
                Users.Add(user.Id,user);
            }
        }
        public User FromString(string s)
        {
            string[] data = s.Trim(' ').Split(',');
            User user = new User()
            {
                Id = int.Parse(data[0]),
                Name = data[1],
                Viewed = Array.ConvertAll(data[2].Split(';'), s => int.Parse(s)).ToList(),
                Bought = Array.ConvertAll(data[3].Split(';'), s => int.Parse(s)).ToList()
            };
            return user;
        }
    }
}
