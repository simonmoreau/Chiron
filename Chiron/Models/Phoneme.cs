using Chiron.Utils;

namespace Chiron.Models
{
    public class Phoneme : IWeighted
    {
        public Phoneme()
        {

        }
        public Phoneme(Phoneme consonne, Phoneme voyelle)
        {
            Name = consonne.Name + voyelle.Name;
            PhonieCode = string.Join(",", voyelle.PhonieCodes.Select(v => consonne.PhonieCode + v).ToArray());
            Page = Math.Max(consonne.Page, voyelle.Page);
            CV = "Sylable";
            VariationText = string.Join(",", voyelle.Variations.Select(v => consonne.Name + v).ToArray());
        }
        public string Name { get; set; }
        public string PhonieCode { get; set; }
        public int Page { get; set; }
        public string CV { get; set; }
        public string VariationText { get; set; }
        public List<string> PhonieCodes
        {
            get
            {
                List<string> codes = PhonieCode.Split(",").ToList();
                return codes;
            }
        }
        public List<string> Variations
        {
            get
            {
                List<string> variations = VariationText.Split(",").ToList();
                variations.Add(Name);
                return variations;
            }
        }

        public int Weight
        {
            get {return Page;}
        }

        public override string ToString()
        {
            return Name;
        }

    }
}