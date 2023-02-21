
using System.Globalization;
using System.Text;
using Chiron.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;

namespace Chiron
{
    public class WordProcessor
    {
        private readonly PageProcessor _pageProcessor;
        private readonly IConfiguration _configuration;

        public WordProcessor(PageProcessor pageProcessor, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _pageProcessor = pageProcessor;
            _configuration = configuration;
        }

        public void ProcessLexique()
        {
            string referrenceFolder = _configuration.GetSection("referrenceFolder").Get<string>() ?? "";

            List<Mot> mots = LoadLexicon("Lexique383-short");

            List<CodePhonemique> codePhonemiques = new List<CodePhonemique>();

            using (FileStream fileStream = new FileStream($"{referrenceFolder}\\CodePhonemiques.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fileStream, Encoding.GetEncoding("iso-8859-1")))
            {
                using (CsvReader csv = new CsvReader(reader, new CultureInfo("fr-FR")))
                {
                    codePhonemiques = csv.GetRecords<CodePhonemique>().ToList();
                }
            }

            foreach (Mot mot in mots)
            {
                mot.ProcessWord(codePhonemiques);
            }

            // Parallel.ForEach(mots, mot =>
            // {
            //     mot.GetWordWithoutVoidLetters(codePhonemiques);
            // });

            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
            };

            using (var writer = new StreamWriter($"{referrenceFolder}\\Lexique383-full.tsv"))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(mots);
            }

        }

        public void ShortenLexicon()
        {
            string referrenceFolder = _configuration.GetSection("referrenceFolder").Get<string>() ?? "";

            List<Mot> mots = LoadLexicon("Lexique383");

            mots = mots.OrderByDescending(m => m.FreqLivres).Take(10000).ToList();

            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
            };

            using (var writer = new StreamWriter($"{referrenceFolder}\\Lexique383-short.tsv"))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(mots);
            }
        }

        private List<Mot> LoadLexicon(string lexiconName)
        {
            List<Mot> mots = new List<Mot>();

            string referrenceFolder = _configuration.GetSection("referrenceFolder").Get<string>() ?? "";

            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
            };

            using (var reader = new StreamReader($"{referrenceFolder}\\{lexiconName}.tsv", Encoding.GetEncoding("UTF-8")))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<MotMap>();
                mots = csv.GetRecords<Mot>().ToList();
            }

            return mots;
        }

        public void ProcessPages()
        {
            string referrenceFolder = _configuration.GetSection("referrenceFolder").Get<string>() ?? "";

            List<Mot> mots = LoadLexicon("Lexique383-full");

            foreach (Page page in _pageProcessor.Pages)
            {
                foreach (Mot mot in mots)
                {
                    if (WordIsComposedOfSyllables(page, mot))
                    {
                        page.AvailableMots.Add(mot);
                    }
                }
            }
        }

        private bool WordIsComposedOfSyllables(Page page, Mot mot)
        {
            int index = 0;
            while (index < mot.Ortho?.Length)
            {
                bool foundSyllable = false;
                foreach (Phoneme syllable in page.AvailableSyllables)
                {
                    if (mot.Ortho.Substring(index).StartsWith(syllable.Name))
                    {
                        index += syllable.Name.Length;
                        foundSyllable = true;
                        break;
                    }
                }
                if (!foundSyllable)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsMotAvailable(Page page, Mot mot)
        {
            string[] orthosyllabes = (mot.OrthoSyll ?? "").Split(new string[] { "-", " " }, StringSplitOptions.None);

            string[] phonisyllabes = (mot.Syll ?? "").Split("-");

            if (orthosyllabes.Count() > phonisyllabes.Count())
            {
                string lastCode = (mot.Syll ?? "").Last().ToString();
                phonisyllabes[phonisyllabes.Count() - 1] = phonisyllabes.Last().Replace(lastCode, "");
                phonisyllabes = phonisyllabes.Append(lastCode + "e").ToArray();
            }

            if (orthosyllabes.Count() != phonisyllabes.Count())
            {
                Console.WriteLine("error");
                return false;
            }

            int i = 0;
            foreach (string orthosyllabe in orthosyllabes)
            {
                List<Phoneme> correspondingSyllables = page.AvailableSyllables.Where(s => orthosyllabe.Contains(s.Name)).ToList();
                if (correspondingSyllables.Count() == 0) return false;

                foreach (Phoneme correspondingSyllable in correspondingSyllables)
                {
                    List<Phoneme> correspondingPhonie = correspondingSyllables.Where(s => s.PhonieCodes.Contains(phonisyllabes[i])).ToList();
                    if (correspondingSyllables.Count() == 0) return false;
                }

                i++;
            }

            return true;
        }
    }
}