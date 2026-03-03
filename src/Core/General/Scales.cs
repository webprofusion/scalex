using System;
using System.Collections;
using System.Collections.Generic;

namespace Webprofusion.Scalex.Music
{
    public class ScaleItem : IEnumerable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool[] ScaleIntervals = { true, true, true, true, true, true, true, true, true, true, true, true }; //all notes of chromatic scale
        public string Description { get; set; }

        public int NoteCount
        {
            get
            {
                int count = 0;
                foreach (bool interval in ScaleIntervals)
                {
                    if (interval == true) count++;
                }

                return count;
            }
        }

        public ScaleItem()
        {
            ID = 0;
            Name = "Chromatic";
            ScaleIntervals = new bool[] { true, true, true, true, true, true, true, true, true, true, true, true }; //all notes of chromatic scale

        }

        //populate from name and interval index i.e Major 1,3,5,6,8,10,12
        public ScaleItem(string name, int[] intervals)
        {
            Name = name;
            for (int i = 0; i < 12; i++)
            {
                ScaleIntervals[i] = false;
                if (intervals != null)
                {
                    foreach (int it in intervals)
                    {
                        if (it == i + 1) ScaleIntervals[i] = true;
                    }
                }
            }
        }

        public ScaleItem(int id, String strName, bool i1, bool i2, bool i3, bool i4, bool i5, bool i6, bool i7, bool i8, bool i9, bool i10, bool i11, bool i12, string description)
        {
            ID = id;
            Name = strName;
            Description = description;
            ScaleIntervals[0] = i1;
            ScaleIntervals[1] = i2;
            ScaleIntervals[2] = i3;
            ScaleIntervals[3] = i4;
            ScaleIntervals[4] = i5;
            ScaleIntervals[5] = i6;
            ScaleIntervals[6] = i7;
            ScaleIntervals[7] = i8;
            ScaleIntervals[8] = i9;
            ScaleIntervals[9] = i10;
            ScaleIntervals[10] = i11;
            ScaleIntervals[11] = i12;
        }

        /// <summary>
        /// get note at given scale sequence position depending on given rootNote
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rootNote"></param>
        /// <returns></returns>
        public Note GetNoteAtSequencePosition(int pos, Note rootNote)
        {
            int intervalCount = 0;
            for (int i = 0; i < 12; i++)
            {
                if (ScaleIntervals[i] == true)
                {
                    intervalCount++;
                    if (intervalCount == pos)
                    {
                        int noteIndex = i + (int)rootNote;
                        if (noteIndex >= 12) noteIndex = noteIndex - 12;

                        return (Note)noteIndex;
                    }
                }
            }

            //requested position not found (i.e pos 5 in a 3 note scale)
            throw new Exception("Note sequence not found");
        }

        /// <summary>
        /// returns the sequence number for the give scale position
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public int GetSequenceNumberInScale(int pos)
        {
            int sequenceCount = 0;
            for (int i = 0; i < 12; i++)
            {
                if (ScaleIntervals[i] == true)
                {
                    sequenceCount++;
                    if (i == pos) return sequenceCount;
                }
            }
            return -1;
        }

