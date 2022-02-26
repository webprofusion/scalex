using System;
using System.Collections.Generic;
using Webprofusion.Scalex.Music;

namespace Webprofusion.Scalex.Guitar
{
    public class SimpleRectangle
    {
        private int X { get; set; }
        private int Y { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }

        public SimpleRectangle(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }
    }

    public enum FrettedNoteStyle
    {
        Standard,
        Highlighted,
        Ghost
    }

    public enum FrettedNoteMode
    {
        Note,
        Sequence,
        ScaleIntervalFromRoot
    }

    public enum FrettedNoteFingering
    {
        Open = 0,
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Harmonic = 10
    }

    public class FrettedNote
    {
        public bool boolSelected;
        public string NoteName = "";

        public FrettedNote(int X, int Y, int Width, int Height)
        {
            boolSelected = false;
        }
    }

    public class GuitarString
    {
        public int StringNumber { get; set; }
        public NoteInstance OpenTuning = new NoteInstance(Note.E, 2);
        public List<FrettedNote> FrettedNotes;
        public int NumberOfFrets { get; set; }
        public int FretSpacing { get; set; } //TODO: move to guitar model

        public GuitarString(PrefSettings appSettings)
        {
            NumberOfFrets = appSettings.NumberFrets;
            FrettedNotes = null;
            StringNumber = 0;
        }

        public static int FretNumberToClientX(int fretNumber, GuitarModel model, int stringNumber)
        {
            // fret position = fretboardLength/1.0595^fretNumber from the bridge on a standard guitar,
            // for multiscale the fan factor decreses towards 12th (neutral) fret and inverts for higher frets
            // actual fret position per string varies depending on the scale length applicable for the given string (Primary to Secondary scale length)

            if (model.IsMultiScale)
            {


                /*var exp = (float)(fretNumber + 1) / 12;
                var pow = Math.Pow(2, exp);
                var BASS = model.PrimaryScaleLengthMM - (model.PrimaryScaleLengthMM / pow);
                var TREBLE = model.SecondaryScaleLengthMM - (model.SecondaryScaleLengthMM / pow);


               */


                double fretboardLength = model.PrimaryScaleLengthMM;
                double fretPosX = (fretboardLength - (fretboardLength / Math.Pow(1.0595, fretNumber)));
                return (int)fretPosX;

            }
            else
            {
                double fretboardLength = model.PrimaryScaleLengthMM;
                double fretPosX = (fretboardLength - (fretboardLength / Math.Pow(1.0595, fretNumber)));
                return (int)fretPosX;
            }


        }

        public int GetFretboardWidth(GuitarModel model, int stringNumber)
        {
            return GuitarString.FretNumberToClientX(NumberOfFrets, model, stringNumber);
        }

        public List<int> GetNoteFretPositions(Note note, int startingFretPosition)
        {
            List<int> noteFrets = new List<int>();

            if (startingFretPosition > NumberOfFrets) return noteFrets;

            for (int fretPos = startingFretPosition; fretPos <= NumberOfFrets; fretPos++)
            {
                //tmpNoteIndex is int value of Note enum at current fret i
                int tmpNoteIndex = fretPos + (int)OpenTuning.SelectedNote;
                if (tmpNoteIndex > 11) tmpNoteIndex = tmpNoteIndex - 12;
                if (tmpNoteIndex > 11) tmpNoteIndex = tmpNoteIndex - 12;

                if (note == (Note)tmpNoteIndex) noteFrets.Add(fretPos);
            }
            return noteFrets;
        }
    }
}