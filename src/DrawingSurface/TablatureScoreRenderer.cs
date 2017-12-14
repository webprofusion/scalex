using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Webprofusion.Scalex.Music.Score;
using Webprofusion.Scalex.DataImport;

#if !SILVERLIGHT && !NETFX_CORE
using System.Drawing;
using System.Diagnostics;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

#endif

#if NETFX_CORE
using Windows.UI.Xaml.Controls;
using Windows.UI;
#endif

namespace Webprofusion.Scalex.Rendering
{

#if SILVERLIGHT
    public class Color
    {
        public static Color Black = new Color();
    }
#endif

    public class TablatureScoreRenderer : Generic2DRenderer
    {

        #region Member variables
        const int STANDARD_NOTESPACING = 16;
        const int STANDARD_NUMSTRINGS = 6;
        const int STANDARD_STRINGSPACING = 12;
        const double STANDARD_FONTSIZE = 12;
        const int STANDARD_MEASUREDIVISIONS = 8;
        public int StandardMeasuresPerRow = 4;

        /// <summary>
        /// Left margin of drawing
        /// </summary>
        private int marginX = 4;
        /// <summary>
        /// Top margin of drawing
        /// </summary>
        private int marginY = 8;

        /// <summary>
        /// Music Score to be rendered
        /// </summary>
        private MusicScore score = new MusicScore();

        /// <summary>
        /// vertical spacing between strings
        /// </summary>
        private int stringSpacing = STANDARD_STRINGSPACING;
        private double fontSize = STANDARD_FONTSIZE;

        /// <summary>
        /// measure X start position
        /// </summary>
        double measureX = 0;

        /// <summary>
        /// measure Y start position
        /// </summary>
        double measureY = 0;

        /// <summary>
        /// spacing between notes at 1/8th intervals
        /// </summary>

        private double noteSpacing = STANDARD_NOTESPACING;
        double nextNoteXPos = 0;
        int standardMeasureDivisions = STANDARD_MEASUREDIVISIONS;
        double standardMeasureWidth = 0;

        double xPos = 0;
        double yPos = 0;

        /// <summary>
        /// current beat position in measure being drawn
        /// </summary>
        int beatPos = 0;

        /// <summary>
        /// number of measures drawn in current row
        /// </summary>
        int measuresDrawn = 0;

        /// <summary>
        /// number of measure rows drawn 
        /// </summary>
        int numRows = 0;

        /// <summary>
        /// start X position of row being drawn
        /// </summary>
        int startX = 0;

        /// <summary>
        /// start Y position of row being drawn
        /// </summary>
        int startY = 0;

        /// <summary>
        /// number of strings in current being rendered in current score
        /// </summary>
        int numStrings = 0;

        /// <summary>
        /// If true, subsequent calls to render will be at next Y position instead of starting from zero
        /// </summary>
        public bool EnableSuccessiveRendering { get; set; }
#if !SILVERLIGHT && ! NETFX_CORE
        public System.Drawing.Size ScrollWindow = new System.Drawing.Size();
#endif
        public string SelectedPartID { get; set; }
        #endregion

        public TablatureScoreRenderer()
        {

        }

        #region Score loading
        public MusicScore Tablature
        {
            set
            {
                this.score = value;
            }
            get
            {
                return this.score;
            }
        }

        public void LoadMusicScore(string filename)
        {
            if (filename.EndsWith(".xml"))
            {
                //music xml
                score = new ImportMusicXML().LoadMusicXML(filename);
            }
            if (filename.EndsWith(".gp4"))
            {
                //guitar pro
                IScoreImportProvider import = new ImportGuitarPro();
                score = import.LoadScore(filename);
            }

            SelectDefaultPart();

        }

        public void LoadMusicScore(BinaryReader b)
        {
            IScoreImportProvider import = new ImportGuitarPro();
            score = import.LoadScore(b);
            SelectDefaultPart();
        }

