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


            //  1     ♭2    2nd   ♭3     3      4    ♭5     5     ♭6     6    ♭7     7
            scaleList.Add(new ScaleItem(1, "Major",
                true, false, true, false, true, true, false, true, false, true, false, true,
                null));
            scaleList.Add(new ScaleItem(2, "Minor",
                true, false, true, true, false, true, false, true, true, false, true, false,
                "Natural Minor"));
            scaleList.Add(new ScaleItem(3, "Harmonic Minor",
                true, false, true, true, false, true, false, true, true, false, false, true,
                ""));
            scaleList.Add(new ScaleItem(4, "Pentatonic Minor",
                true, false, false, true, false, true, false, true, false, false, true, false,
                ""));
            scaleList.Add(new ScaleItem(5, "Pentatonic Minor (Blues)",
                true, false, false, true, false, true, true, true, false, false, true, false,
                ""));
            scaleList.Add(new ScaleItem(6, "Melodic Minor",
                true, false, true, true, false, true, false, true, false, true, false, true,
                    "The melodic minor scale is based on the natural minor with the sixth and seventh tones raised by a semitone (half step) when the scale is ascending. When the scale is descending, the melodic minor is the same as the natural minor"));
            scaleList.Add(new ScaleItem(8, "Whole Tone",
                true, false, true, false, true, false, true, false, true, false, true, false,
                ""));
            scaleList.Add(new ScaleItem(9, "Iwato",
                true, true, false, false, false, true, true, false, false, false, true, false,
                ""));
            scaleList.Add(new ScaleItem(10, "Algerian",
                true, false, true, true, false, true, true, true, true, false, false, true,
                ""));
            scaleList.Add(new ScaleItem(11, "Double Harmonic Major",
                true, false, true, true, false, true, true, false, true, true, false, true,
                ""));
            scaleList.Add(new ScaleItem(12, "Persian",
                true, true, false, false, true, true, true, false, true, false, false, true,
                ""));
            scaleList.Add(new ScaleItem(13, "Byzantine",
                true, true, false, false, true, true, false, true, true, false, false, true,
               ""));

            scaleList.Add(new ScaleItem(14, "Phrygian Dominant", true, true, false, false, true, true, false, true, true, false, true, false, "5th Mode of the Harmonic Minor Scale"));

            scaleList.Add(new ScaleItem(0, "Chromatic",
                true, true, true, true, true, true, true, true, true, true, true, true,
                ""));

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
}
