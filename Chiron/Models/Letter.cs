namespace Chiron.Models
{
    public class Letter
    {
        public Letter(string text, bool isSilent, int syllableId)
        {
            Text = text;
            IsSilent = isSilent;
            SyllableId = syllableId;
        }

        public string Text {get;set;}
        public bool IsSilent {get;set;}
        public int SyllableId {get;set;}
    }
}