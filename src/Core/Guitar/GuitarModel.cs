using System;
using System.Collections.Generic;
using System.Linq;
using Webprofusion.Scalex.Music;

namespace Webprofusion.Scalex.Guitar
{
    public enum FretMarkerStyle
    {
        Dots = 1,
        Triangles = 2
    }

    public enum FrettingHandOrientation
    {
        LeftHand = 1,
        RightHand = 2
    }

    public class GuitarModel
    {
        //set aside space for up to 12 strings

        public GuitarString[] GuitarStrings = { null, null, null, null, null, null, null, null, null, null, null, null };
        public PrefSettings GuitarModelSettings { get; set; }

        /// <summary>
        /// Current fretboard fret marker style 
        /// </summary>
        public FretMarkerStyle FretMarkerStyle = FretMarkerStyle.Dots;

        /// <summary>
        /// fret numbers which have a fret marker present 
        /// </summary>
        public List<int> FretsWithMarkers;

        /// <summary>
        /// Hand used for fretting notes (left/right) 
        /// </summary>
        public FrettingHandOrientation FrettingHand { get; set; }

        /// <summary>
        /// number of fingers on the fretting hand for use fretting scales or chords 
        /// </summary>
        public int FrettingFingers { get; set; }

        /// <summary>
        /// imaginary hand/fret position position at which scales/chords should start 
        /// </summary>
        public int StartingFretPos { get; set; }

        /// <summary>
        /// Maximum stretch from first finger to 
        /// </summary>
        public int MaximumScalePositionStretch { get; set; }

        public int MaximumChordPositionStretch { get; set; }

        public static float DefaultScaleLength { get; set; } = 648f;

        /// <summary>
        /// Scale length on string 0 (generally lowest pitch string)
        /// </summary>
        public float PrimaryScaleLengthMM { get; set; } = DefaultScaleLength;

        /// <summary>
        /// Scale length on string MAX (generally highest pitch string)
        /// </summary>
        public float SecondaryScaleLengthMM { get; set; } = DefaultScaleLength;

        private static float DefaultMultiscaleSecondaryScaleLengthMM = 666f;

        public bool IsMultiScale
        {
            get
            {
                return PrimaryScaleLengthMM != SecondaryScaleLengthMM;
            }
            set
            {
                if (value == false)
                {
                    PrimaryScaleLengthMM = DefaultScaleLength;
                }
                else
                {
                    PrimaryScaleLengthMM = DefaultScaleLength;
                    SecondaryScaleLengthMM = DefaultMultiscaleSecondaryScaleLengthMM;
                }
            }
        }

        public float MultiScaleFanFactor
        {
            get
            {
                return (SecondaryScaleLengthMM - PrimaryScaleLengthMM) / NumberOfStrings;
            }
        }

        public int MultiScaleNeutralFret { get; set; } = 12;

        public GuitarModel()
            : this(new PrefSettings())
        {
        }

        public GuitarModel(PrefSettings appSettings)
        {
            FrettingHand = FrettingHandOrientation.LeftHand;
            FrettingFingers = 4;
            StartingFretPos = 0;
            MaximumScalePositionStretch = 6;
            MaximumChordPositionStretch = 5;
            FretsWithMarkers = new List<int> { 3, 5, 7, 9, 12, 15, 17, 19, 21, 24 };
            GuitarModelSettings = appSettings;
            RefreshStringSettings();

            //populate additional scales based on defined chord arpeggios
            int chordID = 10000;
            foreach (var chord in this.GetAllChordDefinitions())
            {
                chord.ID = chordID;
                chord.Name = chord.Name + " (Arpeggio)";
                this.AllScales.Add(chord);
                chordID++;
            }
        }

        /// <summary>
        /// Number of string on guitar 
        /// </summary>
        public int NumberOfStrings
        {
            get { return GuitarModelSettings.CurrentTuning.NumberStrings; }
        }

        public string GetDiagramTitle()
        {
            var title = $"{ GuitarModelSettings.ScaleManager.GetKeyName(GuitarModelSettings.EnableDiagramNoteNamesSharp).Trim()} {GuitarModelSettings.ScaleManager.CurrentScale.Name.Trim() } scale on {GuitarModelSettings.CurrentTuning.Name.Trim()}";
            return title;            
        }

