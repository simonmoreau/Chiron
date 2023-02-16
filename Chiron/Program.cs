using System.Globalization;
using System.Text;
using Chiron.Models;
using CsvHelper;

namespace Chiron
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FileStream fileStream = new FileStream(@"..\References\Phonemes.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fileStream, Encoding.GetEncoding("iso-8859-1")))
            {
                using (CsvReader csv = new CsvReader(reader, new CultureInfo("fr-FR")))
                {
                    List<Phoneme> phonemes = csv.GetRecords<Phoneme>().ToList();
                }
            }
        }
    }
}