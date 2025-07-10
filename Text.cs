using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typer_Training{
    public class Text
    {
        public String TextToWrite { get; set; }
        public List<String> Words { get; set; }
        public int Mistakes { get; set; }
        public int GuessedWords { get; set; }
        public int CharactersTyped { get; set; }
        public int timeNeeded { get; set; }

        public Text(string textToWrite)
        {
            TextToWrite = textToWrite;
            Words = textToWrite.Split(' ').ToList();
            Mistakes = 0;
            GuessedWords = 0;
            CharactersTyped = 0;
        }
        public int CalculateWPM()
        {
            return (int)((CharactersTyped * 60) / (5 * timeNeeded));
        }

    }
}