        /// <summary>
        /// All supported tunings 
        /// </summary>
        public List<GuitarTuning> AllTunings
        {
            get { return GuitarModelSettings.TuningManager.AllTunings; }
        }

        public List<ScaleItem> AllScales
        {
            get { return GuitarModelSettings.ScaleManager.scaleList; }
        }

        public List<string> AllKeys
        {
            get
            {
                return NoteManager.NoteNames;
            }
        }

        public GuitarTuning SelectedTuning
        {
            get { return GuitarModelSettings.CurrentTuning; }
            set { SetTuning(value.ID); }
        }

        public ScaleItem SelectedScale
        {
            get { return GuitarModelSettings.ScaleManager.CurrentScale; }
            set { SetScale(value.Name); }
        }

        public string SelectedKey
        {
            get { return NoteManager.GetNoteName(GuitarModelSettings.ScaleManager.CurrentKey, true); }
            set { SetKey(value); }
        }

        public bool EnableNoteNames
        {
            get { return GuitarModelSettings.EnableDiagramNoteNames; }
            set { GuitarModelSettings.EnableDiagramNoteNames = value; }
        }

        /// <summary>
        /// Reapplies guitar string settings from current GuitarModelSettings values 
        /// </summary>
        public void RefreshStringSettings()
        {
            if (this.GuitarModelSettings == null || this.GuitarModelSettings.CurrentTuning == null) return;

            for (int i = 0; i < GuitarModelSettings.CurrentTuning.NumberStrings; i++)
            {
                if (GuitarStrings[i] == null)
                {
                    GuitarStrings[i] = new GuitarString(GuitarModelSettings);
                }
                else
                {
                    GuitarStrings[i].NumberOfFrets = GuitarModelSettings.NumberFrets;
                }

                if (GuitarModelSettings.CurrentTuning != null)
                {
                    GuitarStrings[i].OpenTuning = GuitarModelSettings.CurrentTuning.TuningNotes[i];
                }

                GuitarStrings[i].StringNumber = i;
            }
        }

        public string GetScaleIntervals()
        {
            string scaleIntervals = "";
            for (int i = 0; i < SelectedScale.ScaleIntervals.Length; i++)
            {
                if (SelectedScale.ScaleIntervals[i])
                {
                    scaleIntervals += SelectedScale.GetIntervalNameInScale(i) + "  ";
                }
            }

            return scaleIntervals;

        }

        public void SetTuning(int id)
        {
            GuitarTuning selectedTuning = null;

            foreach (var t in AllTunings)
            {
                if (t.ID == id)
                {
                    selectedTuning = t;
                    break;
                }
            }
            GuitarModelSettings.CurrentTuning = selectedTuning;
            RefreshStringSettings();
        }

        public void SetTuning(string tuning)
        {
            GuitarModelSettings.CurrentTuning = GuitarModelSettings.TuningManager.GetTuning(tuning);
            RefreshStringSettings();
        }

        public void SetScale(int id)
        {
            ScaleItem selectedScale = null;

            foreach (var t in this.AllScales)
            {
                if (t.ID == id)
                {
                    selectedScale = t;
                    break;
                }
            }

            GuitarModelSettings.ScaleManager.SetScale(selectedScale.Name);
            RefreshStringSettings();
        }

        public void SetScale(string scale)
        {
            GuitarModelSettings.ScaleManager.SetScale(scale);
            RefreshStringSettings();
        }

        public void SetKey(string noteName)
        {
            GuitarModelSettings.ScaleManager.SetKey(noteName);
            RefreshStringSettings();
        }

        public Note GetKey()
        {
            return GuitarModelSettings.ScaleManager.CurrentKey;
        }

        public void SetKey(Note note)
        {
            string noteName = Webprofusion.Scalex.Music.NoteManager.GetNoteName(note, true);
            SetKey(noteName);
        }

        public GuitarTuning GetTuningById(int id)
        {
            return AllTunings.FirstOrDefault(t => t.ID == id);
        }

