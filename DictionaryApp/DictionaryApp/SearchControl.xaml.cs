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
        private DataService dataService;
        private ImageService imageService;
        private List<WordEntry> _wordEntries;

        public SearchControl()
        {
            InitializeComponent();
            dataService = new DataService("D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\cuvinte.json");
            imageService = new ImageService("D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\Images");
            LoadJsonData();
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            categoryComboBox.SelectionChanged += CategoryComboBox_SelectionChanged;

        }

        private void LoadJsonData()
        {
            _wordEntries = dataService.LoadWords();
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
                // Afiseaza cuvantul in searchTextBox cand este selectat din lista
                searchTextBox.Text = selectedWord.Cuvant;

                selectedWordTextBox.Text = $"Cuvânt: {selectedWord.Cuvant}";
                selectedWordCategoryTextBox.Text = $"Categorie: {selectedWord.Categorie}";
                selectedWordDefinitionTextBox.Text = $"Definiție: {selectedWord.Definitie}";

                // Incarcare si afisare imagine asociata cuvantului, daca exista
                wordImage.Source = imageService.LoadImage(selectedWord.Cuvant);

                suggestionsPopup.IsOpen = false;
            }
        }




    }
}