        public string GetIntervalNameInScale(int pos)
        {
            int sequenceCount = 0;
            for (int i = 0; i < 12; i++)
            {
                if (ScaleIntervals[i] == true)
                {
                    sequenceCount++;
                    if (i == pos)
                    {
                        switch (i + 1)
                        {
                            case 1: return "1";
                            case 2: return "b2";
                            case 3: return "2";
                            case 4: return "b3";
                            case 5: return "3";
                            case 6: return "4";
                            case 7: return "b5";
                            case 8: return "5";
                            case 9: return "b6";
                            case 10: return "6";
                            case 11: return "b7";
                            case 12: return "7";

                            default: return "";
                        }
                    }
                }
            }
            return "?";
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class ScaleManager
    {
        public ScaleItem CurrentScale = new ScaleItem();
        public Note CurrentKey = Note.E;
        public List<ScaleItem> scaleList = new List<ScaleItem>();

        public ScaleManager()
        {

            //http://en.wikipedia.org/wiki/Jazz_scale


            scaleList.Add(new ScaleItem("Major", new[] { 1, 3, 5, 6, 8, 10, 12 }) { ID = 1, Description = "Seven-note diatonic major scale (Ionian mode)." });
            scaleList.Add(new ScaleItem("Minor", new[] { 1, 3, 4, 6, 8, 9, 11 }) { ID = 2, Description = "Natural Minor (Aeolian) scale." });
            scaleList.Add(new ScaleItem("Harmonic Minor", new[] { 1, 3, 4, 6, 8, 9, 12 }) { ID = 3, Description = "Natural minor with a raised seventh degree." });
            scaleList.Add(new ScaleItem("Pentatonic Minor", new[] { 1, 4, 6, 8, 11 }) { ID = 4, Description = "Five-note minor pentatonic scale used in rock and blues." });
            scaleList.Add(new ScaleItem("Pentatonic Minor (Blues)", new[] { 1, 4, 6, 7, 8, 11 }) { ID = 5, Description = "Minor pentatonic with an added flat fifth blue note." });
            scaleList.Add(new ScaleItem("Melodic Minor", new[] { 1, 3, 4, 6, 8, 10, 12 })
            {
                ID = 6,
                Description = "The melodic minor scale is based on the natural minor with the sixth and seventh tones raised by a semitone (half step) when the scale is ascending. When the scale is descending, the melodic minor is the same as the natural minor"
            });
            scaleList.Add(new ScaleItem("Whole Tone", new[] { 1, 3, 5, 7, 9, 11 }) { ID = 8, Description = "Symmetrical six-note scale built entirely from whole steps." });
            scaleList.Add(new ScaleItem("Iwato", new[] { 1, 2, 6, 7, 11 }) { ID = 9, Description = "Japanese pentatonic scale with a tense, exotic colour." });
            scaleList.Add(new ScaleItem("Algerian", new[] { 1, 3, 4, 6, 7, 8, 9, 12 }) { ID = 10, Description = "North African scale with dramatic augmented-step character." });
            scaleList.Add(new ScaleItem("Double Harmonic Major", new[] { 1, 3, 4, 6, 7, 9, 10, 12 }) { ID = 11, Description = "Major scale with flat second and flat sixth; also called Byzantine major." });
            scaleList.Add(new ScaleItem("Persian", new[] { 1, 2, 5, 6, 7, 9, 12 }) { ID = 12, Description = "Middle Eastern-flavoured scale with strong altered tensions." });
            scaleList.Add(new ScaleItem("Byzantine", new[] { 1, 2, 5, 6, 8, 9, 12 }) { ID = 13, Description = "Double-harmonic minor flavour common in Mediterranean music." });

            scaleList.Add(new ScaleItem("Phrygian Dominant", new[] { 1, 2, 5, 6, 8, 9, 11 }) { ID = 14, Description = "Fifth mode of harmonic minor with major third and flat second." });

            scaleList.Add(new ScaleItem("Major Pentatonic", new[] { 1, 3, 5, 8, 10 }) { ID = 15, Description = "Five-note major pentatonic (major without 4th and 7th)." });
            scaleList.Add(new ScaleItem("Major Blues", new[] { 1, 3, 4, 5, 8, 10 }) { ID = 16, Description = "Major pentatonic with a blues passing tone." });
            scaleList.Add(new ScaleItem("Diminished (Whole-Half)", new[] { 1, 3, 4, 6, 7, 9, 10, 12 }) { ID = 17, Description = "Octatonic diminished scale starting with a whole step." });
            scaleList.Add(new ScaleItem("Diminished (Half-Whole)", new[] { 1, 2, 4, 5, 7, 8, 10, 11 }) { ID = 18, Description = "Octatonic diminished scale starting with a half step." });
            scaleList.Add(new ScaleItem("Augmented", new[] { 1, 4, 5, 8, 9, 12 }) { ID = 19, Description = "Symmetrical hexatonic scale based on augmented triad movement." });
            scaleList.Add(new ScaleItem("Hungarian Minor", new[] { 1, 3, 4, 7, 8, 9, 12 }) { ID = 20, Description = "Minor scale with raised 4th and raised 7th." });
            scaleList.Add(new ScaleItem("Neapolitan Minor", new[] { 1, 2, 4, 6, 8, 9, 12 }) { ID = 21, Description = "Minor scale with flattened 2nd and major 7th." });
            scaleList.Add(new ScaleItem("Neapolitan Major", new[] { 1, 2, 4, 6, 8, 10, 12 }) { ID = 22, Description = "Major-type scale featuring a flattened 2nd." });
            scaleList.Add(new ScaleItem("Enigmatic", new[] { 1, 2, 5, 7, 9, 11, 12 }) { ID = 23, Description = "Rare seven-note scale known for strong altered tensions." });
            scaleList.Add(new ScaleItem("Bebop Major", new[] { 1, 3, 5, 6, 8, 9, 10, 12 }) { ID = 24, Description = "Major scale with added passing tone for bebop phrasing." });
            scaleList.Add(new ScaleItem("Bebop Dominant", new[] { 1, 3, 5, 6, 8, 10, 11, 12 }) { ID = 25, Description = "Mixolydian with added major 7 passing tone." });
            scaleList.Add(new ScaleItem("Bebop Minor", new[] { 1, 3, 4, 5, 6, 8, 10, 11 }) { ID = 26, Description = "Minor bebop scale with chromatic passing tone." });
            scaleList.Add(new ScaleItem("Lydian Dominant", new[] { 1, 3, 5, 7, 8, 10, 11 }) { ID = 27, Description = "Lydian mode with flat seventh (melodic minor mode)." });
            scaleList.Add(new ScaleItem("Altered", new[] { 1, 2, 4, 5, 7, 9, 11 }) { ID = 28, Description = "Super Locrian altered dominant scale." });
            scaleList.Add(new ScaleItem("Egyptian Pentatonic", new[] { 1, 3, 6, 8, 11 }) { ID = 29, Description = "Suspended pentatonic common in folk and modal riffs." });
            scaleList.Add(new ScaleItem("Hirajoshi", new[] { 1, 3, 4, 8, 9 }) { ID = 30, Description = "Japanese pentatonic with a dark, tense character." });
            scaleList.Add(new ScaleItem("Kumoi", new[] { 1, 3, 4, 8, 10 }) { ID = 31, Description = "Japanese pentatonic related to Hirajoshi with brighter colour." });

            scaleList.Add(new ScaleItem("Chromatic", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }) { ID = 0, Description = "All twelve semitones in equal steps." });

            CurrentScale = scaleList[0];

        }

        public List<string> GetScaleNameList()
        {
            List<string> list = new List<string>();
            foreach (ScaleItem scale in scaleList)
            {
                list.Add(scale.Name);
            }
            return list;
        }

        public int Count
        {
            get { return scaleList.Count; }
        }

        public String GetScaleName(int index)
        {
            if (scaleList[index] != null)
            {
                return scaleList[index].Name;
            }
            return null;
        }

        public void SetScale(String strScaleName)
        {
            for (int i = 0; i < scaleList.Count; i++)
            {
                if (scaleList[i] != null)
                {
                    if (scaleList[i].Name == strScaleName)
                    {
                        CurrentScale = scaleList[i];
                        return;
                    }
                }
            }
        }

        public void SetScale(ScaleItem scaleItem)
        {
            CurrentScale = scaleItem;
        }

        public void SetKey(String myKey)
        {
            CurrentKey = NoteManager.GetNoteByName(myKey);
        }

        public String GetKeyName(bool boolShowSharps)
        {
            return NoteManager.GetNoteName(CurrentKey, boolShowSharps);
        }
    }

