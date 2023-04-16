using Chiron.Utils;

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

        public List<string> RandomSyllabes
        {
            get { return PickSyllables(); }
        }

        private List<string> PickSyllables()
        {
            List<string> syllabes = new List<string>();

            RandomWeightedPicker<Phoneme> availablePhoneme = new RandomWeightedPicker<Phoneme>(AvailableSyllables);

            int lenght = 0;

            while (lenght < 20)
            {
                Phoneme phoneme = availablePhoneme.PickAnItem();
                if (syllabes.LastOrDefault() != phoneme.Name)
                {
                    syllabes.Add(phoneme.Name);
                    lenght = lenght + phoneme.Name.Length;
                }
            }

            return syllabes;
        }
    }
}