        public void SelectDefaultPart()
        {
            if (score == null) return;

            if (score.Parts.Count > 0)
            {
                foreach (Part p in score.Parts.Values)
                {
                    SelectedPartID = p.ID;
                    break;
                }
            }
        }
        #endregion

        #region Rendering
        private void StartNextMeasureRow(double groupNoteSpacing)
        {
            //move to next row
            measuresDrawn = 0;
            numRows++;
            measureX = xPos;
            startY += 25;
            measureY = startY + (numRows * numStrings * stringSpacing);
            nextNoteXPos = measureX + groupNoteSpacing;
        }

        private void InitialiseRenderingSettings()
        {
            measureX = 0;
            measureY = 0;

            xPos = 0;
            yPos = 0;

            beatPos = 0;
            numRows = 0;
            measuresDrawn = 0;

            standardMeasureWidth = 0;

            startX = 0;
            marginY = 8;

            if (!EnableSuccessiveRendering)
            {
                startY = 0;
            }
            else
            {
                marginY = startY;
            }

            numStrings = STANDARD_NUMSTRINGS;
            nextNoteXPos = 0;
            noteSpacing = STANDARD_NOTESPACING;

        }

        public override void Render(object canvas)
        {
            Render(canvas, 800);
        }

        public void Render(object canvas, double canvasMaxWidth)
        {
            #region Initialise Canvas

            InitialiseRenderingSettings();

            //select and init rendering surface type
            IGenericDrawingSurface g = null;

#if NETFX_CORE
            if (canvas is Canvas)
            {
                g = new WPFDrawingSurface((Canvas)canvas);
                ((Canvas)canvas).Width = 864;
                ((Canvas)canvas).Height = 12000;

                foreach (GenericColorPreset p in ColorOverrides)
                {
                    ((WPFDrawingSurface)g).OveridePresetColor(p);
                }
            }
#endif
#if DESKTOP

            if (canvas is System.Windows.Controls.Canvas)
            {
                g = new WPFDrawingSurface((System.Windows.Controls.Canvas)canvas);
                ((System.Windows.Controls.Canvas)canvas).Width = 1024;
                ((System.Windows.Controls.Canvas)canvas).Height = 12000;

                foreach (GenericColorPreset p in ColorOverrides)
                {
                    ((WPFDrawingSurface)g).OveridePresetColor(p);
                }
            }
#endif
#if !SILVERLIGHT && !NETFX_CORE
            if (canvas is Graphics)
            {
                ((Graphics)canvas).CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g = new GDIDrawingSurface((Graphics)canvas);
            }

            if (canvas is PdfSharp.Pdf.PdfDocument)
            {
                // ((Graphics)canvas).CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g = new PDFDrawingSurface((PdfSharp.Pdf.PdfDocument)canvas);
                ((PDFDrawingSurface)g).RequestNewPage = true;
                noteSpacing = 12;
            }

            if (canvas is WebDrawingSurface)
            {
                g = (WebDrawingSurface)canvas;
            }
#endif

            g.Clear();
            #endregion

            //init score part selection if not already set
            if (SelectedPartID == "" || SelectedPartID == null) SelectDefaultPart();

            startX = marginX;
            startY = marginY;

            // draw song tablature
            if (this.score != null)
            {
                #region render document titles

                if (score.IsLoadedCorrectly == false || SelectedPartID == null)
                {
                    g.DrawString(10, 10, "File did not load completely.", 14, ColorPreset.Subtle);

                    if (SelectedPartID == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Could not render any parts");
                        return;
                    }
                }

                //song title (centered,large)
                //g.DrawStringCentered(startY, score.Title, canvasMaxWidth, 20);
                g.DrawString(startX, startY, score.Title, 20);
                startY += 28;

                //artist
                g.DrawString(startX, startY, score.Artist, 12);
                startY += 18;

                //album
                g.DrawString(startX, startY, score.Album, 12);
                startY += 18;

                //words and music
                //
                if (score.Author.Length > 0)
                {
                    //g.DrawStringCentered(startY, "Words & Music " + score.Author, canvasMaxWidth, 9);
                    g.DrawString(startX, startY, "Words & Music " + score.Author, 9);
                    startY += 18;
                }

                //part name
                g.DrawString(startX, startY, "Part: " + score.GetPartName(SelectedPartID), 9);

                //begin rendering score
                startY += 36;
                xPos = startX;
                yPos = startY;

                beatPos = 0;
                measuresDrawn = 0;

                //layout rules:
                //1: at 8 notes per measure a standard number of measures will fit onscreen (4)
                //2: measures are precalculated in width up to the end of the current row, this then decides the note spacing factor for the row relative
                //3: the measures for this row are then drawn and a new row starts

                //max measures per displayed row is calculated as max number of standard measures which can fit in the canvas width
                //int maxMeasuresPerRow = (int)Math.Round(canvasMaxWidth / (noteSpacing * standardMeasureDivisions));
                //if (maxMeasuresPerRow > 6) maxMeasuresPerRow = 6;

                canvasMaxWidth = GetNoteDisplayWidth(1, NoteType.Eighth, noteSpacing) * standardMeasureDivisions * StandardMeasuresPerRow;

                //int totalScoreWidth = stdMeasuresPerRow * ((int)noteSpacing * standardMeasureDivisions);
                standardMeasureWidth = (int)noteSpacing * standardMeasureDivisions;

                //TODO: need layout pass to calculate measure sizes? check tuxguitar source to see examples.

                measureY = 0;
                measureX = 0;

                int measureCount = 0;

                //Measure track pairs are stuctured Measure 1, Track 1, Measure 1, Track 2, Measure 2, Track 1 etc

                string trackID = SelectedPartID;
                if (trackID == null)
                {
                    g.DrawStringCentered(50, "No score loaded", canvasMaxWidth, 12);
                    return;
                }



                #endregion

                if (score.HasPart(trackID))
                {
                    #region render score measures for selected part (track)

                    int measureNum = score.GetPartMeasureCount(trackID);
                    double scoreRightX = 0;
                    nextNoteXPos = measureX;

                    //initialise measure display spacing
                    ArrangeMeasureDisplayGroups(score.Parts[trackID].Measures, canvasMaxWidth);

                    int measureGroup = 0;
                    for (int m = 0; m < score.Parts[trackID].Measures.Count; m++)
                    {
                        Measure measure = score.Parts[trackID].Measures[m];

                        numStrings = score.Parts[trackID].ScoreInstrument.NumStrings;

                        double measureEndX = 0;

#if DEBUG
                        //System.Diagnostics.Debug.WriteLine("Measure:" + m + " group:" + measure._DisplayMeasureGroup + " spacing: " + measure._DisplayNoteSpacing + " : width:" + measure._DisplayWidth);
                        //System.Diagnostics.Debug.WriteLine(measure.ToString());
#endif
                        noteSpacing = measure._DisplayNoteSpacing;
                        if (measureGroup == measure._DisplayMeasureGroup)
                        {
                            measureX = nextNoteXPos;// +measure._DisplayNoteSpacing;
                            measureY = startY + (numRows * numStrings * stringSpacing);
                            beatPos = 0;
                        }
                        else
                        {
                            measureGroup++;
                            StartNextMeasureRow(measure._DisplayNoteSpacing);
                            beatPos = 0;
                        }

                        nextNoteXPos = measureX;

                        if (measuresDrawn == 0)
                        {
                            measureX = xPos;

                            //Draw strings
                            int stringy = 0;

                            scoreRightX = xPos + canvasMaxWidth; //measureX + scoreWidth;//(standardMeasureLength * stdMeasuresPerRow);

                            for (int i = 0; i < numStrings; i++)
                            {
                                g.DrawLine(measureX, measureY + stringy, scoreRightX, measureY + stringy, 1, ColorPreset.Foreground);
                                stringy += stringSpacing;
                            }

                            //draw bar decoration
                            //left
                            if (numRows == 0)
                            {
                                g.DrawLine(measureX, measureY, measureX, measureY + ((numStrings - 1) * stringSpacing), 3, ColorPreset.Foreground);
                                g.DrawLine(measureX + 3, measureY, measureX + 3, measureY + ((numStrings - 1) * stringSpacing), 1, ColorPreset.Foreground);
                            }
                            else
                            {
                                g.DrawLine(measureX, measureY, measureX, measureY + ((numStrings - 1) * stringSpacing), 1, ColorPreset.Foreground);
                            }

                            //right
                            g.DrawLine(scoreRightX, measureY, scoreRightX, measureY + ((numStrings - 1) * stringSpacing), 1, ColorPreset.Foreground);

                            //draw tempo: TODO: get tempo from first measure if not specified
                            if (measure.Sound != null)
                            {
                                if (measure.Sound.Tempo != null)
                                {
                                    g.DrawString(measureX + 36, measureY - (STANDARD_STRINGSPACING * 2), "t = " + measure.Sound.Tempo.ToString(), fontSize);
                                }
                            }

                            //draw measure number
                            g.DrawString(measureX, measureY - (STANDARD_STRINGSPACING * 2), (measureCount + 1).ToString(), fontSize, ColorPreset.Accent);
                        }
                        else
                        {
                            g.DrawLine(measureX, measureY, measureX, measureY + ((numStrings - 1) * stringSpacing), 1, ColorPreset.Foreground);
                        }

                        measuresDrawn++;

                        //render notes in measure

                        bool measureIsRest = true;
                        nextNoteXPos = measureX;

                        double previousNoteXEndPos = 0;
                        double previousNoteWidth = 0;
                        //bool isPreviousNoteAChord =false;

                        for (int noteNum = 0; noteNum < measure.Notes.Count; noteNum++)
                        {
                            Note note = measure.Notes[noteNum];

                            if (!note.IsChordNote)
                            {
                                beatPos++;
                                nextNoteXPos = previousNoteXEndPos;
                            }
                            else
                            {
                                //jump back to chord start pos for rest of chord notes
                                nextNoteXPos = nextNoteXPos - previousNoteWidth;
                            }

                            if (!note.IsRest)
                            {
                                int stringNum = note.Notations.Technical.String;
                                int fretNum = note.Notations.Technical.Fret;

                                //System.Diagnostics.Debug.WriteLine("note xpos:"+nextNoteXPos);
                                previousNoteXEndPos = DrawFrettedNote(g, stringNum - 1, beatPos, fretNum, note, nextNoteXPos, measure._DisplayNoteSpacing);
                                previousNoteWidth = previousNoteXEndPos - nextNoteXPos;
                                nextNoteXPos = previousNoteXEndPos;

                                measureIsRest = false;
                            }
                            else
                            {
                                if (note.Duration < 4)
                                {
                                    DrawRest(g, beatPos, note);
                                }

                                //rest
                                nextNoteXPos += GetNoteDisplayWidth(note, measure._DisplayNoteSpacing);
                                beatPos += standardMeasureDivisions;
                            }

                            //if (note.IsChordNote) isPreviousNoteAChord = true;
                        }

                        if (measureIsRest)
                        {
                            //draw bar rest
                            g.DrawLine(measureX + 20, measureY + 20, measureX + 40, measureY + 20, 6, ColorPreset.Foreground);

                            nextNoteXPos += 4 * (int)noteSpacing; ; //standardMeasureDivisions * (int)noteSpacing;
                        }

                        //if beatPos is less than division (so less than 8 notes rendered, advance beatpos)
                        if (beatPos < standardMeasureDivisions) beatPos = standardMeasureDivisions;

                        measureEndX = measureX + measure._DisplayWidth; //nextNoteXPos;

                        //draw end of measure
                        // g.DrawLine(measureEndX, measureY, measureEndX, measureY + ((numStrings - 1) * stringSpacing), 1, Color.Black.ToString());

                        measureCount++;
                    }
                    #endregion
                }
                
#if !SILVERLIGHT && !NETFX_CORE
                //update scroll region
                this.ScrollWindow = new System.Drawing.Size(0, startY + ((numRows + 1) * numStrings * stringSpacing));
#endif
            }
        }


