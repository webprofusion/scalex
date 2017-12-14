using System;
using System.Collections.Generic;

namespace Webprofusion.Scalex.Music.Score
{
    public class MeasureSound
    {
        public decimal? Tempo { get; set; }
        public decimal? Pan { get; set; }
    }

    public class MeasureAttributes
    {
        public int divisions = 0;
        public string key_fifths = "0";
        public string key_mode = "major";
        public int time_beats = 4;
        public int time_beats_type = 4;
    }


    /// <summary>
    /// defines a specific measure within a Score, if score is Timewise, Measure contains list of Parts
    /// </summary>
    public class Measure : NoteContainer
    {
        public int Number = 0;
        public MeasureAttributes Attributes = null;
        public Dictionary<String, Part> Parts = new Dictionary<String, Part>();

        public double _DisplayWidth { get; set; }
        public double _DisplayNoteSpacing = 32;
        public int _DisplayMeasureGroup { get; set; }

        public override string ToString()
        {
            string output = "\n<measure number=\"" + Number + "\">";
            foreach (Note n in Notes)
            {
                output += n.ToString();
            }
            output += "\n</measure>\r\n";
            return output;
        }
    }
}
