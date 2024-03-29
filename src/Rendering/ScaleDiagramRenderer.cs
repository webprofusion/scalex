using System;
using System.Collections.Generic;
using System.Linq;
using Webprofusion.Scalex.Guitar;
using Webprofusion.Scalex.Music;
using Webprofusion.Scalex.Util;

namespace Webprofusion.Scalex.Rendering
{
    /// <summary>
    /// Renders the fretboard scale diagram for the current settings to the provided Graphics context 
    /// </summary>
    public class ScaleDiagramRenderer : Generic2DRenderer
    {
        private GuitarModel _guitarModel = null;
        public const int BasicFontSizePt = 5;
        public int PaddingLeft { get; set; } = 10;
        public int PaddingRight { get; set; } = 10;
        public int PaddingTop { get; set; } = 20;

        public bool IsExportMode { get; set; }

        public struct NoteItem
        {
            public int X;
            public int Y;
            public Note Note;
            public int FretNumber;
            public int StringNumber;
            public int Octave;
            public bool IsHighlighted;

        }

        private List<NoteItem> _noteList = new List<NoteItem>();
        private List<NoteItem> _hightlightedNotes = new List<NoteItem>();

        public ScaleDiagramRenderer(GuitarModel guitarModel)
        {
            _guitarModel = guitarModel;
        }

        public void SetGuitarModel(GuitarModel guitarModel)
        {
            _guitarModel = guitarModel;
        }

        public int GetFretboardHeight()
        {
            if (_guitarModel != null)
            {
                return _guitarModel.GuitarModelSettings.StringSpacing * (_guitarModel.NumberOfStrings - 1);
            }
            return 0;
        }

        public NoteItem? GetNoteAtPoint(double x, double y)
        {
            var noteSize = _guitarModel.GuitarModelSettings.MarkerSize;
            foreach (var note in _noteList)
            {

                if (x >= note.X && x <= note.X + noteSize)
                {
                    if (y >= note.Y && y <= note.Y + noteSize)
                    {
                        return note;
                    }

                }
            }

            return null;
        }

        public void HighlightNote(NoteItem note)
        {
            for (var i = 0; i < _noteList.Count; i++)
            {
                var n = _noteList[i];
                if (n.StringNumber == note.StringNumber && n.FretNumber == note.FretNumber)
                {
                    n.IsHighlighted = true;
                    if (!_hightlightedNotes.Contains(n)) _hightlightedNotes.Add(n);
                }
            }
        }

        public int GetDiagramWidth()
        {
            if (_guitarModel != null)
            {
                return PaddingLeft
                    + GuitarString.FretNumberToClientX(_guitarModel.GuitarModelSettings.NumberFrets, _guitarModel, 0)
                    + PaddingRight;
            }
            return 0;
        }

        public override void Render(IGenericDrawingSurface canvas)
        {
            if (EnableRendering == false) return;

            //select and init rendering surface type
            IGenericDrawingSurface g = InitialiseDrawingSurface(canvas);

            int numberOfStrings = _guitarModel.GuitarModelSettings.CurrentTuning.NumberStrings;
            int numberFrets = _guitarModel.GuitarModelSettings.NumberFrets;

            int startX = PaddingLeft;
            int startY = PaddingTop;

            _noteList = new List<NoteItem>();

            ColorValue defaultCanvasFontColor = ColorPalette[ThemeColorPreset.Foreground];

            int fboardWidth = GuitarString.FretNumberToClientX(_guitarModel.GuitarModelSettings.NumberFrets, _guitarModel, 0);

            int fboardHeight = _guitarModel.GuitarModelSettings.StringSpacing * (numberOfStrings - 1);

            if (_guitarModel.GuitarModelSettings.EnableFretboardBackgroundFill)
            {
                // optional background fretboard fill
                g.FillRectangle(startX, startY + _guitarModel.GuitarModelSettings.StringSpacing, fboardWidth, fboardHeight, ColorPalette[ThemeColorPreset.Background], ColorPalette[ThemeColorPreset.Subtle]);
            }

            //draw frets..
            DrawFrets(numberOfStrings, g, startX, startY);

            //draw strings (and note markers)..
            startY = PaddingTop + (_guitarModel.GuitarModelSettings.StringSpacing * numberOfStrings);
            startY = DrawGuitarStrings(numberOfStrings, g, PaddingLeft, startY);

            //draw fret numbers
            if (_guitarModel.GuitarModelSettings.EnableFretNumbers)
            {
                startX = PaddingLeft;
                startY = PaddingTop + 5 + _guitarModel.GuitarModelSettings.StringSpacing * (numberOfStrings);

                DrawFretNumbers(numberFrets, g, startX, startY, defaultCanvasFontColor);
            }

            //show scale intervals & notes
            var scale = _guitarModel.SelectedScale;

            DrawScaleFormulaIntervals(g, PaddingLeft + 4, startY, scale, defaultCanvasFontColor);
            DrawScaleNoteList(g, PaddingLeft + 4, startY, scale, defaultCanvasFontColor);

            if (IsExportMode)
            {
                //draw copyright/watermark (for exported images)
                g.DrawString(PaddingLeft + 5, startY + 10, "Generated by Guitar Toolkit - Copyright " + System.DateTime.Now.Year + " Soundshed.com");
            }

            startX = 0;
            startY = 10;

            //draw scale title
            if (_guitarModel.GuitarModelSettings.EnableDiagramTitle)
            {
                var title = _guitarModel.GetDiagramTitle();
                g.DrawString(startX, startY, title, 8, defaultCanvasFontColor);
            }
        }

