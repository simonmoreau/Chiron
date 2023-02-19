using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace Chiron.Models
{
    public class CodePhonemique
    {
        [Name("code")]
        public string code { get; set; }

        [Name("type")]
        public string type { get; set; }

        [Name("lettres")]
        public string lettres { get; set; }

        [Name("exemples")]
        public string exemples { get; set; }

        [Name("son")]
        public string son { get; set; }

        [Name("api")]
        public string api { get; set; }
    }
}