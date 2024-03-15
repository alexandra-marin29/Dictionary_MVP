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
        private string filePath = "D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\cuvinte.json";
        private string imagesPath = "D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\Images";
        private List<WordEntry> words;

        public AdminControl()
        {
            InitializeComponent();
            LoadWords();
            wordTextBox.TextChanged += wordTextBox_TextChanged;

        }
        private bool IsValidWord(string word)
        {
            return Regex.IsMatch(word, "^[a-zA-ZăîâșțĂÎÂȘȚ-]+$");
        }

        private bool IsValidCategory(string category)
        {
            return Regex.IsMatch(category, "^[a-zA-ZăîâșțĂÎÂȘȚ ]+$");
        }

        private bool IsValidDefinition(string definition)
        {
            return Regex.IsMatch(definition, "^[a-zA-ZăîâșțĂÎÂȘȚ0-9,;.!?-]+$");
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
                    categoryComboBox.SelectedItem = categoryComboBox.Items.Cast<string>().FirstOrDefault(c => c.Equals(existingEntry.Categorie, StringComparison.OrdinalIgnoreCase));

                    string imagePath = Path.Combine(imagesPath, existingEntry.Cuvant + ".png");
                    if (!File.Exists(imagePath))
                    {
                        imagePath = Path.Combine(imagesPath, "no_image.png");
                    }
                    LoadImage(imagePath);
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
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                selectedImage.Source = bitmap;
            }
        }
        private void LoadImage(string imagePath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            selectedImage.Source = bitmap;
        }


        private void LoadWords()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                words = JsonConvert.DeserializeObject<List<WordEntry>>(json) ?? new List<WordEntry>();
                LoadCategories(); 
            }
            else
            {
                words = new List<WordEntry>();
            }
        }

        private void LoadCategories()
        {
            var categories = words.Select(w => w.Categorie).Distinct().ToList();

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


        private void AddModifyWordButton_Click(object sender, RoutedEventArgs e)
        {
            var word = wordTextBox.Text.Trim();
            var definition = descriptionTextBox.Text.Trim();
            var category = categoryComboBox.Text.Trim();

            if (string.IsNullOrEmpty(word) || string.IsNullOrEmpty(definition) || string.IsNullOrEmpty(category))
            {
                MessageBox.Show("Trebuie să introduceți un cuvânt, o definiție și să selectați o categorie pentru a salva.");
                return;
            }
            if (!IsValidWord(word))
            {
                MessageBox.Show("Cuvântul introdus este invalid.");
                return;
            }

            if (!IsValidCategory(category))
            {
                MessageBox.Show("Categoria introdusă este invalidă.");
                return;
            }

            if (!IsValidDefinition(definition))
            {
                MessageBox.Show("Definiția introdusă este invalidă.");
                return;
            }

            var existingEntry = words.FirstOrDefault(w => w.Cuvant.Equals(word, StringComparison.OrdinalIgnoreCase));
            if (existingEntry != null)
            {
                existingEntry.Definitie = definition;
                existingEntry.Categorie = category;

                MessageBox.Show("Informațiile cuvântului au fost actualizate.");

                var imagePath = SaveImage(selectedImage.Source as BitmapImage, word);
                if (imagePath != null)
                {
                    MessageBox.Show("Imaginea a fost actualizată.");
                }
            }
            else
            {
                var imagePath = SaveImage(selectedImage.Source as BitmapImage, word);
                if (imagePath != null)
                {
                    MessageBox.Show("Imaginea a fost salvată.");
                }

                var newWord = new WordEntry
                {
                    Cuvant = word,
                    Definitie = definition,
                    Categorie = category
                };

                words.Add(newWord);
                MessageBox.Show("Cuvântul a fost adăugat cu succes.");
            }

            SaveWords(); 
        }


        private string SaveImage(BitmapImage bitmapImage, string word)
        {
            if (bitmapImage != null && bitmapImage.UriSource != null)
            {
                var fileName = word + ".png";
                var destinationPath = Path.Combine(imagesPath, fileName);

                try
                {
                    File.Copy(bitmapImage.UriSource.LocalPath, destinationPath, true);
                    return destinationPath; 
                }
                catch
                {
                    return null; 
                }
            }
            return null; 
        }


        private void DeleteWordButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
                    SaveWords();

                    var imagePath = Path.Combine(imagesPath, wordToDelete + ".png");

                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                        MessageBox.Show($"Cuvântul și imaginea asociată pentru '{wordToDelete}' au fost șterse.");
                    }
                    else
                    {
                        MessageBox.Show($"Cuvântul '{wordToDelete}' a fost șters, dar nu s-a găsit nicio imagine asociată pentru a fi eliminată.");
                    }
                }
                else
                {
                    MessageBox.Show($"Cuvântul '{wordToDelete}' nu a fost găsit.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare: {ex.Message}");
            }
        }



        public class WordEntry
        {
            public string Cuvant { get; set; }
            public string Categorie { get; set; }
            public string Definitie { get; set; }
        }

        
    }
}