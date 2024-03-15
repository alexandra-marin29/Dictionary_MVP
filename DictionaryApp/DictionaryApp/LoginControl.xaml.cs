using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input; 
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
            usernameTextBox.KeyDown += HandleEnterKey;
            passwordBox.KeyDown += HandleEnterKey;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            AttemptLogin();
        }

        private void HandleEnterKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AttemptLogin();
            }
        }

        private void AttemptLogin()
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
