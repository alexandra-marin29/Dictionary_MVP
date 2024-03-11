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
    public class WordEntry
    {
        public string cuvant { get; set; }
        public string categorie { get; set; }
        public string definitie { get; set; }

        public override string ToString()
        {
            return cuvant;
        }
    }

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

            // Add the categories to the ComboBox
            categoryComboBox.ItemsSource = _wordEntries.Select(w => w.categorie).Distinct().ToList();
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
                .Where(entry => (string.IsNullOrWhiteSpace(selectedCategory) || entry.categorie == selectedCategory) &&
                                entry.cuvant.StartsWith(query, System.StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            if (!string.IsNullOrWhiteSpace(query) && filteredList.Any())
            {
                resultsListBox.ItemsSource = filteredList;
                suggestionsPopup.IsOpen = true;

                // Ajustează înălțimea ListBox-ului în funcție de numărul de sugestii
                // Presupunând că înălțimea unei linii este de 30px și maximul dorit pentru ListBox este 150px
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
                // Actualizează controalele UI cu informațiile cuvântului selectat
                selectedWordTextBox.Text = $"Cuvânt: {selectedWord.cuvant}";
                selectedWordCategoryTextBox.Text = $"Categorie: {selectedWord.categorie}";
                selectedWordDefinitionTextBox.Text = $"Definiție: {selectedWord.definitie}";

                // Încercăm să actualizăm imaginea cuvântului selectat
                var imagePath = $"D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\Images\\{selectedWord.cuvant}.png"; // Presupunând că ai imagini numite după cuvânt
                if (File.Exists(imagePath))
                {
                    wordImage.Source = new BitmapImage(new Uri(imagePath));
                }
                else
                {
                    wordImage.Source = new BitmapImage(new Uri("D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\Images\\no_image.png")); // Imaginea de rezervă
                }

                // Închide Popup-ul de sugestii
                suggestionsPopup.IsOpen = false;
            }
        }



    }
}
