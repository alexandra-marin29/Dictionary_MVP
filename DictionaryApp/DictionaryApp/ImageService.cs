using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace DictionaryApp
{
    public class ImageService
    {
        private string imagesPath;

        public ImageService(string imagesPath)
        {
            this.imagesPath = imagesPath;
        }

        public BitmapImage LoadImage(string imageName)
        {
            var imagePath = Path.Combine(imagesPath, $"{imageName}.png");
            if (!File.Exists(imagePath))
            {
                // Calea pentru imaginea implicită
                imagePath = Path.Combine(imagesPath, "no_image.png");
            }

            BitmapImage bitmap = new BitmapImage();
            using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();
            }
            return bitmap;
        }


        public bool SaveImage(string sourceFilePath, string word)
        {
            var destinationPath = Path.Combine(imagesPath, $"{word}.png");

            try
            {
                // Asigură-te că imaginea precedentă nu este blocată și este eliberată
                if (File.Exists(destinationPath))
                {
                    // Încercăm să ștergem imaginea existentă
                    File.Delete(destinationPath);
                }

                // Folosind FileStream pentru a scrie noua imagine
                using (var sourceStream = File.OpenRead(sourceFilePath))
                using (var destinationStream = File.Create(destinationPath))
                {
                    sourceStream.CopyTo(destinationStream);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la salvarea imaginii");
                return false;
            }
        }




        public bool DeleteImage(string word)
        {
            try
            {
                var filePath = Path.Combine(imagesPath, $"{word}.png");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la ștergerea imaginii: {ex.Message}");
            }
            return false;
        }


    }

}
