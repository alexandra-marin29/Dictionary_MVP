using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DictionaryApp
{
    public partial class SearchControl : UserControl
    {
        private List<WordEntry> _wordEntries;

        public SearchControl()
        {
            InitializeComponent();
            LoadJsonData();
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            categoryComboBox.SelectionChanged += CategoryComboBox_SelectionChanged;

        }

        private void LoadJsonData()
        {
            string filePath = "D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\cuvinte.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                _wordEntries = JsonConvert.DeserializeObject<List<WordEntry>>(json) ?? new List<WordEntry>();
            }
            else
            {
                _wordEntries = new List<WordEntry>();
            }

            categoryComboBox.ItemsSource = _wordEntries.Select(w => w.Categorie).Distinct().ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSuggestions();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSuggestions();
        }

        private void UpdateSuggestions()
        {
            if (_wordEntries == null) return;

            string query = searchTextBox.Text;
            string selectedCategory = categoryComboBox.SelectedItem as string;

            var filteredList = _wordEntries
                .Where(entry => (string.IsNullOrWhiteSpace(selectedCategory) || entry.Categorie == selectedCategory) &&
                                entry.Cuvant.StartsWith(query, System.StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            if (!string.IsNullOrWhiteSpace(query) && filteredList.Any())
            {
                resultsListBox.ItemsSource = filteredList;
                suggestionsPopup.IsOpen = true;

               
                int itemHeight = 30;
                int maxItemsToShow = 5;
                int listBoxHeight = Math.Min(filteredList.Count, maxItemsToShow) * itemHeight;
                resultsListBox.Height = listBoxHeight;
            }
            else
            {
                suggestionsPopup.IsOpen = false;
            }
        }


        private void ResultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultsListBox.SelectedItem is WordEntry selectedWord)
            {
                // Setează textul searchTextBox la cuvântul selectat
                searchTextBox.Text = selectedWord.Cuvant;

                selectedWordTextBox.Text = $"Cuvânt: {selectedWord.Cuvant}";
                selectedWordCategoryTextBox.Text = $"Categorie: {selectedWord.Categorie}";
                selectedWordDefinitionTextBox.Text = $"Definiție: {selectedWord.Definitie}";

                var imagePath = $"D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\Images\\{selectedWord.Cuvant}.png"; // Presupunând că ai imagini numite după cuvânt
                if (File.Exists(imagePath))
                {
                    wordImage.Source = new BitmapImage(new Uri(imagePath));
                }
                else
                {
                    wordImage.Source = new BitmapImage(new Uri("D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\Images\\no_image.png")); // Imaginea de rezervă
                }

                suggestionsPopup.IsOpen = false;
            }
        }




    }
}
