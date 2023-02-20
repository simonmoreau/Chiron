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
        public string phonosyll { 
            get {return _phonsyll;} 
        }

        public Mot() : base()
        {
        }
        public void GetWordWithoutVoidLetters(List<CodePhonemique> codePhonemiques)
        {
            if (Ortho == "de")
            {
                Console.WriteLine(Ortho);
            }
            string wordWithoutVoidLetters = "";
            int rankInPhon = 0;
            int rankInOrth = 0;

            List<int> syllableRanks = new List<int>();
            int rankInOrthoSyll = 0;
            while (rankInOrthoSyll < OrthoSyll.Length)
            {
                if (OrthoSyll.Substring(rankInOrthoSyll).StartsWith("-"))
                    {
                        syllableRanks.Add(rankInOrthoSyll-syllableRanks.Count);
                    }
                    rankInOrthoSyll++;
            }

            while (rankInPhon < Phon.Length)
            {
                bool foundSyllable = false;
                foreach (CodePhonemique codePhonemique in codePhonemiques)
                {
                    if (Phon.Substring(rankInPhon).StartsWith(codePhonemique.code))
                    {
                        rankInPhon += codePhonemique.code.Length;
                        string[] letters = codePhonemique.lettres.Split(",");

                        while (rankInOrth < Ortho.Length)
                        {
                            List<string> beginWith = letters.Where(l => Ortho.Substring(rankInOrth).StartsWith(l)).ToList();
                            if (beginWith.Count() > 0)
                            {
                                rankInOrth += beginWith.First().Length;
                                wordWithoutVoidLetters = wordWithoutVoidLetters + beginWith.First();
                                if (syllableRanks.Contains(rankInOrth))
                                {
                                    wordWithoutVoidLetters = wordWithoutVoidLetters + "-";
                                }
                                
                                break;
                            }
                            else
                            {
                                rankInOrth += 1;
                            }
                        }

                        break;
                    }
                }
            }

            _phonsyll = wordWithoutVoidLetters;
        }
    }

    public sealed class MotMap : ClassMap<Mot>
    {
        public MotMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.phonosyll).Ignore();
        }
    }
}