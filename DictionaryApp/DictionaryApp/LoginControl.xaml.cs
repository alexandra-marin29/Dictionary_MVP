using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static DictionaryApp.LoginControl;

namespace DictionaryApp
{
    public partial class LoginControl : Window
    {
        public delegate void AuthenticationSuccessHandler();
        public event AuthenticationSuccessHandler OnAuthenticationSuccess;

        public LoginControl()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\users.json";
            string json = File.ReadAllText(filePath);
            var users = JsonConvert.DeserializeObject<List<User>>(json);

            var username = usernameTextBox.Text; 
            var password = passwordBox.Password; 

            var user = users?.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                OnAuthenticationSuccess?.Invoke(); 
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
