namespace Chiron.Models
{
    public class Phoneme
    {
        public string Name {get;set;}
        public string PhonieCode {get;set;}
        public int Page {get;set;}
        public string CV {get;set;}
        public string VariationText {get;set;}
        public List<string> Variations { get {return VariationText.Split(",").ToList();}}

    }
}