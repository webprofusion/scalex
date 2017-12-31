namespace Scalex.Models
{
    public class Item : BaseDataObject
    {
        private string text = string.Empty;

        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        private string description = string.Empty;

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }
    }
}