        public ScaleItem GetScaleById(int id)
        {
            
            return AllScales.FirstOrDefault(t => t.ID == id);
        }

        public void SetNumberOfFrets(int numFrets)
        {
            GuitarModelSettings.NumberFrets = numFrets;
            RefreshStringSettings();
        }

        public string GetChordAsStringOld(Note rootNote, string chordType)
        {
            ChordManager chordManager = new ChordManager();
            string output = "Chord: " + rootNote.ToString() + " " + chordType + " - ";
            List<Note> notes = chordManager.GetChordNotes(rootNote, chordType, StartingFretPos);

            for (int i = 0; i < NumberOfStrings; i++)
            {
                bool noteFound = false;
                foreach (Note n in notes)
                {
                    if (!noteFound)
                    {
                        List<int> frets = GuitarStrings[i].GetNoteFretPositions(n, StartingFretPos);
                        if (frets.Count > 0)
                        {
                            if (frets[0] < 5 && !noteFound)
                            {
                                output += n.ToString() + " fret " + frets[0] + ",";
                                noteFound = true;
                            }
                        }
                    }
                }
            }

            return output;
        }

        public List<ChordDiagram> GetChordDiagramsByGroup(string groupName)
        {
            if (groupName == "popular")
            {
                return GetPopularChordDiagrams();
            }

            if (groupName == "major") groupName = "Maj";
            if (groupName == "minor") groupName = "Min";

            List<ChordDiagram> CurrentChordDiagrams = new List<ChordDiagram>();
            for (int i = 0; i < 11; i++)
            {
                CurrentChordDiagrams.Add(this.GetChordDiagram((Note)i, groupName));
            }
            return CurrentChordDiagrams;
        }

