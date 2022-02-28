namespace Webprofusion.Scalex.Music
{
    using System;
    using System.Collections.Generic;
    using Webprofusion.Scalex.Util;

    public enum Note
    {
        A = 0, Bb = 1, B = 2, C = 3, Db = 4, D = 5, Eb = 6, E = 7, F = 8, Gb = 9, G = 10, Ab = 11
    }

    /// <summary>
    /// Interval type based on half steps from root note. Useful links:
    /// http://library.thinkquest.org/15413/theory/intervals.htm //http://www.guitar-chord-theory.com/intervals-scales.html
    /// </summary>
    public enum IntervalType
    {
        i01_1_Prime = 1,
        i02_b2_MinorSecond = 2,
        i03_2_MajorSecond = 3,
        i04_b3_MinorThird = 4,
        i05_3_MajorThird_DiminishedFourth = 5,
        i06_4_PerfectFourth = 6,
        i07_b5_AugmentedFourth_DiminishedFifth = 7,
        i08_5_PerfectFifth = 8,
        i09_b6_AugmentedFifth_MinorSixth = 9,
        i10_6_MajorSixth = 10,
        i11_b7_AugmentedSixth_MinorSeventh = 11,
        i12_7_MajorSeventh = 12,
        i13_PerfectOctave = 13
    }

    public class NoteInstance
    {
        public const double MiddleAFrequency = 440;
        public const int MiddleAOctave = 4;

        public Note SelectedNote { get; set; }

        /// <summary>
        /// Octave is not a defined standard but middle C on keyboard is often referred as middle C,
        /// making the lowest A on keyboard A0, concert pitch A 440Hz is A4
        /// </summary>
        public int Octave { get; set; }

        public NoteInstance(Note note, int octave)
        {
            this.SelectedNote = note;
            this.Octave = octave;
        }

        /// <summary>
        /// Get index of note in any octave if index starts at C=0 
        /// </summary>
        public int StandardAdjustedNoteIndex
        {
            get
            {
                int stdRootIndex = (int)Note.C;
                int adjustedNoteIndex = ((int)SelectedNote) - stdRootIndex;
                if (adjustedNoteIndex < 0) adjustedNoteIndex = 12 + adjustedNoteIndex;
                return adjustedNoteIndex;
            }
        }

        /// <summary>
        /// </summary>
        public int NoteIndexPosAcrossOctaves
        {
            get
            {
                return StandardAdjustedNoteIndex + (Octave * 12);
            }
        }

        public int OffsetHalfStepsFromMiddleA
        {
            get
            {
                NoteInstance referenceNote = new NoteInstance(Note.A, MiddleAOctave);
                return this.NoteIndexPosAcrossOctaves - referenceNote.NoteIndexPosAcrossOctaves;
            }
        }

        public double NoteFrequency
        {
            get
            {
                //from http://en.wikipedia.org/wiki/Note
                //f= 2 raised to power (n, divided by 10) multiplied by Middle A Frequency eg 440, where n is number of semitones from middle A

                //reference note is middle A @ 440Hz (A4)
                int halfStepsOffset = OffsetHalfStepsFromMiddleA;
                double power = (double)halfStepsOffset / 12;
                double freq = Math.Pow(2, power) * MiddleAFrequency;

                return freq;
            }
        }

        public string Name
        {
            get
            {
                return SelectedNote.ToString() + Octave;
            }
        }
    }

    public class NoteManager
    {
        public static String GetNoteName(Note myNote, bool boolUseSharps)
        {
            String strNote = "";
            string sharp = "#"; // ♯
            string flat = "b";// ♭
            switch (myNote)
            {
                case Note.Bb:
                    strNote = (boolUseSharps ? "A" + sharp : "B" + flat);
                    break;

                case Note.Db:
                    strNote = (boolUseSharps ? "C" + sharp : "D" + flat);
                    break;

                case Note.Eb:
                    strNote = (boolUseSharps ? "D" + sharp : "E" + flat);
                    break;

                case Note.Gb:
                    strNote = (boolUseSharps ? "F" + sharp : "G" + flat);
                    break;

                case Note.Ab:
                    strNote = (boolUseSharps ? "G" + sharp : "A" + flat);
                    break;

                case Note.A:
                    strNote = "A";
                    break;

                case Note.B:
                    strNote = "B";
                    break;

                case Note.C:
                    strNote = "C";
                    break;

                case Note.D:
                    strNote = "D";
                    break;

                case Note.E:
                    strNote = "E";
                    break;

                case Note.F:
                    strNote = "F";
                    break;

                case Note.G:
                    strNote = "G";
                    break;

                default:
                    strNote = myNote.ToString();
                    break;
            }

            return strNote;
        }

        public static List<string> NoteNames
        {
            get
            {
                List<string> list = new List<string>();
                for (int i = 0; i < 12; i++)
                {
                    Note myNote = (Note)i;
                    list.Add(GetNoteName(myNote, true));
                }
                return list;
            }
        }

        public static Note GetNoteByName(String strNote)
        {
            strNote = strNote.ToUpperInvariant();
            strNote = strNote.Replace("♯", "#");
            strNote = strNote.Replace("♭", "b");

            Note myNote = Note.A;
            if (strNote == "A") myNote = Note.A;
            if (strNote == "B") myNote = Note.B;
            if (strNote == "C") myNote = Note.C;
            if (strNote == "D") myNote = Note.D;
            if (strNote == "E") myNote = Note.E;
            if (strNote == "F") myNote = Note.F;
            if (strNote == "G") myNote = Note.G;

            if (strNote == "Bb" || strNote == "A#" || strNote == "A#/Bb") myNote = Note.Bb;
            if (strNote == "Db" || strNote == "C#" || strNote == "C#/Db") myNote = Note.Db;
            if (strNote == "Eb" || strNote == "D#" || strNote == "D#/Eb") myNote = Note.Eb;
            if (strNote == "Gb" || strNote == "F#" || strNote == "F#/Gb") myNote = Note.Gb;
            if (strNote == "Ab" || strNote == "G#" || strNote == "G#/Ab") myNote = Note.Ab;

            return myNote;
        }

        public static ColorValue GetNoteColour(Note note, int octave)
        {
            //http://www.colorpiano.com/
            //http://mudcu.be/piano/

            ColorValue color = new ColorValue();

            switch (note)
            {
                case Note.B:
                    color = new ColorValue { R = 255, G = 0, B = 128 };
                    break;

                case Note.Bb:
                    color = new ColorValue { R = 255, G = 0, B = 255 };
                    break;

                case Note.A:
                    color = new ColorValue { R = 128, G = 0, B = 255 };
                    break;

                case Note.Ab:
                    color = new ColorValue { R = 0, G = 0, B = 255 };
                    break;

                case Note.G:
                    color = new ColorValue { R = 0, G = 128, B = 255 };
                    break;

                case Note.Eb:
                    color = new ColorValue { R = 128, G = 255, B = 0 };
                    break;

                case Note.F:
                    color = new ColorValue { R = 0, G = 255, B = 128 };
                    break;

                case Note.E:
                    color = new ColorValue { R = 0, G = 255, B = 0 };
                    break;

                case Note.Gb:
                    color = new ColorValue { R = 0, G = 255, B = 255 };
                    break;

                case Note.D:
                    color = new ColorValue { R = 255, G = 255, B = 0 };
                    break;

                case Note.Db:
                    color = new ColorValue { R = 255, G = 128, B = 0 };
                    break;

                case Note.C:
                    color = new ColorValue { R = 255, G = 0, B = 0 };
                    break;
            }

            return color;
        }
    }
}