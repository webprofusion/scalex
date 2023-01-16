using System.Collections.Generic;

namespace Webprofusion.Scalex.General
{
    public class GlossaryItem
    {
        public string Keyword { get; set; }
        public string Description { get; set; }
    }

    public class Glossary
    {
        public List<GlossaryItem> GlossaryItems { get; set; }
        public Glossary()
        {
            GlossaryItems = new List<GlossaryItem>();

            //http://www.music.vt.edu/musicdictionary/textt/Tonic.html
            GlossaryItems.Add(new GlossaryItem
            {
                Keyword = "Tonic",
                Description = "The first, root or key note of a scale or chord."
            }
                );

        }
    }
}
