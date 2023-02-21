using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Chiron.Extensions;

namespace Chiron.Models
{
    public class Mot : BaseMot
    {
        private string _phonsyll;
        public string phonosyll
        {
            get { return _phonsyll; }
        }
        public string Error {get;set;}

        public List<Letter> Letters { get; set; }
        public Mot() : base()
        {
            Letters = new List<Letter>();
        }
        public void ProcessWord(List<CodePhonemique> codePhonemiques)
        {
            if (Ortho == "destin")
            {
                Console.WriteLine(Ortho);
            }

            int rankInPhon = 0;
            int rankInOrth = 0;
            List<int> syllableRanks = GetSyllableRanks();

            int syllableId = 0;
            int errorCheck = 0;
            while (rankInPhon < Phon?.Length)
            {
                if (errorCheck > 100)
                {
                    Error = "Loop";
                    break;
                }
                errorCheck++;

                List<CodePhonemique> possibleCodePhonemiques = codePhonemiques.Where(c => Phon.Substring(rankInPhon).StartsWith(c?.code)).ToList();

                if (possibleCodePhonemiques.Count == 0)
                {
                    rankInPhon++;
                    continue;
                }

                List<string> beginWith = new List<string>();

                foreach (CodePhonemique possibleCodePhonemique in possibleCodePhonemiques)
                {
                    string[] letters = possibleCodePhonemique.lettres.Split(",");
                    beginWith = letters.Where(l => Ortho.Substring(rankInOrth).StartsWith(l)).ToList();

                    if (beginWith.Count() > 0)
                    {
                        rankInPhon += possibleCodePhonemique.code.Length;
                        break;
                    }
                }

                if (rankInOrth < Ortho.Length)
                {
                    if (beginWith.Count() > 0)
                    {
                        foreach (char letter in beginWith.First())
                        {
                            Letters.Add(new Letter(letter.ToString(), false, syllableId));
                        }

                        rankInOrth += beginWith.First().Length;

                        if (syllableRanks.Contains(Letters.Count))
                        {
                            syllableId++;
                        }
                    }
                    else
                    {
                        Letters.Add(new Letter(Ortho.Substring(rankInOrth, 1), true, syllableId));
                        rankInOrth += 1;

                        if (syllableRanks.Contains(Letters.Count))
                        {
                            syllableId++;
                        }
                    }
                }
            }

            if (Letters.Count < Ortho.Count())
            {
                foreach (char remainingLetter in Ortho.Substring(Letters.Count, Ortho.Count() - Letters.Count))
                {
                    Letters.Add(new Letter(remainingLetter.ToString(), true, syllableId));
                }
            }

            if (Ortho.Last() == 'e')
            {

            }

            _phonsyll = WordWithoutSilentLetters();
        }

        private List<int> GetSyllableRanks()
        {
            List<int> syllableRanks = new List<int>();
            int rankInOrthoSyll = 0;
            while (rankInOrthoSyll < OrthoSyll?.Length)
            {
                if (OrthoSyll.Substring(rankInOrthoSyll).StartsWith("-"))
                {
                    syllableRanks.Add(rankInOrthoSyll - syllableRanks.Count);
                }
                rankInOrthoSyll++;
            }

            return syllableRanks;
        }

        private string WordWithoutSilentLetters()
        {
            string wordWithoutSilentLetters = "";
            int previousSyllabeId = 0;
            foreach (Letter letter in Letters)
            {
                if (letter.SyllableId != previousSyllabeId)
                {
                    wordWithoutSilentLetters = wordWithoutSilentLetters + "-";
                }

                if (!letter.IsSilent)
                {
                    wordWithoutSilentLetters = wordWithoutSilentLetters + letter.Text;
                }

                previousSyllabeId = letter.SyllableId;

            }

            return wordWithoutSilentLetters;
        }
    }

    public sealed class MotMap : ClassMap<Mot>
    {
        public MotMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.phonosyll).Ignore();
            Map(m => m.Error).Ignore();
        }
    }
}
