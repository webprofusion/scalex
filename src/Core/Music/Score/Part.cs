using System;
using System.Collections.Generic;

namespace Webprofusion.Scalex.Music.Score
{

    /// <summary>
    /// Each part is a labled list of Measures which make up the complete score for given part
    /// </summary>
    public class Part : NoteContainer
    {
        public string ID = "";
        public string PartName = "";

        public List<Measure> Measures = new List<Measure>();

        public ScoreInstrument ScoreInstrument = new ScoreInstrument();
        public MidiInstrument MidiInstrument = new MidiInstrument();

        public Part()
        {

        }
    }
}
