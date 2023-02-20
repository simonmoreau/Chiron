using System.Globalization;
using System.Text;
using Chiron.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace Chiron
{
    class Program
    {
        static void Main(string[] args)
        {
            string referrenceFolder = @"C:\Users\smoreau\Github\Divers\Chiron\References";
            List<Phoneme> phonemes = new List<Phoneme>();

            using (FileStream fileStream = new FileStream($"{referrenceFolder}\\Phonemes.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fileStream, Encoding.GetEncoding("iso-8859-1")))
            {
                using (CsvReader csv = new CsvReader(reader, new CultureInfo("fr-FR")))
                {
                    phonemes = csv.GetRecords<Phoneme>().ToList();
                }
            }

            int[] pageNumbers = { 17, 18, 21, 22, 26, 27, 28, 29, 30, 32, 35, 36, 37, 38, 39, 40 };

            List<Page> pages = new List<Page>();

            foreach (int pageNumber in pageNumbers)
            {
                pages.Add(GetAvailableSylables(phonemes, pageNumber));
            }

            List<Mot> mots = new List<Mot>();
            List<CodePhonemique> codePhonemiques = new List<CodePhonemique>();

            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
            };

            using (var reader = new StreamReader($"{referrenceFolder}\\Lexique383.tsv", Encoding.GetEncoding("UTF-8")))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<MotMap>();
                mots = csv.GetRecords<Mot>().ToList();
            }

            using (var reader = new StreamReader($"{referrenceFolder}\\CodePhonemiques.csv", Encoding.GetEncoding("iso-8859-1")))
            using (var csv = new CsvReader(reader, new CultureInfo("fr-FR")))
            {
                codePhonemiques = csv.GetRecords<CodePhonemique>().ToList();
            }

            // foreach (Mot mot in mots)
            // {
            //     mot.GetWordWithoutVoidLetters(codePhonemiques);
            // }

            Parallel.ForEach(mots, mot =>
            {
                mot.GetWordWithoutVoidLetters(codePhonemiques);
            });

            // List<Task> processWordTask = new List<Task>();

            // foreach (Mot mot in mots)
            // {
            //     processWordTask.Add(Task.Run(() => mot.GetWordWithoutVoidLetters(codePhonemiques)));
            // }

            // Task.WhenAll(processWordTask);

            using (var writer = new StreamWriter($"{referrenceFolder}\\Lexique383-full.tsv"))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(mots);
            }

            foreach (Page page in pages)
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


        static bool IsMotAvailable2(Page page, Mot mot)
        {
            List<Phoneme> startingSyllables = page.AvailableSyllables.Where(s => mot.Ortho.StartsWith(s.Name)).ToList();

            return true;
        }

        static bool WordIsComposedOfSyllables(Page page, Mot mot)
        {
            int index = 0;
            while (index < mot.Ortho.Length)
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

        static bool IsMotAvailable(Page page, Mot mot)
        {
            string[] orthosyllabes = mot.OrthoSyll.Split(new string[] { "-", " " }, StringSplitOptions.None);

            string[] phonisyllabes = mot.Syll.Split("-");

            if (orthosyllabes.Count() > phonisyllabes.Count())
            {
                string lastCode = mot.Syll.Last().ToString();
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

        static Page GetAvailableSylables(List<Phoneme> phonemes, int pageNumber)
        {
            Page page = new Page();
            page.Number = pageNumber;

            List<Phoneme> consonnes = phonemes.Where(p => p.CV == "Consonne" && p.Page <= pageNumber).ToList();
            List<Phoneme> voyelles = phonemes.Where(p => p.CV == "Voyelle" && p.Page <= pageNumber).ToList();

            foreach (Phoneme consonne in consonnes)
            {
                foreach (Phoneme voyelle in voyelles)
                {
                    page.AvailableSyllables.Add(new Phoneme(consonne, voyelle));
                }
            }

            page.Consonnes.AddRange(consonnes);
            page.Voyelles.AddRange(voyelles);

            return page;
        }
    }
}