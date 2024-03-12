using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using Microsoft.Win32;
using System;

namespace DictionaryApp
{
    public partial class AdminControl : UserControl
    {
        private string filePath = "D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\cuvinte.json";
        private string imagesPath = "D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\Images";
        private List<WordEntry> words;

        public AdminControl()
        {
            InitializeComponent();
            LoadWords();
        }
        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png)|*.png",
                Title = "Selectează o imagine"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Încarcă și afișează imaginea selectată
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                selectedImage.Source = bitmap;
            }
        }

        private void LoadWords()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                words = JsonConvert.DeserializeObject<List<WordEntry>>(json) ?? new List<WordEntry>();
                LoadCategories(); // Apelăm metoda nou adăugată pentru a încărca categoriile
            }
            else
            {
                words = new List<WordEntry>();
            }
        }

        private void LoadCategories()
        {
            // Extrage toate categoriile unice din lista de cuvinte
            var categories = words.Select(w => w.Categorie).Distinct().ToList();

            // Setează sursele pentru categoryComboBox
            categoryComboBox.ItemsSource = categories;
        }
        private void SaveWords()
        {
            string json = JsonConvert.SerializeObject(words, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCategory = categoryComboBox.Text;
            if (!string.IsNullOrWhiteSpace(selectedCategory) && !words.Any(w => w.Categorie == selectedCategory))
            {
                
                var newCategory = selectedCategory;
                words.Add(new WordEntry { Categorie = newCategory });
                SaveWords(); 

                LoadCategories();
            }
        }



        private void AddWordButton_Click(object sender, RoutedEventArgs e)
        {
            // Logica pentru adăugarea unui cuvânt nou
            var word = wordTextBox.Text;
            var definition = descriptionTextBox.Text;
            var category = categoryComboBox.Text;

            // Salvează imaginea
            SaveImage(selectedImage.Source as BitmapImage, word);
            var newWord = new WordEntry
            {
                Cuvant = word,
                Definitie = definition,
                Categorie = category,
            };

            words.Add(newWord);
            SaveWords(); // Salvează lista actualizată în fișier
            MessageBox.Show("Cuvântul a fost adăugat. Imaginea a fost salvată.");
        }
        private string SaveImage(BitmapImage bitmapImage, string word)
        {
            if (bitmapImage != null && bitmapImage.UriSource != null)
            {
                var fileName = word + ".png"; // Numele fișierului este format din cuvântul introdus
                var destinationPath = Path.Combine(imagesPath, fileName);

                // Copiază imaginea în folderul destinat
                File.Copy(bitmapImage.UriSource.LocalPath, destinationPath, true);

                return fileName; // Returnează noul nume de fișier
            }

            return null; // Returnează null dacă nu s-a salvat o imagine
        }

        private void UpdateWordButton_Click(object sender, RoutedEventArgs e)
        {
            //var wordToUpdate = wordTextBox.Text;
            //var newDefinition = descriptionTextBox.Text;
            //var newCategory = categoryComboBox.Text;

            //// Găsește cuvântul în lista de cuvinte
            //var wordEntry = words.FirstOrDefault(w => w.Cuvant.Equals(wordToUpdate, StringComparison.OrdinalIgnoreCase));
            //if (wordEntry != null)
            //{
            //    // Actualizează detaliile cuvântului
            //    wordEntry.Definitie = newDefinition;
            //    wordEntry.Categorie = newCategory;

            //    SaveWords(); // Salvează lista actualizată în fișier
            //    MessageBox.Show("Cuvântul a fost actualizat.");
            //}
            //else
            //{
            //    MessageBox.Show("Cuvântul nu a fost găsit.");
            //}
        }


        private void DeleteWordButton_Click(object sender, RoutedEventArgs e)
        {
            //var wordToDelete = wordTextBox.Text;

            //// Găsește și șterge cuvântul din lista de cuvinte
            //var wordEntry = words.FirstOrDefault(w => w.Cuvant.Equals(wordToDelete, StringComparison.OrdinalIgnoreCase));
            //if (wordEntry != null)
            //{
            //    words.Remove(wordEntry);
            //    SaveWords(); // Salvează lista actualizată în fișier
            //    MessageBox.Show("Cuvântul a fost șters.");
            //}
            //else
            //{
            //    MessageBox.Show("Cuvântul nu a fost găsit.");
            //}
        }

        public class WordEntry
        {
            public string Cuvant { get; set; }
            public string Categorie { get; set; }
            public string Definitie { get; set; }
        }

       
    }
}