    public static class ScaleUtilities
    {
        public static string? GetRelativeScaleInfo(ScaleItem? scale, string? keyName, bool showSharps)
        {
            if (scale == null || string.IsNullOrWhiteSpace(keyName))
            {
                return null;
            }

            var isMajor = scale.Name.Equals("Major", StringComparison.OrdinalIgnoreCase);
            var isMinor = scale.Name.Equals("Minor", StringComparison.OrdinalIgnoreCase);

            if (!isMajor && !isMinor)
            {
                return null;
            }

            var rootNote = NoteManager.GetNoteByName(keyName);
            var relativeNote = Transpose(rootNote, isMajor ? -3 : 3);
            var relativeKeyName = GetEnhancedNoteName(relativeNote, showSharps);
            var relativeScaleName = isMajor ? "Minor" : "Major";
            var relationship = isMajor ? "Relative Minor" : "Relative Major";

            return $"{relationship}: {relativeKeyName} {relativeScaleName}";
        }

        private static Note Transpose(Note note, int semitoneOffset)
        {
            var value = ((int)note + semitoneOffset) % 12;
            if (value < 0)
            {
                value += 12;
            }

            return (Note)value;
        }

        public static string GetEnhancedNoteName(Note note, bool showSharps)
        {
            var primary = NoteManager.GetNoteName(note, showSharps);
            var alternate = NoteManager.GetNoteName(note, !showSharps);

            return string.Equals(primary, alternate, StringComparison.OrdinalIgnoreCase)
                ? primary
                : $"{primary}/{alternate}";
        }
    }
}
