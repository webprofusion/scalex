using System;
using System.Collections.Generic;
using System.Linq;

namespace Webprofusion.Scalex.Music
{
    /// <summary>
    /// Represents a musical mode derived from a parent scale
    /// </summary>
    public class ModeItem
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ParentScaleId { get; set; }
        public int Degree { get; set; } // 1-7 for heptatonic scales
        public string Description { get; set; } = string.Empty;
        public string Character { get; set; } = string.Empty; // e.g., "Minor with raised 6th"

        public ModeItem() { }

        public ModeItem(int id, string name, int parentScaleId, int degree, string character, string description = "")
        {
            ID = id;
            Name = name;
            ParentScaleId = parentScaleId;
            Degree = degree;
            Character = character;
            Description = description;
        }

        /// <summary>
        /// Derives the mode's intervals by rotating the parent scale intervals
        /// </summary>
        public bool[] GetModeIntervals(ScaleItem parentScale)
        {
            return ModeUtilities.RotateIntervals(parentScale.ScaleIntervals, Degree);
        }
    }

    /// <summary>
    /// Manages available modes and their relationships to parent scales
    /// </summary>
    public class ModeManager
    {
        public List<ModeItem> ModeList { get; } = new List<ModeItem>();
        public ModeItem? CurrentMode { get; set; }

        private readonly ScaleManager _scaleManager;

        public ModeManager(ScaleManager scaleManager)
        {
            _scaleManager = scaleManager;
            InitializeModes();
        }

        private void InitializeModes()
        {
            // Major Scale Modes (Parent Scale ID = 1)
            ModeList.Add(new ModeItem(1, "Ionian", 1, 1, "Major scale", "The standard major scale"));
            ModeList.Add(new ModeItem(2, "Dorian", 1, 2, "Minor with major 6th", "Popular in jazz and rock"));
            ModeList.Add(new ModeItem(3, "Phrygian", 1, 3, "Minor with flat 2nd", "Spanish/Flamenco sound"));
            ModeList.Add(new ModeItem(4, "Lydian", 1, 4, "Major with sharp 4th", "Dreamy, floating quality"));
            ModeList.Add(new ModeItem(5, "Mixolydian", 1, 5, "Major with flat 7th", "Blues and rock sound"));
            ModeList.Add(new ModeItem(6, "Aeolian", 1, 6, "Natural Minor", "The standard natural minor scale"));
            ModeList.Add(new ModeItem(7, "Locrian", 1, 7, "Diminished", "Dark, unstable sound"));

            // Melodic Minor Modes (Parent Scale ID = 6)
            ModeList.Add(new ModeItem(8, "Melodic Minor", 6, 1, "Minor with major 6th and 7th", "Jazz minor scale"));
            ModeList.Add(new ModeItem(9, "Dorian b2", 6, 2, "Phrygian #6", "Also called Phrygian #6"));
            ModeList.Add(new ModeItem(10, "Lydian Augmented", 6, 3, "Lydian with #5", "Bright and tense"));
            ModeList.Add(new ModeItem(11, "Lydian Dominant", 6, 4, "Lydian with b7", "Overtone scale"));
            ModeList.Add(new ModeItem(12, "Mixolydian b6", 6, 5, "Hindu scale", "Also called Hindu or Aeolian Dominant"));
            ModeList.Add(new ModeItem(13, "Locrian #2", 6, 6, "Half-diminished", "Also called Aeolocrian"));
            ModeList.Add(new ModeItem(14, "Super Locrian", 6, 7, "Altered scale", "Used over altered dominant chords"));

            // Harmonic Minor Modes (Parent Scale ID = 3)
            ModeList.Add(new ModeItem(15, "Harmonic Minor", 3, 1, "Minor with major 7th", "Classical minor sound"));
            ModeList.Add(new ModeItem(16, "Locrian #6", 3, 2, "Locrian with natural 6th", ""));
            ModeList.Add(new ModeItem(17, "Ionian #5", 3, 3, "Major with augmented 5th", "Augmented major"));
            ModeList.Add(new ModeItem(18, "Dorian #4", 3, 4, "Ukrainian Dorian", "Also called Romanian scale"));
            ModeList.Add(new ModeItem(19, "Phrygian Dominant", 3, 5, "Spanish Gypsy", "Flamenco and Middle Eastern"));
            ModeList.Add(new ModeItem(20, "Lydian #2", 3, 6, "Lydian with #2", ""));
            ModeList.Add(new ModeItem(21, "Ultra Locrian", 3, 7, "Superlocrian bb7", "Also called Altered Diminished"));
        }

        /// <summary>
        /// Gets all modes for a specific parent scale
        /// </summary>
        public List<ModeItem> GetModesForScale(int scaleId)
        {
            return ModeList.Where(m => m.ParentScaleId == scaleId).OrderBy(m => m.Degree).ToList();
        }

        /// <summary>
        /// Gets a mode by its ID
        /// </summary>
        public ModeItem? GetModeById(int modeId)
        {
            return ModeList.FirstOrDefault(m => m.ID == modeId);
        }

        /// <summary>
        /// Gets a mode by its name (case-insensitive)
        /// </summary>
        public ModeItem? GetModeByName(string name)
        {
            return ModeList.FirstOrDefault(m => 
                string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Converts a mode to an equivalent ScaleItem with rotated intervals
        /// </summary>
        public ScaleItem? GetModeAsScale(ModeItem mode)
        {
            var parentScale = _scaleManager.scaleList.FirstOrDefault(s => s.ID == mode.ParentScaleId);
            if (parentScale == null) return null;

            var modeIntervals = mode.GetModeIntervals(parentScale);
            
            return new ScaleItem
            {
                ID = mode.ID + 1000, // Offset to avoid ID collision
                Name = mode.Name,
                ScaleIntervals = modeIntervals,
                Description = mode.Description
            };
        }

        /// <summary>
        /// Gets scales that support modes (have 7 notes)
        /// </summary>
        public List<ScaleItem> GetScalesWithModes()
        {
            return _scaleManager.scaleList
                .Where(s => s.NoteCount == 7 && GetModesForScale(s.ID).Any())
                .ToList();
        }
    }

    /// <summary>
    /// Utility methods for mode calculations
    /// </summary>
    public static class ModeUtilities
    {
        /// <summary>
        /// Rotates scale intervals to create mode starting from given degree
        /// </summary>
        /// <param name="scaleIntervals">The parent scale's 12-semitone interval pattern</param>
        /// <param name="startDegree">The degree to start from (1-based)</param>
        /// <returns>Rotated interval pattern</returns>
        public static bool[] RotateIntervals(bool[] scaleIntervals, int startDegree)
        {
            if (startDegree == 1)
            {
                // First mode is the same as the parent scale
                return (bool[])scaleIntervals.Clone();
            }

            // Find the positions of all notes in the scale
            var notePositions = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                if (scaleIntervals[i]) notePositions.Add(i);
            }

            if (startDegree < 1 || startDegree > notePositions.Count)
            {
                return (bool[])scaleIntervals.Clone();
            }

            // Find semitone offset for the target degree (0-indexed)
            int offset = notePositions[startDegree - 1];

            // Rotate all intervals by shifting them back by the offset
            var rotated = new bool[12];
            for (int i = 0; i < 12; i++)
            {
                if (scaleIntervals[i])
                {
                    int newPos = (i - offset + 12) % 12;
                    rotated[newPos] = true;
                }
            }

            return rotated;
        }

        /// <summary>
        /// Gets information about the parallel relationship between modes
        /// </summary>
        public static string? GetModeRelationshipInfo(ModeItem mode, string parentScaleName, string keyName)
        {
            if (mode.Degree == 1)
            {
                return null; // First mode is the parent scale itself
            }

            return $"{mode.Name} is the {GetOrdinal(mode.Degree)} mode of {parentScaleName}";
        }

        /// <summary>
        /// Determines the relative key for a mode based on parent scale
        /// </summary>
        public static string? GetRelativeKeyInfo(ModeItem mode, Note currentKey, bool showSharps)
        {
            if (mode.Degree == 1) return null;

            // Calculate intervals to go back to find the parent key
            // This depends on the specific mode and parent scale
            return null; // Can be expanded for more detailed relative key info
        }

        private static string GetOrdinal(int number)
        {
            return number switch
            {
                1 => "1st",
                2 => "2nd",
                3 => "3rd",
                _ => $"{number}th"
            };
        }
    }
}
