namespace Webprofusion.Scalex.Music.Score
{
    public enum NoteType { Long, Breve, Whole, Half, Quarter, Eighth, Sixteenth, ThirtySecond, SixtyFourth, HundredTwentyEighth, TwoHundredAndSixtyFifth, Unknown }
    public enum Dynamics { ppp, pp, p, mp, mf, f, ff, fff, Unknown }

    public class Pitch
    {
        public char Step = 'A';
        public int Octave = 0;
    }

    public class Technical
    {
        public int String = 0;
        public int Fret = 0;
        public bool IsVibrato = false;
        public bool IsWideVibrato = false;
        public bool IsStaccato = false;
        public bool IsPalmMute = false;
        public bool LetRing = false;
        public string OtherTechnical = "";
    }

    public class Articulation
    {
        public bool IsScoop = false;
    }

    public class Notations
    {
        public Dynamics Dynamics = Dynamics.Unknown;
        public Technical Technical;
        public Articulation Articulation;

        public bool IsSlideStart = false;
        public bool IsSlideStop = false;
        public bool IsSlurStart = false;
        public bool IsSlurStop = false;

        public Notations()
        {
            Technical = new Technical();
        }

    }

    public class Note
    {

        public NoteType NoteType = NoteType.Unknown;
        public bool IsRest = false;
        public int Duration = 0;
        public int Voice = 0;

        public Pitch Pitch;
        public Notations Notations;

        public bool IsGraceNote = false;
        public bool IsChordNote = false;
        public bool IsTie = false;
        public bool IsDeadNote = false;
        public bool IsDotted = false;

        public override string ToString()
        {
            /*
             * <note><duration>12</duration>
				<voice>1</voice>
				<type>quarter</type>
				<notations><dynamics><fff></fff>
					</dynamics>
					<technical><string>1</string>
						<fret>14</fret>
					</technical>
				</notations>
			</note>
             * */
            string output = "\n\t<note>";
            output += "<duration>" + Duration + "</duration>";
            output += "<type>" + NoteType + "</type>";
            if (this.Notations.Technical != null)
            {
                output += "<technical>";
                output += "<string>" + Notations.Technical.String + "</string>";
                output += "<fret>" + Notations.Technical.Fret + "</fret>";
                output += "</technical>";
            }
            if (IsChordNote) output += "<chord/>";
            output += "</note>";
            return output;
        }

        public static decimal NoteTypeToFraction(NoteType noteType)
        {
            switch (noteType)
            {
                case NoteType.Whole:
                    return 1;
                case NoteType.Half:
                    return (decimal)0.5;
                case NoteType.Quarter:
                    return (decimal)0.25;
                case NoteType.Eighth:
                    return (decimal)1 / 8;
                case NoteType.Sixteenth:
                    return (decimal)1 / 16;
                case NoteType.ThirtySecond:
                    return (decimal)1 / 32;
                case NoteType.SixtyFourth:
                    return (decimal)1 / 64;
                case NoteType.HundredTwentyEighth:
                    return (decimal)1 / 128;
                case NoteType.TwoHundredAndSixtyFifth:
                    return (decimal)1 / 256;
            }

            return 1;
        }

        public Note()
        {
            Pitch = new Pitch();
            Notations = new Notations();
        }
#if !SCRIPTSHARP
        public Note Copy()
        {
            return (Note)this.MemberwiseClone();
        }
#endif
    }

    public class GraceNote : Note
    {

        /// <summary>
        /// true for Slashed Eighth Notes
        /// </summary>
        public bool IsSlash = false;

        /// <summary>
        /// Steal-time-previous indicates the percentage of time to steal from the previous note for the grace note.
        /// </summary>
        public float StealTimePrevious = 0;

        /// <summary>
        /// Steal-time-following indicates the percentage of time to steal from the following note for the grace note.
        /// </summary>
        public float StealTimeFollowing = 0;

        public GraceNote()
        {
            this.IsGraceNote = true;
        }
    }
}
