using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DictionaryApp
{
    public class ValidationService
    {
        public bool IsValidWord(string word)
        {
            return Regex.IsMatch(word, "^[a-zA-ZăîâșțĂÎÂȘȚ-]+$");
        }

        public bool IsValidCategory(string category)
        {
            return Regex.IsMatch(category, "^[a-zA-ZăîâșțĂÎÂȘȚ ]+$");
        }

        public bool IsValidDefinition(string definition)
        {
            return Regex.IsMatch(definition, "^[a-zA-ZăîâșțĂÎÂȘȚ0-9,;.!? -]+$");
        }
    }

}
