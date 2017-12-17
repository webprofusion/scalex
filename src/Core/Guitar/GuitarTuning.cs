using System;
using System.Collections.Generic;
using Webprofusion.Scalex.Music;

namespace Webprofusion.Scalex.Guitar
{
    public class GuitarTuning
    {
        public int ID { get; set; }
        public int NumberStrings { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public NoteInstance[] TuningNotes { get; set; }

        public GuitarTuning()
        {
            //create a default tuning for use in Designer etc
            this.ID = 1;
            this.Name = "6 String Guitar - Standard";
            this.Description = "E,A,D,G,B,E";
            this.NumberStrings = 6;
            this.TuningNotes = new NoteInstance[] {
                    new NoteInstance(Note.E, 2),
                    new NoteInstance(Note.A, 2),
                    new NoteInstance(Note.D, 3),
                    new NoteInstance(Note.G, 3),
                    new NoteInstance(Note.B, 3),
                    new NoteInstance(Note.E, 4)
                };
        }
    }

    public class TuningManager
    {
        public List<GuitarTuning> AllTunings { get; set; }

        public TuningManager()
        {
            AllTunings = new List<GuitarTuning>();
            int id = 1;

            GuitarTuning t = new GuitarTuning
            {
                ID = id,
                Name = "6 String Guitar - Standard",
                Description = "E,A,D,G,B,E",
                NumberStrings = 6,
                TuningNotes = new NoteInstance[] {
                    new NoteInstance(Note.E, 2),
                    new NoteInstance(Note.A, 2),
                    new NoteInstance(Note.D, 3),
                    new NoteInstance(Note.G, 3),
                    new NoteInstance(Note.B, 3),
                    new NoteInstance(Note.E, 4)
                }
            };
            AllTunings.Add(t);
            id++;

            t = new GuitarTuning();
            t.ID = id;
            t.Name = "6 String Guitar - Drop-D";
            t.Description = "D,A,D,G,B,E";
            t.NumberStrings = 6;
            t.TuningNotes = new NoteInstance[t.NumberStrings];
            t.TuningNotes[0] = new NoteInstance(Note.D, 2);
            t.TuningNotes[1] = new NoteInstance(Note.A, 2);
            t.TuningNotes[2] = new NoteInstance(Note.D, 3);
            t.TuningNotes[3] = new NoteInstance(Note.G, 3);
            t.TuningNotes[4] = new NoteInstance(Note.B, 3);
            t.TuningNotes[5] = new NoteInstance(Note.E, 4);
            AllTunings.Add(t);
            id++;

            //Open C: c1	g1	c2	g2	c3	e3
            t = new GuitarTuning();
            t.ID = id;
            t.Name = "6 String Guitar - Open C";
            t.Description = "C,G,C,G,C,E";
            t.NumberStrings = 6;
            t.TuningNotes = new NoteInstance[t.NumberStrings];
            t.TuningNotes[0] = new NoteInstance(Note.C, 1);
            t.TuningNotes[1] = new NoteInstance(Note.G, 1);
            t.TuningNotes[2] = new NoteInstance(Note.C, 2);
            t.TuningNotes[3] = new NoteInstance(Note.G, 2);
            t.TuningNotes[4] = new NoteInstance(Note.C, 3);
            t.TuningNotes[5] = new NoteInstance(Note.E, 3);
            AllTunings.Add(t);
            id++;

            t = new GuitarTuning();
            t.ID = id;
            t.Name = "7 String Guitar - Standard-B";
            t.Description = "B,E,A,D,G,B,E";
            t.NumberStrings = 7;
            t.TuningNotes = new NoteInstance[t.NumberStrings];
            t.TuningNotes[0] = new NoteInstance(Note.B, 1);
            t.TuningNotes[1] = new NoteInstance(Note.E, 2);
            t.TuningNotes[2] = new NoteInstance(Note.A, 2);
            t.TuningNotes[3] = new NoteInstance(Note.D, 3);
            t.TuningNotes[4] = new NoteInstance(Note.G, 3);
            t.TuningNotes[5] = new NoteInstance(Note.B, 3);
            t.TuningNotes[6] = new NoteInstance(Note.E, 4);
            AllTunings.Add(t);
            id++;

            t = new GuitarTuning();
            t.ID = id;
            t.Name = "7 String Guitar - Drop-A";
            t.Description = "A,E,A,D,G,B,E";
            t.NumberStrings = 7;
            t.TuningNotes = new NoteInstance[t.NumberStrings];
            t.TuningNotes[0] = new NoteInstance(Note.A, 1);
            t.TuningNotes[1] = new NoteInstance(Note.E, 2);
            t.TuningNotes[2] = new NoteInstance(Note.A, 2);
            t.TuningNotes[3] = new NoteInstance(Note.D, 3);
            t.TuningNotes[4] = new NoteInstance(Note.G, 3);
            t.TuningNotes[5] = new NoteInstance(Note.B, 3);
            t.TuningNotes[6] = new NoteInstance(Note.E, 4);
            AllTunings.Add(t);
            id++;

            t = new GuitarTuning();
            t.ID = id;
            t.Name = "4 String Bass - Standard";
            t.Description = "E,A,D,G";
            t.NumberStrings = 4;
            t.TuningNotes = new NoteInstance[t.NumberStrings];
            //tune std down one octave;
            t.TuningNotes[0] = new NoteInstance(Note.E, 1);
            t.TuningNotes[1] = new NoteInstance(Note.A, 1);
            t.TuningNotes[2] = new NoteInstance(Note.D, 2);
            t.TuningNotes[3] = new NoteInstance(Note.G, 2);
            AllTunings.Add(t);
            id++;

            t = new GuitarTuning();
            t.ID = id;
            t.Name = "5 String Bass - Standard";
            t.Description = "B,E,A,D,G";
            t.NumberStrings = 5;
            t.TuningNotes = new NoteInstance[t.NumberStrings];
            t.TuningNotes[0] = new NoteInstance(Note.B, 0);
            t.TuningNotes[1] = new NoteInstance(Note.E, 1);
            t.TuningNotes[2] = new NoteInstance(Note.A, 1);
            t.TuningNotes[3] = new NoteInstance(Note.D, 2);
            t.TuningNotes[4] = new NoteInstance(Note.G, 2);
            t.NumberStrings = 5;
            AllTunings.Add(t);
            id++;

            t = new GuitarTuning();
            t.ID = id;
            t.Name = "5 String Bass - Drop-A";
            t.Description = "A,E,A,D,G";
            t.NumberStrings = 5;
            t.TuningNotes = new NoteInstance[t.NumberStrings];
            t.TuningNotes[0] = new NoteInstance(Note.A, 0);
            t.TuningNotes[1] = new NoteInstance(Note.E, 1);
            t.TuningNotes[2] = new NoteInstance(Note.A, 1);
            t.TuningNotes[3] = new NoteInstance(Note.D, 2);
            t.TuningNotes[4] = new NoteInstance(Note.G, 2);
            AllTunings.Add(t);
            id++;

            t = new GuitarTuning()
            {
                ID = id,
                NumberStrings = 6,
                Name = "6 String Bass - Standard",
                Description = "B,E,A,G,D,C",
                TuningNotes = new NoteInstance[] {
                    new NoteInstance(Note.B, 0),
                    new NoteInstance(Note.E, 1),
                    new NoteInstance(Note.A, 1),
                    new NoteInstance(Note.D, 2),
                    new NoteInstance(Note.G, 2),
                    new NoteInstance(Note.C, 3)
                }
            };
            AllTunings.Add(t);
            id++;

            t = new GuitarTuning()
            {
                ID = id,
                NumberStrings = 4,
                Name = "4 String Mandolin - Standard",
                Description = "G,D,A,E",
                TuningNotes = new NoteInstance[] {
                    new NoteInstance(Note.G, 3),
                    new NoteInstance(Note.D, 4),
                    new NoteInstance(Note.A, 4),
                    new NoteInstance(Note.E, 5)
                }
            };
            AllTunings.Add(t);
            id++;

            t = new GuitarTuning()
            {
                ID = id,
                NumberStrings = 8,
                Name = "8 String - Standard",
                Description = "F#,B,E,A,D,G,B,E",
                TuningNotes = new NoteInstance[] {
                    new NoteInstance(Note.Gb, 1),
                    new NoteInstance(Note.B, 1),
                    new NoteInstance(Note.E, 2),
                    new NoteInstance(Note.A, 2),
                    new NoteInstance(Note.D, 3),
                    new NoteInstance(Note.G, 3),
                    new NoteInstance(Note.B, 3),
                    new NoteInstance(Note.E, 4),
                }
            };
            AllTunings.Add(t);
            id++;

            //Ukulele G4 C4 E4 A4
            t = new GuitarTuning()
            {
                ID = id,
                NumberStrings = 4,
                Name = "4 String - Ukulele - G,C,E,A",
                Description = "G4,C4,E4,A4",
                TuningNotes = new NoteInstance[] {
                    new NoteInstance(Note.G, 4),
                    new NoteInstance(Note.C, 4),
                    new NoteInstance(Note.E, 4),
                    new NoteInstance(Note.A, 4),
                }
            };
            AllTunings.Add(t);
            id++;

            //other example tunings: http://www.howtotuneaguitar.org/tuning/alternate-guitar-tuning-chart/
        }

        public GuitarTuning GetTuning(String strTuning)
        {
            for (int i = 0; i < AllTunings.Count; i++)
            {
                if (AllTunings[i] != null)
                {
                    if (AllTunings[i].Name == strTuning) return AllTunings[i];
                }
            }
            return null;
        }

        public int Count
        {
            get
            {
                return AllTunings.Count;
            }
        }

        public List<string> GetTuningNameList()
        {
            List<string> list = new List<string>();
            foreach (GuitarTuning tuning in AllTunings)
            {
                list.Add(tuning.Name);
            }
            return list;
        }

        public string GetTuningName(int index)
        {
            if (AllTunings[index] != null) return AllTunings[index].Name;
            else return null;
        }
    }
}