        private void DrawRest(IGenericDrawingSurface g, int timePos, Note note)
        {
            int leftPadding = 4;

            double xRestPos = leftPadding + (measureX + ((timePos - 1) * noteSpacing));
            double yRestPos = 30;

            g.DrawString(xRestPos, yRestPos, "r");

            if (note.IsDotted)
            {
                g.DrawString(xRestPos + 10, yRestPos, "dotted");
            }
        }

        private double DrawFrettedNote(IGenericDrawingSurface g, int numString, int timePos, int numFret, Note note, double previousNoteXPos, double groupNoteSpacing)
        {

            double vspacing = 3;

            //calc position of this note, based on end of previous note
            double xNotePos = measureX + groupNoteSpacing;

            double noteDisplayWidth = GetNoteDisplayWidth(note, groupNoteSpacing);

            //if previous note drawn in measure, start from there
            if (previousNoteXPos > measureX)
            {
                xNotePos = previousNoteXPos;
            }

            double yNotePos = (measureY + (numString * stringSpacing));// -(fontSize * 0.5);

            //draw masking rectangle behind text to improve readability
            double notationWidth = (numFret.ToString().Length * (fontSize * 0.75));
            g.FillRectangle(xNotePos, yNotePos + (fontSize * 0.5), notationWidth, (fontSize), ColorPreset.Background, ColorPreset.Background);

            //draw fret number on string
            g.DrawString(xNotePos, yNotePos + (fontSize * 0.5), numFret.ToString(), fontSize);

            //draw notations
            if (note.Notations != null)
            {
                //draw slur start
                if (note.Notations.IsSlurStart)
                {
                    g.DrawArc(xNotePos + vspacing, yNotePos - vspacing, groupNoteSpacing, true);
                }

                if (note.Notations.IsSlideStart)
                {
                    g.DrawLine((xNotePos + fontSize), (yNotePos + fontSize), (xNotePos + groupNoteSpacing), (yNotePos + fontSize), 2, ColorPreset.Foreground);
                }

                if (note.Notations.Technical.IsVibrato)
                {
                    double x1 = xNotePos + vspacing;
                    double y1 = measureY - STANDARD_STRINGSPACING;
                    string vibratoString = "~";

                    for (int i = 0; i < note.Duration; i++)
                    {
                        vibratoString += "~";
                    }

                    g.DrawString(x1, y1, vibratoString, STANDARD_FONTSIZE * 2);
                }
            }

            xNotePos += noteDisplayWidth;
            return xNotePos;
        }

