
namespace Chiron.Models
{
    public class WritingPage
    {
        private int _numberOfLines {get;set;}
        private int _numberOfLetters {get;set;}

        public WritingPage(string letter)
        {
            _numberOfLines = 5;
            _numberOfLetters = 6;

            Letter = letter;

            Lines = Enumerable.Repeat<List<string>>(Enumerable.Repeat<string>(Letter,_numberOfLetters).ToList(),_numberOfLines).ToList();


            // Enumerable.Range(0, count)
        }

        public readonly List<List<string>> Lines;
        public string Letter { get; set; } 
        public bool Consonne { get; set; } 
    }
}