using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DictionaryApp
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AdminModeButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginControl();
            loginWindow.OnAuthenticationSuccess += LoginWindow_OnAuthenticationSuccess; 
            loginWindow.ShowDialog(); 
        }
        private void LoginWindow_OnAuthenticationSuccess()
        {
            ShowContent(new AdminControl()); 
        }
        private void ShowContent(UserControl control)
        {
            contentHost.Content = control;
            HideInitialContent(); 
        }

        private void SearchModeButton_Click(object sender, RoutedEventArgs e)
        {
            HideInitialContent();
            ShowContent(new SearchControl());
        }

        private void EntertainmentModeButton_Click(object sender, RoutedEventArgs e)
        {
            HideInitialContent();
            ShowContent(new EntertainmentControl());
        }

        private void HideInitialContent()
        {
            logoDict.Visibility = Visibility.Collapsed;
            titleLabel.Visibility = Visibility.Collapsed;
        }

    }
}