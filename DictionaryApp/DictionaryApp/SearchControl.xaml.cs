using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            categoryComboBox.SelectionChanged += CategoryComboBox_SelectionChanged; // Adaugă acest event handler
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
                selectedWordTextBlock.Text = $"Cuvânt: {selectedWord.cuvant}";
                selectedWordCategoryTextBlock.Text = $"Categorie: {selectedWord.categorie}";
                selectedWordDefinitionTextBlock.Text = $"Definiție: {selectedWord.definitie}";

                // Închide Popup-ul de sugestii
                suggestionsPopup.IsOpen = false;
            }
        }


    }
}
