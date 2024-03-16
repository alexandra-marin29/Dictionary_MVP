using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DictionaryApp
{
    public class UserService
    {
        private string filePath;

        public UserService(string filePath)
        {
            this.filePath = filePath;
        }

        public User AuthenticateUser(string username, string password)
        {
            var json = File.ReadAllText(filePath);
            var users = JsonConvert.DeserializeObject<List<User>>(json);
            return users?.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
