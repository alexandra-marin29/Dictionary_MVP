using Newtonsoft.Json;
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
using System.IO;


using IO = System.IO;

namespace DictionaryApp
{

    public partial class EntertainmentControl : UserControl
    {
        private string imagesPath = "D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\Images";
        private List<WordEntry> allWords; 
        private List<WordEntry> gameWords; 
        private int currentWordIndex;
        private int correctAnswers;

        public EntertainmentControl()
        {
            InitializeComponent();
            InitializeGame();

        }
        private void SelectGameWords()
        {
            var random = new Random();
            gameWords = allWords.OrderBy(x => random.Next()).Take(5).ToList();
        }
        private void InitializeGame()
        {
            string json = File.ReadAllText("D:\\FACULTATE_AN_2\\MVP\\Dictionary_MVP\\DictionaryApp\\DictionaryApp\\Resources\\cuvinte.json");
            allWords = JsonConvert.DeserializeObject<List<WordEntry>>(json);

            SelectGameWords();
            currentWordIndex = 0;
            correctAnswers = 0;
            correctAnswersText.Text = "Răspunsuri corecte: 0";
            DisplayCurrentWord();
        }
        private void UserInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NextButton_Click(this, new RoutedEventArgs());
            }
        }


        private void DisplayCurrentWord()
        {
            var currentWord = gameWords[currentWordIndex];

            var random = new Random();
            bool showDescription = random.Next(0, 2) == 0;

            wordImage.Visibility = Visibility.Collapsed;
            wordDescription.Visibility = Visibility.Collapsed;

            currentQuestionText.Text = $"Cuvântul {currentWordIndex + 1} din {gameWords.Count}";


            if (!showDescription)
            {
                string imagePath = IO.Path.Combine(imagesPath, currentWord.Cuvant + ".png");
                if (File.Exists(imagePath))
                {
                    var imageUri = new Uri(imagePath, UriKind.Absolute);
                    wordImage.Source = new BitmapImage(imageUri);
                    wordImage.Visibility = Visibility.Visible;
                }
                else
                {
                    ShowDescription(currentWord);
                }
            }
            else
            {
                ShowDescription(currentWord);
            }
        }

        private void ShowDescription(WordEntry word)
        {
            wordDescription.Text = word.Definitie;
            wordDescription.Visibility = Visibility.Visible;
        }
        private void ResetInputAndFeedback()
        {
            userInput.Foreground = System.Windows.Media.Brushes.Gray;
            userInput.Text = "";
            feedbackText.Text = "";
            correctWordText.Text = "";
            nextButton.Content = currentWordIndex == 4 ? "Finish" : "Next";
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer();

            Task.Delay(800).ContinueWith(t =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (currentWordIndex < 4)
                    {
                        currentWordIndex++;
                        ResetInputAndFeedback();
                        DisplayCurrentWord();
                    }
                    else
                    {
                        FinishGame();
                    }
                });
            }, TaskScheduler.FromCurrentSynchronizationContext()); 
        }

        private void CheckAnswer()
        {
            var currentWord = gameWords[currentWordIndex];
            if (userInput.Text.Trim().Equals(currentWord.Cuvant, StringComparison.OrdinalIgnoreCase))
            {
                correctAnswers++;
                feedbackText.Text = "Corect!";
                feedbackText.Foreground = new SolidColorBrush(Colors.Green); 
                correctWordText.Text = ""; 
            }
            else
            {
                feedbackText.Text = "Incorect!";
                feedbackText.Foreground = new SolidColorBrush(Colors.Red);
                correctWordText.Text = $"Răspuns corect: {currentWord.Cuvant}";
            }
        }

        private void FinishGame()
        {
            MessageBox.Show("Jocul s-a încheiat!");
            ResetInputAndFeedback();
            InitializeGame();
        }
         private void UserInput_GotFocus(object sender, RoutedEventArgs e)
         {
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "Scrie răspunsul aici...")
            {
                textBox.Text = "";
                textBox.Foreground = System.Windows.Media.Brushes.Black; 
            }
         }

        private void UserInput_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Scrie răspunsul aici...";
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

    }
}
