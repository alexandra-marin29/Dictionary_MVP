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
        // Definiți un delegat și un event pentru succesul autentificării
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

            var username = usernameTextBox.Text; // Asigură-te că ai un TextBox cu x:Name="usernameTextBox" în XAML
            var password = passwordBox.Password; // Asigură-te că ai un PasswordBox cu x:Name="passwordBox" în XAML

            // Caută utilizatorul în lista încărcată
            var user = users?.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                OnAuthenticationSuccess?.Invoke(); // Notifică despre succesul autentificării
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
