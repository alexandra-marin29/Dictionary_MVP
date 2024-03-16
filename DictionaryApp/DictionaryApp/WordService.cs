using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryApp
{
    public class WordEntry
    {
        public string Cuvant { get; set; }
        public string Categorie { get; set; }
        public string Definitie { get; set; }

        public override string ToString()
        {
            return Cuvant;
        }


    }
    

}
