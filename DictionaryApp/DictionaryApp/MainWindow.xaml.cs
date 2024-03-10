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
            HideInitialContent();
            ShowContent(new AdminControl());
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
            // Ascunde logo și titlu, sau alte elemente inițiale
            logoDict.Visibility = Visibility.Collapsed;
            titleLabel.Visibility = Visibility.Collapsed;
        }

        private void ShowContent(UserControl control)
        {
            // Asumăm că există un ContentControl sau un alt container în MainWindow pentru a găzdui UserControl-urile
            contentHost.Content = control;
        }
       
    }
}