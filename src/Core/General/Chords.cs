using System;
using System.Collections.Generic;
using System.Linq;

namespace Webprofusion.Scalex.Music
{



    public enum ChordInversionType
    {
        RootPosition = 0,
        FirstPosition = 1,
        SecondPosition = 2
    }

    public enum ChordGroup
    {
        Common,
        Major,
        Minor,
        Dominant7th,
        Diminished,
        Augmented
    }

    public class ChordDefinition : ScaleItem
    {
        public ChordGroup ChordGroup { get; set; }
        public string ShortName { get; set; }
        public string SymbolName { get; set; }

        /// <summary>
        /// Chromatic intervals of chord definition
        /// </summary>
        public List<int> IntervalList { get; set; }
        public List<ChordDefinitionVariation> PerNoteVariations { get; set; }

        public ChordDefinition(ChordGroup chordGroup, string name, string shortname, string symbolname, int[] intervals)
            : base(name, intervals)
        {
            ChordGroup = chordGroup;
            ShortName = shortname;
            SymbolName = symbolname;


            //copy interval settings into a list instead of array
            IntervalList = new List<int>();
            if (intervals != null)
            {
                foreach (int i in intervals)
                {
                    IntervalList.Add(i);
                }
            }
        }
    }

    /// <summary>
    /// Used to defined specific variations for certain chords such as C7 which conventionally drops the 5th (8 chromatic) interval TODO: should really only apply in standard tuning?
    /// </summary>
    public class ChordDefinitionVariation
    {
        public Note VariationNote { get; set; }
        public int StartingFretPos { get; set; }
        public List<int> RemovedIntervals { get; set; }
        public List<int> AddedIntervals { get; set; }
    }

    public class FrettedNote
    {
        public int Fret { get; set; }
        public int Finger { get; set; }
        public Note Note { get; set; }
    }

    public class ChordDiagram
    {
        public ChordDefinition CurrentChordDefinition { get; set; }
        public Note RootNote { get; set; }
        public FrettedNote[] FrettedStrings = { null, null, null, null, null, null, null, null, null, null, null, null };
        public ChordInversionType InversionType { get; set; }

        public string ChordName
        {
            get
            {
                if (CurrentChordDefinition != null)
                {
                    return NoteManager.GetNoteName(RootNote, true) + CurrentChordDefinition.SymbolName;
                }
                else return "";
            }
        }

        public override string ToString()
        {
            string output = "";
            output += this.ChordName + " : ";

            for (int s = 0; s < FrettedStrings.Length; s++)
            {
                if (FrettedStrings[s] != null)
                {
                    output += FrettedStrings[s].Fret.ToString();
                }
                else
                {
                    output += "x";
                }
            }
            return output;
        }
    }

    public class ChordManager
    {
        public List<ChordDefinition> ChordDefinitions { get; set; }

