using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace Chiron.Models
{
    public class BaseMot
    {
        [Name("ortho")]
        public string Ortho { get; set; }

        [Name("phon")]
        public string Phon { get; set; }

        [Name("lemme")]
        public string Lemme { get; set; }

        [Name("cgram")]
        public string Cgram { get; set; }

        [Name("genre")]
        public string Genre { get; set; }

        [Name("nombre")]
        public string Nombre { get; set; }

        [Name("freqlemfilms2")]
        public double? FreqLemFilms2 { get; set; }

        [Name("freqlemlivres")]
        public double? FreqLemLivres { get; set; }

        [Name("freqfilms2")]
        public double? FreqFilms2 { get; set; }

        [Name("freqlivres")]
        public double? FreqLivres { get; set; }

        [Name("infover")]
        public string Infover { get; set; }

        [Name("nbhomogr")]
        public int? NbHomogr { get; set; }

        [Name("nbhomoph")]
        public int? NbHomoph { get; set; }

        [Name("islem")]
        public int? IsLem { get; set; }

        [Name("nblettres")]
        public int? NbLettres { get; set; }

        [Name("nbphons")]
        public int? NbPhons { get; set; }

        [Name("cvcv")]
        public string CVCV { get; set; }

        [Name("p_cvcv")]
        public string PCVCVC { get; set; }

        [Name("voisorth")]
        public int? VoisOrth { get; set; }

        [Name("voisphon")]
        public int? VoisPhon { get; set; }

        [Name("puorth")]
        public int? PuOrth { get; set; }

        [Name("puphon")]
        public int? PuPhon { get; set; }

        [Name("syll")]
        public string Syll { get; set; }

        [Name("nbsyll")]
        public int? NbSyll { get; set; }

        [Name("cv-cv")]
        public string CVCVSyl { get; set; }

        [Name("orthrenv")]
        public string OrthRenv { get; set; }

        [Name("phonrenv")]
        public string PhonRenv { get; set; }

        [Name("orthosyll")]
        public string OrthoSyll { get; set; }

        [Name("cgramortho")]
        public string CgramOrtho { get; set; }

        [Name("deflem")]
        public string Deflem { get; set; }

        [Name("defobs")]
        public string DefObs { get; set; }

        [Name("old20")]
        public double? Old20 { get; set; }

        [Name("pld20")]
        public double? Pld20 { get; set; }

        [Name("morphoder")]
        public string Morphoder { get; set; }

        [Name("nbmorph")]
        public int? NbMorph { get; set; }

        public override string ToString()
        {
            return Ortho;
        }
    }
}