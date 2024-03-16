using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DictionaryApp
{
    public class DataService
    {
        private string filePath;

        public DataService(string filePath)
        {
            this.filePath = filePath;
        }

        public List<WordEntry> LoadWords()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<WordEntry>>(json) ?? new List<WordEntry>();
            }
            return new List<WordEntry>();
        }

        public void SaveWords(List<WordEntry> words)
        {
            string json = JsonConvert.SerializeObject(words, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }

}