        private double GetNoteDisplayWidth(Note n, double groupNoteSpacing)
        {
            return GetNoteDisplayWidth(n.Duration, n.NoteType, groupNoteSpacing);
        }

        /// <summary>
        /// Get display width of rendered note
        /// </summary>
        /// <param name="Duration"></param>
        /// <param name="Type"></param>
        /// <param name="groupNoteSpacing"></param>
        /// <returns></returns>
        private double GetNoteDisplayWidth(int Duration, NoteType Type, double groupNoteSpacing)
        {
            //space note based on combination of note type (1/8th/16th etc of measure) and duration
            double measureSpacing = groupNoteSpacing * standardMeasureDivisions;
            double noteSpace = (groupNoteSpacing / 2) + Duration * ((double)Note.NoteTypeToFraction(Type)) * measureSpacing;

            return noteSpace;
        }

        private void ArrangeMeasureDisplayGroups(List<Measure> measures, double maxRowWidth)
        {
            //arranges measures into display groups and sets a note spacing based on fitting the measures into a single row

            int measuresInGroup = 0;
            int measureGroup = 0;
            double standardNoteSpacing = noteSpacing;
            double measureGroupExtentX = 0;
            int lastGroupEndIndex = -1;

            for (int m = 0; m < measures.Count; m++)
            {
                Measure measure = measures[m];

                //get standard display width of this measure
                CalculateMeasureWidth(measure, standardNoteSpacing);

                measureGroupExtentX += measure._DisplayWidth;

                //if width of row would exceed limit when added to this row, end this group, normalise the group spacing  and start another
                if (measureGroupExtentX + (standardMeasureWidth * 2) >= maxRowWidth)
                {
                    //all measures in previous measure group get normalised to note spacing based on max width

                    int groupStartIndex = lastGroupEndIndex + 1;
                    if (lastGroupEndIndex < 0) groupStartIndex = 0;

                    int groupEndIndex = groupStartIndex + (measuresInGroup);
                    lastGroupEndIndex = groupEndIndex;

                    double normalisedNoteSpacing = (maxRowWidth / measureGroupExtentX) * standardNoteSpacing;
                    double normalisedRowWidth = 0;

                    for (int i = groupStartIndex; i <= groupEndIndex; i++)
                    {
                        Measure d = measures[i];
                        d._DisplayNoteSpacing = normalisedNoteSpacing;

                        //reset display width to new normalised value
                        d._DisplayWidth = d._DisplayWidth * (maxRowWidth / measureGroupExtentX); ;
                        normalisedRowWidth += d._DisplayWidth;
                    }
#if DEBUG
                    //System.Diagnostics.Debug.WriteLine("Measures: " + (groupStartIndex + 1) + " to " + (groupEndIndex + 1) + " spacing:" + normalisedNoteSpacing + " width:" + normalisedRowWidth);
#endif

                    measure._DisplayMeasureGroup = measureGroup;

                    measuresInGroup = 0;
                    measureGroupExtentX = 0;
                    measureGroup++;
                }
                else
                {
                    measure._DisplayMeasureGroup = measureGroup;
                    measure._DisplayNoteSpacing = standardNoteSpacing;
                    measuresInGroup++;
                }
            }
        }

