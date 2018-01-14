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
        public SimpleRectangle displayArea;
        public bool boolSelected;
        public string NoteName = "";

        public FrettedNote(int X, int Y, int Width, int Height)
        {
            displayArea = new SimpleRectangle(X, Y, Width, Height);
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
            FretSpacing = appSettings.FretSpacing;
            FrettedNotes = null;
            StringNumber = 0;
        }

        public static int FretNumberToClientX(int fretNumber, int startX, GuitarModel guitarModel, int stringNumber)
        {
            //fret position = fretboardLength/1.0595^fretNumber from the bridge

            double fretOffset = 0;
            double fretboardLength = PrefSettings.MaxNumberFrets * guitarModel.GuitarModelSettings.FretSpacing;
            if (guitarModel.IsMultiScale)
            {
                fretboardLength = fretboardLength - (guitarModel.MultiScaleFanFactor * stringNumber);

                /*if (fretNumber != 8)
                {
                    if (fretNumber < 8)
                    {
                        fretOffset = (int)(fretNumber * guitarModel.MultiScaleFanFactor);
                    }
                    if (fretNumber > 8)
                    {
                        fretOffset = (int)(fretNumber * guitarModel.MultiScaleFanFactor);
                    }
                }*/
            }

            double fretPosX = startX + fretOffset + (fretboardLength - (fretboardLength / Math.Pow(1.0595, fretNumber)));

            return (int)fretPosX;
        }

        public int GetFretboardWidth(GuitarModel model, int stringNumber)
        {
            return GuitarString.FretNumberToClientX(NumberOfFrets, 0, model, stringNumber);
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