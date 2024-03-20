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

        private UserService userService;

        public LoginControl()
        {
            InitializeComponent();
            usernameTextBox.KeyDown += HandleEnterKey;
            passwordBox.KeyDown += HandleEnterKey;
            userService = new UserService("D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\users.json");

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
         
            var username = usernameTextBox.Text;
            var password = passwordBox.Password;

            var user = userService.AuthenticateUser(username, password);

            if (user != null)
            {
                OnAuthenticationSuccess?.Invoke();
                this.Close();
            }
            else
            {
                MessageBox.Show("Nume de utilizator sau parolă invalidă");
            }
        }

        
    }
}
