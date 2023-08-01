
using System.Globalization;
using System.Text;
using Chiron.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Fluid;
using Microsoft.Extensions.Configuration;

namespace Chiron
{
    public class PageProcessor
    {
        private readonly IConfiguration _configuration;

        public List<Page> Pages { get; set; }
        private List<Phoneme> _phonemes;
        public PageProcessor(IConfiguration configuration)
        {
            _configuration = configuration;
            Pages = new List<Page>();
            _phonemes = new List<Phoneme>();
        }

        public void LoadAvailablePhonemes()
        {
            string referrenceFolder = _configuration.GetSection("referrenceFolder").Get<string>() ?? "";

            using (FileStream fileStream = new FileStream($"{referrenceFolder}\\Phonemes.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fileStream, Encoding.GetEncoding("iso-8859-1")))
            {
                using (CsvReader csv = new CsvReader(reader, new CultureInfo("fr-FR")))
                {
                    _phonemes = csv.GetRecords<Phoneme>().ToList();
                }
            }
        }

        public void WritePages()
        {
            FluidParser parser = new FluidParser();

            string templateFolder = _configuration.GetSection("templateFolder").Get<string>() ?? "";

            string source = File.ReadAllText(Path.Combine(templateFolder, "index-template.html"));
            ProcessPages();
            object model = new { pages = Pages };

            if (parser.TryParse(source, out IFluidTemplate? template, out string? error))
            {
                TemplateOptions options = new TemplateOptions();
                options.MemberAccessStrategy = new UnsafeMemberAccessStrategy();

                TemplateContext context = new TemplateContext(model,options);

                string outputFolder = _configuration.GetSection("outputFolder").Get<string>() ?? "";
                string outputFile = Path.Combine(outputFolder, "Syllabes", "index.html");
                File.WriteAllText(outputFile, template.Render(context));
            }
            else
            {
                Console.WriteLine($"Error: {error}");
            }
        }

        private void ProcessPages()
        {
            
            List<int> pageNumbers = _phonemes.Select(p => p.Page).Distinct().ToList();

            for (int i = 0; i < pageNumbers.Count;)
            {
                if (pageNumbers[i] < 17)
                {
                    pageNumbers.RemoveAt(i);
                }
                else
                {
                    break;
                }
                
            }

            Pages.Clear();
            foreach (int pageNumber in pageNumbers)
            {
                Pages.Add(GetAvailableSylables(_phonemes, pageNumber));
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