        /// <summary>
        /// Calculate display width of given measure
        /// </summary>
        /// <param name="m"></param>
        /// <param name="groupNoteSpacing"></param>
        private double CalculateMeasureWidth(Measure m, double groupNoteSpacing)
        {
            double width = 0;

            //add measure start left margin
            width += groupNoteSpacing;

            //add measure reset (if any)
            if (m.Notes.Count == 0)
            {
                //is measure rest, space for 4 quarter notes
                width += GetNoteDisplayWidth(4, NoteType.Quarter, groupNoteSpacing);
            }

            //double chordWidth = 0;
            double noteWidth = 0;

            foreach (Note n in m.Notes)
            {
                //for each note, add display width
                noteWidth = GetNoteDisplayWidth(n, groupNoteSpacing);

                if (!n.IsChordNote) width += noteWidth;
            }

            //add measure right margin
            width += groupNoteSpacing;

            m._DisplayWidth = width;

            return width;
        }

        #endregion

#if !SILVERLIGHT && !NETFX_CORE

        public string RenderHTML()
        {
            WebDrawingSurface g = new WebDrawingSurface();
            this.Render(g, 900);
            return g.GetHTMLDocument();
        }

        public void RenderPDF(string FileName)
        {
            PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();

            // this.EnableSuccessiveRendering = true;

            doc.Info.Title = Tablature.Title;
            doc.Info.Author = Tablature.Artist;
            doc.Info.Subject = Tablature.Album;
            doc.Info.Keywords = Tablature.Comments;
            foreach (Part p in score.Parts.Values)
            {
                SelectedPartID = p.ID;

                if (doc.Pages.Count > 0)
                {
                    PdfOutline outline = new PdfOutline(p.PartName, doc.Pages[doc.Pages.Count - 1], true);
                    doc.Outlines.Add(outline);
                }
                Render(doc, 600);
            }

            // Save the document...
            doc.Save(FileName);

        }
#endif
        public void RenderPNG(string FileName)
        {
#if !SILVERLIGHT && !NETFX_CORE
            Bitmap bmp = new Bitmap(900, 300);
            Graphics g = Graphics.FromImage(bmp);

            this.Render(g, 900);

            //save png
            bmp.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);

            bmp.Dispose();
            g.Dispose();
#endif
        }
    }
}
