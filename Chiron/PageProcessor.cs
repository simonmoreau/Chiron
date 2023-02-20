
using System.Globalization;
using System.Text;
using Chiron.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;

namespace Chiron
{
    public class PageProcessor
    {
        public List<Page> Pages { get; set; }
        public PageProcessor(IConfiguration configuration)
        {
            string referrenceFolder = configuration.GetSection("referrenceFolder").Get<string>() ?? "";
            List<Phoneme> phonemes = new List<Phoneme>();

            using (FileStream fileStream = new FileStream($"{referrenceFolder}\\Phonemes.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fileStream, Encoding.GetEncoding("iso-8859-1")))
            {
                using (CsvReader csv = new CsvReader(reader, new CultureInfo("fr-FR")))
                {
                    phonemes = csv.GetRecords<Phoneme>().ToList();
                }
            }

            Pages = new List<Page>();

            int[] pageNumbers = { 17, 18, 21, 22, 26, 27, 28, 29, 30, 32, 35, 36, 37, 38, 39, 40 };

            foreach (int pageNumber in pageNumbers)
            {
                Pages.Add(GetAvailableSylables(phonemes, pageNumber));
            }


        }

        private Page GetAvailableSylables(List<Phoneme> phonemes, int pageNumber)
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