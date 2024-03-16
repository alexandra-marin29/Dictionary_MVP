using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using Microsoft.Win32;
using System;
using System.Text.RegularExpressions;

namespace DictionaryApp
{
    public partial class AdminControl : UserControl
    {
        private string selectedImagePath;
        private DataService dataService;
        private ImageService imageService;
        private ValidationService validationService;
        private List<WordEntry> words;

        public AdminControl()
        {
            InitializeComponent();

            dataService = new DataService("D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\cuvinte.json");
            imageService = new ImageService("D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\Images");
            validationService = new ValidationService();

            LoadWords();
            wordTextBox.TextChanged += wordTextBox_TextChanged;

        }


        private void LoadWords()
        {
            words = dataService.LoadWords();
            LoadCategories();
        }

        private void LoadCategories()
        {
            categoryComboBox.ItemsSource = words.Select(w => w.Categorie).Distinct().ToList();
        }

        private void wordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && !string.IsNullOrWhiteSpace(textBox.Text))
            {
                var existingEntry = words.FirstOrDefault(w => w.Cuvant.Equals(textBox.Text, StringComparison.OrdinalIgnoreCase));
                if (existingEntry != null)
                {
                    descriptionTextBox.Text = existingEntry.Definitie;
                    categoryComboBox.SelectedItem = existingEntry.Categorie;
                    selectedImage.Source = imageService.LoadImage(existingEntry.Cuvant);
                }
            }
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
                selectedImagePath = openFileDialog.FileName; 
                selectedImage.Source = new BitmapImage(new Uri(selectedImagePath));
            }
        }
        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCategory = categoryComboBox.Text;
            if (!string.IsNullOrWhiteSpace(selectedCategory) && !words.Any(w => w.Categorie == selectedCategory))
            {

                var newCategory = selectedCategory;
                words.Add(new WordEntry { Categorie = newCategory });
                dataService.SaveWords(words);

                LoadCategories();
            }
        }

        private void AddModifyWordButton_Click(object sender, RoutedEventArgs e)
        {
            var word = wordTextBox.Text.Trim();
            var definition = descriptionTextBox.Text.Trim();
            var category = categoryComboBox.Text.Trim();

            if (!validationService.IsValidWord(word) || !validationService.IsValidCategory(category) || !validationService.IsValidDefinition(definition))
            {
                MessageBox.Show("Unul sau mai multe câmpuri sunt invalide.");
                return;
            }

            var existingEntry = words.FirstOrDefault(w => w.Cuvant.Equals(word, StringComparison.OrdinalIgnoreCase));
            bool isNewWord = existingEntry == null;

            if (!isNewWord)
            {
                existingEntry.Definitie = definition;
                existingEntry.Categorie = category;

                MessageBox.Show("Informațiile cuvântului au fost actualizate.");
            }
            else
            {
                words.Add(new WordEntry { Cuvant = word, Definitie = definition, Categorie = category });
                MessageBox.Show("Cuvântul a fost adăugat cu succes.");
            }

            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                if (!isNewWord && existingEntry != null && !string.IsNullOrEmpty(selectedImagePath))
                {
                    imageService.DeleteImage(existingEntry.Cuvant);
                }

                var saveResult = imageService.SaveImage(selectedImagePath, word);
                imageService.SaveImage(selectedImagePath, word); 
                if (saveResult != null)
                {
                    MessageBox.Show("Imaginea a fost salvată cu succes");
                }
                else
                {
                    MessageBox.Show("Nu s-a putut salva imaginea.");
                }
            }


            dataService.SaveWords(words);
            LoadCategories(); 
        }


        private void DeleteWordButton_Click(object sender, RoutedEventArgs e)
        {
            var wordToDelete = wordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(wordToDelete))
            {
                MessageBox.Show("Vă rugăm să introduceți un cuvânt pentru a fi șters.");
                return;
            }

            var wordEntry = words.FirstOrDefault(w => w.Cuvant.Equals(wordToDelete, StringComparison.OrdinalIgnoreCase));
            if (wordEntry != null)
            {
                words.Remove(wordEntry);
                dataService.SaveWords(words);
                imageService.DeleteImage(wordToDelete); 
                MessageBox.Show($"Cuvântul și imaginea asociată pentru '{wordToDelete}' au fost șterse.");
            }
            else
            {
                MessageBox.Show($"Cuvântul '{wordToDelete}' nu a fost găsit.");
            }
        }

    }
}