using System.Collections.Generic;
using Webprofusion.Scalex.Guitar;
using Webprofusion.Scalex.Music;
using Webprofusion.Scalex.Util;

namespace Webprofusion.Scalex.Rendering
{
    public class ChordDiagramRenderer : Generic2DRenderer
    {
        private const int GENERAL_PADDING = 10;
        private const int STRING_SPACING = 10;
        private const int FRET_SPACING = 10;
        private const int FRET_MARKERSIZE = 8;
        private const int NUMBER_OF_FRETS = 5;
        private const int FONT_SIZE = 12;
        public int ChordsPerRow { get; set; } = 4;

        public List<ChordDiagram> CurrentChordDiagrams = null;
        public GuitarModel GuitarModel = null;
        public ColorValue DrawingColor { get; set; }
        public ColorValue BackgroundColor { get; set; }

        public ChordDiagramRenderer()
        {
            DrawingColor = ColorPalette[ThemeColorPreset.Foreground];
            BackgroundColor = ColorPalette[ThemeColorPreset.Background];
            GuitarModel = new GuitarModel();
            CurrentChordDiagrams = GuitarModel.GetPopularChordDiagrams();
        }

        public int GetRequiredHeight(IGenericDrawingSurface canvas)
        {
            return 1024 * 3;
        }

        public int GetRequiredWidthPerChord()
        {
            int fretBoardWidth = (STRING_SPACING * (GuitarModel.NumberOfStrings - 1));
            return fretBoardWidth + GENERAL_PADDING;
        }

        public override void Render(IGenericDrawingSurface canvas)
        {
            if (EnableRendering == false) return;
            if (CurrentChordDiagrams == null) return;

            IGenericDrawingSurface g = InitialiseDrawingSurface(canvas, 800, 1024);

            int startX = GENERAL_PADDING;
            int startY = GENERAL_PADDING;

            int stringSpacing = STRING_SPACING;
            int fretSpacing = FRET_SPACING;
            int fretMarkerSize = FRET_MARKERSIZE;
            int numFrets = NUMBER_OF_FRETS;
            int fontSize = FONT_SIZE;
            int fretBoardWidth = (stringSpacing * (GuitarModel.NumberOfStrings - 1));
            int minFretboardBoxSize = (stringSpacing * 5); //allow min fretboard spacing of 6 strings width
            int chordsPerRow = ChordsPerRow;
            int rowY = startY;
            int titleSpacing = fontSize * 2;

            int rowChordCount = 0;
            int xPos, yPos;

            foreach (ChordDiagram CurrentChordDiagram in CurrentChordDiagrams)
            {
                if (rowChordCount >= chordsPerRow)
                {
                    //start new row
                    rowY += (titleSpacing * 2) + (numFrets * fretSpacing);

                    xPos = startX;
                    yPos = rowY;

                    rowChordCount = 0;
                }
                else
                {
                    //start next chord on same row
                    if (rowChordCount > 0)
                    {
                        //next chord position in row is based on fretboardwidth +padding
                        xPos = startX +
                            (rowChordCount *
                                (fretBoardWidth < minFretboardBoxSize ? minFretboardBoxSize : fretBoardWidth) + (rowChordCount * stringSpacing)
                                );
                        //+ (rowChordCount * stringSpacing) + stringSpacing;
                    }
                    else
                    {
                        xPos = startX;
                    }

                    yPos = rowY;
                }

                //draw chord name
                g.DrawString(xPos, yPos, CurrentChordDiagram.ChordName, fontSize, DrawingColor);
                yPos += fontSize * 2;

                //draw nut (if chord starts at fret 0)
                g.DrawLine(xPos, yPos, xPos + fretBoardWidth, yPos, 2D, DrawingColor);

                //draw strings
                for (int i = 0; i < GuitarModel.NumberOfStrings; i++)
                {
                    double currentX = xPos + (i * stringSpacing);
                    g.DrawLine(currentX, yPos, currentX, yPos + (numFrets * fretSpacing), 1, DrawingColor);
                }

                //draw frets
                for (int i = 0; i <= numFrets; i++)
                {
                    double currentY = yPos + (i * fretSpacing);
                    g.DrawLine(xPos, currentY, xPos + fretBoardWidth, currentY, 1, DrawingColor);
                }

                //draw fretted notes per string
                for (int i = 0; i < GuitarModel.NumberOfStrings; i++)
                {
                    double currentX = xPos + (i * stringSpacing) - (fretMarkerSize / 2); //position marker centred on string

                    if (CurrentChordDiagram.FrettedStrings[i] != null)
                    {
                        int fret = CurrentChordDiagram.FrettedStrings[i].Fret;
                        double currentY = yPos + (fret * fretSpacing) - (fretSpacing / 2) - (fretMarkerSize / 2); //position marker centered between frets or behind nut

                        if (fret == 0)
                        {
                            g.FillEllipse(currentX, currentY, fretMarkerSize, fretMarkerSize, ColorPalette[ThemeColorPreset.MutedForeground], DrawingColor);
                        }
                        else
                        {
                            g.FillEllipse(currentX, currentY, fretMarkerSize, fretMarkerSize, DrawingColor, DrawingColor);
                        }
                    }
                    else
                    {
                        //string not played
                        g.DrawString(currentX, yPos - (fretSpacing / 2) - (fretMarkerSize / 2), "x", DrawingColor);
                    }
                }

                rowChordCount++; //move to next chord in list
            }
        }
    }
}