        private void DrawScaleNoteList(IGenericDrawingSurface g, int offsetX, int startY, ScaleItem scale, ColorValue fontColor)
        {
            string scaleNotes = "";
            int notePos = 0;
            try
            {
                for (int i = 0; i < scale.ScaleIntervals.Length; i++)
                {
                    if (scale.ScaleIntervals[i])
                    {
                        notePos++;

                        scaleNotes += NoteManager.GetNoteName(scale.GetNoteAtSequencePosition(notePos, NoteManager.GetNoteByName(_guitarModel.SelectedKey)), true);
                        scaleNotes += "  ";
                    }
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Exception: Couldn't resolve note name at one or more scale position");
            }

            g.DrawString(offsetX + 2, startY + 18, "Notes:", BasicFontSizePt, fontColor);
            g.DrawString(offsetX + 40, startY + 18, scaleNotes, BasicFontSizePt, fontColor);
        }

        private void DrawScaleFormulaIntervals(IGenericDrawingSurface g, int offsetX, int startY, ScaleItem scale, ColorValue fontColor)
        {
            string scaleIntervals = _guitarModel.GetScaleIntervals();
            g.DrawString(offsetX + 2, startY + 11, "Intervals:", BasicFontSizePt, fontColor);
            g.DrawString(offsetX + 40, startY + 11, scaleIntervals, BasicFontSizePt, fontColor);
        }

        private int DrawGuitarStrings(int numberOfStrings, IGenericDrawingSurface g, int offsetX, int startY)
        {
            for (int i = 0; i < numberOfStrings; i++)
            {
                DrawGuitarString(_guitarModel.GuitarStrings[i], startY, offsetX, g);
                startY = startY - _guitarModel.GuitarModelSettings.StringSpacing;
            }
            return startY;
        }

        private void DrawFrets(int numberOfStrings, IGenericDrawingSurface g, int startX, int startY)
        {
            //draw nut
            double fretLength = (_guitarModel.GuitarModelSettings.StringSpacing * (numberOfStrings));

            var nutX = startX - 2;
            g.DrawLine(nutX, startY + _guitarModel.GuitarModelSettings.StringSpacing, nutX, startY + fretLength, 2, ColorPalette[ThemeColorPreset.MutedForeground]);

            for (int i = 0; i < _guitarModel.GuitarModelSettings.NumberFrets + 1; i++)
            {
                int fretTopX = startX + GuitarString.FretNumberToClientX(i, _guitarModel, 0);
                int fretBottomX = startX + GuitarString.FretNumberToClientX(i, _guitarModel, _guitarModel.NumberOfStrings - 1);

                int nextFretX = startX + GuitarString.FretNumberToClientX(i + 1, _guitarModel, 0);

                g.DrawLine(fretTopX, startY + _guitarModel.GuitarModelSettings.StringSpacing, fretBottomX, startY + fretLength, 1, ColorPalette[ThemeColorPreset.Foreground]);

                //draw fret marker at specific points
                if (_guitarModel.FretsWithMarkers.Contains(i))
                {
                    double markerLeft = fretTopX - ((nextFretX - fretTopX) / 2) - 3;

                    if (_guitarModel.FretMarkerStyle == FretMarkerStyle.Dots)
                    {
                        if (i % 12 == 0) //fret 12 & 24 get double marker
                        {
                            g.FillEllipse(markerLeft, startY + (_guitarModel.GuitarModelSettings.StringSpacing * numberOfStrings) - _guitarModel.GuitarModelSettings.StringSpacing, 5, 5, ColorPalette[ThemeColorPreset.Subtle], ColorPalette[ThemeColorPreset.Subtle]);
                            g.FillEllipse(markerLeft, startY + (_guitarModel.GuitarModelSettings.StringSpacing * 2), 5, 5, ColorPalette[ThemeColorPreset.Subtle], ColorPalette[ThemeColorPreset.Subtle]);
                        }
                        else
                        {
                            g.FillEllipse(markerLeft, startY + (_guitarModel.GuitarModelSettings.StringSpacing * (numberOfStrings / 2)) + 2, 5, 5, ColorPalette[ThemeColorPreset.Subtle], ColorPalette[ThemeColorPreset.Subtle]);
                        }
                    }
                }
            }
        }

        private void DrawFretNumbers(int numberFrets, IGenericDrawingSurface g, int startX, int startY, ColorValue defaultCanvasFontColor)
        {
            for (int fretNum = 0; fretNum <= numberFrets; fretNum++)
            {
                int fretX = startX + GuitarString.FretNumberToClientX(fretNum, _guitarModel, 0);

                if (_guitarModel.GuitarModelSettings.EnableDisplacedFingeringMarkers)
                {
                    if (fretNum > 0)
                    {
                        var posL = fretX - GuitarString.FretNumberToClientX(fretNum - 1, _guitarModel, 0);
                        fretX -= posL / 2;
                    }
                    else
                    {
                        fretX -= 5; //displace 0 fret by 5
                    }
                }
                else
                {
                    //offset x by half of text width
                    fretX -= ((BasicFontSizePt / 3) * fretNum.ToString().Length);
                }

                g.DrawString(fretX, startY + 2, fretNum.ToString(), BasicFontSizePt, defaultCanvasFontColor);
            }
        }

        public void DrawGuitarString(GuitarString s, int startY, int offsetX, IGenericDrawingSurface g)
        {
            String strNote = "";
            int startX = offsetX;

            //draw string
            int fretboardWidth = s.GetFretboardWidth(_guitarModel, s.StringNumber);
            int fanScaleMM = (int)_guitarModel.MultiScaleFanFactor;

            if (_guitarModel.IsMultiScale)
            {
                startX += (s.StringNumber * fanScaleMM);
                fretboardWidth = fretboardWidth - (s.StringNumber * fanScaleMM * 2);
            }

            if (_guitarModel.GuitarModelSettings.EnableDiagramStrings)
            {
                var stringThickness = 0.3 + ((_guitarModel.NumberOfStrings - s.StringNumber) * 0.1);

                g.DrawLine(startX, startY - 0.5, startX + fretboardWidth, startY - 0.5, stringThickness, ColorPalette[ThemeColorPreset.Foreground]);
                g.DrawLine(startX, startY, startX + fretboardWidth, startY, stringThickness, ColorPalette[ThemeColorPreset.MutedForeground]);
            }

            for (int fretNum = 0; fretNum <= _guitarModel.GuitarModelSettings.NumberFrets; fretNum++)
            {
                int tmpVal = fretNum + (int)s.OpenTuning.SelectedNote;
                int octave = 1;
                if (tmpVal > 11)
                {
                    tmpVal = tmpVal - 12;
                    octave++;
                }
                if (tmpVal > 11)
                {
                    tmpVal = tmpVal - 12;
                    octave++;
                }

                int sclVal = (fretNum - (int)_guitarModel.GuitarModelSettings.ScaleManager.CurrentKey) + (int)s.OpenTuning.SelectedNote;
                if (sclVal < 0) sclVal = sclVal + 12;
                if (sclVal > 11) sclVal = sclVal - 12;
                if (sclVal > 11) sclVal = sclVal - 12;

                if (sclVal < 0) System.Diagnostics.Debug.WriteLine(sclVal);

                if (_guitarModel.SelectedScale.ScaleIntervals[sclVal] == true)
                {
                    if (fretNum <= _guitarModel.GuitarModelSettings.NumberFrets)
                    {
                        ColorValue strokeColor = ColorPalette[ThemeColorPreset.Foreground];
                        ColorValue fillColor = ColorPalette[ThemeColorPreset.Foreground];

                        if (fretNum == 0)
                        {
                            //fret zero has empty circle marker
                            fillColor = ColorPalette[ThemeColorPreset.Subtle];
                        }

                        if ((Note)tmpVal == _guitarModel.GetKey())
                        {
                            //root note has accent colour border
                            strokeColor = ColorPalette[ThemeColorPreset.Accent];
                        }

                        if (_guitarModel.GuitarModelSettings.EnableDisplacedFingeringMarkers)
                        {
                            //displace marker to place behind fret

                            if (fretNum > 0)
                            {
                                var posL = startX - GuitarString.FretNumberToClientX(fretNum - 1, _guitarModel, s.StringNumber);
                                startX -= posL / 2;
                                startX += (_guitarModel.GuitarModelSettings.MarkerSize / 2);
                            }
                            else
                            {
                                //fret 0, displace marker behind nut
                                startX = startX - (_guitarModel.GuitarModelSettings.MarkerSize / 2);
                            }
                        }

                        var currentNote = new NoteItem
                        {
                            Note = (Note)tmpVal,
                            X = startX - (_guitarModel.GuitarModelSettings.MarkerSize / 2),
                            Y = startY - (_guitarModel.GuitarModelSettings.MarkerSize / 2),
                            FretNumber = fretNum,
                            StringNumber = s.StringNumber,
                            Octave = octave
                        };

                        _noteList.Add(currentNote);


                        if (_hightlightedNotes.Any(n => n.Note == currentNote.Note && n.StringNumber == currentNote.StringNumber && n.Octave == currentNote.Octave))
                        {
                            // highlight

                            g.FillEllipse(startX - (_guitarModel.GuitarModelSettings.MarkerSize / 2), startY - (_guitarModel.GuitarModelSettings.MarkerSize / 2), _guitarModel.GuitarModelSettings.MarkerSize, _guitarModel.GuitarModelSettings.MarkerSize, ColorPalette[ThemeColorPreset.MutedBackground], new ColorValue(255, 255, 255, 255));

                        }

                        //draw note marker centered behind fret
                        if (_guitarModel.GuitarModelSettings.EnableNoteColours == true)
                        {
                            var noteColor = NoteManager.GetNoteColour((Note)tmpVal, 2);
                            if (_hightlightedNotes.Any())
                            {
                                noteColor.A = 128;
                            }
                            g.FillEllipse(startX - (_guitarModel.GuitarModelSettings.MarkerSize / 2), startY - (_guitarModel.GuitarModelSettings.MarkerSize / 2), _guitarModel.GuitarModelSettings.MarkerSize, _guitarModel.GuitarModelSettings.MarkerSize, ColorPalette[ThemeColorPreset.MutedBackground], noteColor);
                        }
                        else
                        {
                            g.FillEllipse(startX - (_guitarModel.GuitarModelSettings.MarkerSize / 2), startY - (_guitarModel.GuitarModelSettings.MarkerSize / 2), _guitarModel.GuitarModelSettings.MarkerSize, _guitarModel.GuitarModelSettings.MarkerSize, fillColor, strokeColor);
                        }

                        //if enabled, draw note name/sequence number
                        if (_guitarModel.GuitarModelSettings.NoteMarkerDisplayMode != NoteMarkerDisplayMode.NoLabel)
                        {
                            if (_guitarModel.GuitarModelSettings.NoteMarkerDisplayMode == NoteMarkerDisplayMode.NoteName)
                            {
                                strNote = NoteManager.GetNoteName((Note)tmpVal, _guitarModel.GuitarModelSettings.EnableDiagramNoteNamesSharp);
                            }
                            else if (_guitarModel.GuitarModelSettings.NoteMarkerDisplayMode == NoteMarkerDisplayMode.NoteSequence)
                            {
                                strNote = "" + _guitarModel.GuitarModelSettings.ScaleManager.CurrentScale.GetSequenceNumberInScale(sclVal);
                            }
                            else if (_guitarModel.GuitarModelSettings.NoteMarkerDisplayMode == NoteMarkerDisplayMode.ScaleInterval)
                            {
                                strNote = "" + _guitarModel.GuitarModelSettings.ScaleManager.CurrentScale.GetIntervalNameInScale(sclVal);
                            }

                            double markerFontSize = BasicFontSizePt;
                            double labelX = startX - (_guitarModel.GuitarModelSettings.MarkerSize * 0.45);
                            double labelY = startY - (markerFontSize * 0.3);
                            if (strNote.Length == 1)
                            {
                                labelX += markerFontSize * 0.3;
                                g.DrawString(labelX + 0.5, labelY + 0.5, strNote, markerFontSize, ColorPalette[ThemeColorPreset.TextShadow]); //shadow
                                g.DrawString(labelX, labelY, strNote, markerFontSize, ColorPalette[ThemeColorPreset.ForegroundText]);
                            }
                            else
                            {
                                labelX += markerFontSize * 0.2;
                                labelY += markerFontSize * 0.1;
                                g.DrawString(labelX + 0.5, labelY + 0.5, strNote, markerFontSize * .8, ColorPalette[ThemeColorPreset.TextShadow]);
                                g.DrawString(labelX, labelY, strNote, markerFontSize * .8, ColorPalette[ThemeColorPreset.ForegroundText]);
                            }
                        }
                    }
                }

                startX = offsetX + GuitarString.FretNumberToClientX(fretNum + 1, _guitarModel, s.StringNumber);
            }
        }
    }
}