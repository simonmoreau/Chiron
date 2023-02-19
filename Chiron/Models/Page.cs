namespace Chiron.Models
{
    public class Page
    {
        public Page()
        {
            Voyelles = new List<Phoneme>();
            Consonnes = new List<Phoneme>();
            AvailableSyllables = new List<Phoneme>();
            AvailableMots = new List<Mot>();
        }
        public int Number { get; set; }
        public List<Phoneme> Voyelles { get; set; }
        public List<Phoneme> Consonnes { get; set; }
        public List<Phoneme> AvailableSyllables { get; set; }
        public List<Mot> AvailableMots { get; set; }

    }
}