        public ChordManager()
        {
            //http://johncomino.tripod.com/const.htm

            ChordDefinitions = new List<ChordDefinition>();
            ChordDefinitions.Add(new ChordDefinition(ChordGroup.Major, "Major", "Maj", "", new int[] {
                (int)IntervalType.i01_1_Prime,
                (int)IntervalType.i05_3_MajorThird_DiminishedFourth,
                (int)IntervalType.i08_5_PerfectFifth
            }));

            ChordDefinitions.Add(new ChordDefinition(ChordGroup.Minor, "Minor", "Min", "m", new int[] {
                (int)IntervalType.i01_1_Prime, (int)IntervalType.i04_b3_MinorThird, (int)IntervalType.i08_5_PerfectFifth
            }));

            ChordDefinitions.Add(new ChordDefinition(ChordGroup.Augmented, "Augmented", "Aug", "aug", new int[] {
                (int)IntervalType.i01_1_Prime,
                (int)IntervalType.i05_3_MajorThird_DiminishedFourth,
                (int)IntervalType.i09_b6_AugmentedFifth_MinorSixth
            }));

            ChordDefinitions.Add(new ChordDefinition(ChordGroup.Diminished, "Diminished", "Dim", "dim", new int[] { 1, 4, 7 }));
            ChordDefinitions.Add(new ChordDefinition(ChordGroup.Diminished, "Diminished Seventh", "Dim7", "dim7", new int[] { 1, 4, 7, 10 }));
            ChordDefinitions.Add(new ChordDefinition(ChordGroup.Diminished, "Half Diminished Seventh", "HalfDim7", "1/2dim7", new int[] { 1, 4, 7, 11 }));
            ChordDefinitions.Add(new ChordDefinition(ChordGroup.Minor, "Minor Seventh", "Min7", "m7", new int[] { 1, 4, 8, 11 }));
            ChordDefinitions.Add(new ChordDefinition(ChordGroup.Minor, "Minor Major Seventh", "mM7", "m(M7)", new int[] { 1, 4, 8, 12 }));
            ChordDefinitions.Add(new ChordDefinition(ChordGroup.Augmented, "Augmented Seventh", "Aug7", "aug7", new int[] { 1, 5, 9, 11 }));
            ChordDefinitions.Add(new ChordDefinition(ChordGroup.Augmented, "Augmented Ninth", "Aug9", "aug9", new int[] {
                (int)IntervalType.i01_1_Prime,
                (int)IntervalType.i05_3_MajorThird_DiminishedFourth,
                (int)IntervalType.i09_b6_AugmentedFifth_MinorSixth,
                (int)IntervalType.i11_b7_AugmentedSixth_MinorSeventh,
                (int)IntervalType.i02_b2_MinorSecond
        }));

            //TODO: special cases: C7 in open position drops 5th interval (8) in 1,5,8,11
            var dom7 = new ChordDefinition(ChordGroup.Dominant7th, "Dominant Seventh", "Dom7", "7", new int[] { 1, 5, 8, 11 });

            //var dom7Variation = new ChordDefinitionVariation { VariationNote = Note.C, RemovedIntervals = new int[] { (int)IntervalType.i08_5_PerfectFifth }.ToList() };
            //hack because sharpkit not allowing int .ToList on safari
            var dom7Variation = new ChordDefinitionVariation { VariationNote = Note.C };
            var removedIntervals = new int[] { (int)IntervalType.i08_5_PerfectFifth };
            dom7Variation.RemovedIntervals = new List<int>();
            for (int i = 0; i < removedIntervals.Length; i++)
            {
                dom7Variation.RemovedIntervals.Add(removedIntervals[i]);
            }

            dom7.PerNoteVariations = new List<ChordDefinitionVariation>();
            dom7.PerNoteVariations.Add(dom7Variation);
            ChordDefinitions.Add(dom7);
            ChordDefinitions.Add(new ChordDefinition(ChordGroup.Major, "Major Seventh", "Maj7", "Maj7", new int[] { 1, 5, 8, 12 }));
            ChordDefinitions.Add(new ChordDefinition(ChordGroup.Augmented, "Augmented Major Seventh", "AugMaj7", "aug M7", new int[] { 1, 5, 9, 12 }));

        }

        /// <summary>
        /// checks if computed chord has a manually specified override for fingering
        /// </summary>
        /// <param name="chord"></param>
        /// <returns></returns>
        public bool HasChordDiagramOverride(ChordDiagram chord)
        {
            return false;
        }

        public ChordDefinition GetChordDefinition(string chordType)
        {
            try
            {
                return ChordDefinitions.First(c => c.Name == chordType || c.ShortName == chordType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Note> GetChordNotes(Note rootNote, string chordType, int startingFretPos)
        {
            List<Note> notes = new List<Note>();
            try
            {
                ChordDefinition chord = ChordDefinitions.First(c => c.Name == chordType || c.ShortName == chordType);
                List<int> intervalList = chord.IntervalList.ToList();

                //check for chord variation specific to this root note
                if (chord.PerNoteVariations != null && chord.PerNoteVariations.Any(v => v.VariationNote == rootNote && v.StartingFretPos == startingFretPos))
                {
                    var variation = chord.PerNoteVariations.First(v => v.VariationNote == rootNote);
                    if (variation.RemovedIntervals != null)
                    {
                        foreach (int removedInterval in variation.RemovedIntervals)
                        {
                            intervalList.Remove(removedInterval);
                        }
                    }

                    if (variation.AddedIntervals != null)
                    {
                        foreach (int addedInterval in variation.AddedIntervals)
                        {
                            intervalList.Add(addedInterval);
                        }
                    }
                }

                for (int i = 0; i < intervalList.Count; i++)
                {
                    int intervalPos = intervalList[i] - 1;

                    int noteIndex = intervalPos + (int)rootNote;
                    if (noteIndex >= 12) noteIndex = noteIndex - 12;
                    Note n = (Note)noteIndex;

                    notes.Add(n);
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Chord Name Not Found");
            }

            return notes;
        }

        public string GetStringRepresentation(Note rootNote, string chordType)
        {
            string output = "Chord: " + rootNote.ToString() + " " + chordType + " - ";
            List<Note> notes = GetChordNotes(rootNote, chordType, 0);
            foreach (Note n in notes)
            {
                output += n.ToString() + " ";
            }
            return output;
        }
    }
}