        public List<ChordDiagram> GetPopularChordDiagrams()
        {
            List<ChordDiagram> CurrentChordDiagrams = new List<ChordDiagram>();

            //like http://www.guitarchordsmagic.com/guitar-chord-charts/free-printable-guitar-chord-chart.html

            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.C, "Maj"));
            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.C, "Dom7"));
            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.D, "Maj"));
            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.D, "Min"));

            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.D, "Dom7"));
            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.E, "Maj"));
            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.E, "Min"));
            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.E, "Dom7"));

            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.F, "Maj"));
            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.G, "Maj"));
            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.G, "Dom7"));
            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.A, "Maj"));

            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.A, "Min"));
            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.A, "Dom7"));
            CurrentChordDiagrams.Add(this.GetChordDiagram(Note.B, "Dom7"));
            return CurrentChordDiagrams;
        }

        /// <summary>
        /// Get chord diagram settings based on guitar model tuning/strings etc 
        /// </summary>
        /// <param name="rootNote"></param>
        /// <param name="chordType"></param>
        /// <returns></returns>
        public ChordDiagram GetChordDiagramOld(Note rootNote, string chordType)
        {
            ChordManager chordManager = new ChordManager();
            ChordDiagram chordDiagram = new ChordDiagram();

            chordDiagram.RootNote = rootNote;
            chordDiagram.CurrentChordDefinition = chordManager.GetChordDefinition(chordType);

            //determine fretted notes per string based on current guitar model settings
            List<Note> notes = chordManager.GetChordNotes(rootNote, chordType, StartingFretPos);

            ///http://en.wikipedia.org/wiki/Chord_(music)
            bool hasChordSequenceStarted = false; //set to true once the target starting note is found, depending on the inversion (if any root, 1st pos , 2nd pos etc)

            for (int i = 0; i < NumberOfStrings; i++)
            {
                System.Diagnostics.Debug.WriteLine("Checking String:" + i);
                bool currentNoteFound = false;
                for (int n = 0; n < notes.Count; n++)
                {
                    Note chordNote = notes[n];

                    System.Diagnostics.Debug.WriteLine("Checking Note:" + chordNote.ToString());

                    if (!currentNoteFound)
                    {
                        List<int> currentNoteFrets = GuitarStrings[i].GetNoteFretPositions(chordNote, StartingFretPos);

                        if (currentNoteFrets.Count > 0)
                        {
                            if (currentNoteFrets[0] < MaximumChordPositionStretch)
                            {
                                if (!currentNoteFound)
                                {
                                    if (n == 0) hasChordSequenceStarted = true;

                                    //only add fretted note if root note has been found
                                    if (hasChordSequenceStarted)
                                    {
                                        chordDiagram.FrettedStrings[i] = new Music.FrettedNote() { Fret = currentNoteFrets[0] };
                                    }
                                    currentNoteFound = true;
                                }
                            }
                        }
                    }
                }
            }

            return chordDiagram;
        }

        public List<ChordDefinition> GetAllChordDefinitions()
        {
            return new ChordManager().ChordDefinitions;
        }

        /// <summary>
        /// returns the fretted notes of a chord based on root note and chord type 
        /// </summary>
        /// <param name="rootNote"></param>
        /// <param name="chordType"></param>
        /// <returns></returns>
        public ChordDiagram GetChordDiagram(Note rootNote, string chordType)
        {
            ChordManager chordManager = new ChordManager();
            ChordDiagram chordDiagram = new ChordDiagram();

            chordDiagram.RootNote = rootNote;
            chordDiagram.CurrentChordDefinition = chordManager.GetChordDefinition(chordType);

            //determine notes in the given chord  : http://en.wikipedia.org/wiki/Chord_(music)
            List<Note> notes = chordManager.GetChordNotes(rootNote, chordType, StartingFretPos);
            List<Note> notesFound = new List<Note>();

            //determine which string/frets would be fretted based on current guitar model settings
            bool hasChordSequenceStarted = false; //set to true once the target starting note is found, depending on the inversion (if any root, 1st pos , 2nd pos etc)

            //basic algorithm is to fret the root note of the chord on the first string possible (low to high)
            //then we fret the note on each string with the lowest fret position regardless of which interval it is in chord

            for (int currentStringIndex = 0; currentStringIndex < NumberOfStrings; currentStringIndex++)
            {
                //System.Diagnostics.Debug.WriteLine("Checking String:" + i);
                var chordNotesOnString = new List<Music.FrettedNote>();

                //get collection of notes applicable to this chord on current string
                for (int chordNoteIndex = 0; chordNoteIndex < notes.Count; chordNoteIndex++)
                {
                    var frettedNote = FindChordNote(chordDiagram, notes[chordNoteIndex], ref hasChordSequenceStarted, currentStringIndex, chordNoteIndex);
                    if (frettedNote != null) chordNotesOnString.Add(frettedNote);
                }

                //got a collection of notes for the chord on current string, decide which one to fret in chord diagram
                if (chordNotesOnString.Any())
                {
                    //if chord not yet started, use position of root/bass note on string (if any within range of fretted start position), otherwise choose note with minimum fret position
                    if (!chordDiagram.FrettedStrings.Any(c => c != null))
                    {
                        if (chordNotesOnString.Any(c => c.Note == notes[0]))
                        {
                            chordDiagram.FrettedStrings[currentStringIndex] = chordNotesOnString.Where(c => c.Note == notes[0]).First();
                        }
                    }
                    else
                    {
                        chordDiagram.FrettedStrings[currentStringIndex] = chordNotesOnString.OrderBy(f => f.Fret).First();
                    }
                }
            }

            return chordDiagram;
        }

        private Music.FrettedNote FindChordNote(ChordDiagram chordDiagram, Note chordNote, ref bool hasChordSequenceStarted, int currentStringIndex, int currentNoteIndex)
        {
            bool currentNoteFound = false;

            if (!currentNoteFound)
            {
                List<int> currentNoteFrets = GuitarStrings[currentStringIndex].GetNoteFretPositions(chordNote, StartingFretPos);

                if (currentNoteFrets.Count > 0)
                {
                    if (currentNoteFrets[0] < StartingFretPos + MaximumChordPositionStretch)
                    {
                        if (!currentNoteFound)
                        {
                            //if note is root of chord then chord sequence has started and can start fretting notes
                            if (currentNoteIndex == 0) hasChordSequenceStarted = true;

                            //only add fretted note if root note has been found
                            if (hasChordSequenceStarted)
                            {
                                currentNoteFound = true;
                                return new Music.FrettedNote() { Fret = currentNoteFrets[0], Note = chordNote };
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}