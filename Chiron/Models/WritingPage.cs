
namespace Chiron.Models
{
    public class WritingPage
    {
        private int _numberOfLines { get; set; }
        private int _numberOfLetters { get; set; }

        public WritingPage(string letter)
        {
            _numberOfLines = 8;
            _numberOfLetters = 1;

            Letter = letter;
            SetDimensions(letter);

            Lines = Enumerable.Repeat<string>(Letter, _numberOfLines).ToList();

            // Enumerable.Range(0, count)
        }

        public WritingPage(List<string> lines, string title)
        {
            Letter = title;
            Lines = lines;

            // Enumerable.Range(0, count)
        }

        public void SetDimensions(string letter)
        {
            switch (letter)
            {
                case "a":
                    CapHeight = false;
                    Descender = false;
                    break;
                case "c":
                    CapHeight = false;
                    Descender = false;
                    break;
                case "d":
                case "t":
                    CapHeight = true;
                    Descender = false;
                    break;
                case "b":
                case "l":
                    BigCapHeight = true;
                    CapHeight = false;
                    Descender = false;
                    break;
                case "q":
                    CapHeight = false;
                    Descender = true;
                    break;
                default:
                    CapHeight = false;
                    Descender = false;
                    break;

            }
        }

        public readonly List<string> Lines;
        public string Letter { get; set; }
        public bool Consonne { get; set; }
        public bool CapHeight { get; set; }
        public bool BigCapHeight { get; set; }
        public bool Descender { get; set; }
    }
}