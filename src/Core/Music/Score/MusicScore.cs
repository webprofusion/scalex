using System;
using System.Collections.Generic;
//using System.Data;

namespace Webprofusion.Scalex.Music.Score
{
    public class MusicScore
    {

        public string Version = "";
        public string Title = "";
        public string Subtitle = "";
        public string Artist = "";
        public string Album = "";
        public string Author = "";
        public string Copyright = "";
        public string TranscribedBy = "";
        public string Instructions = "";
        public string Comments = "";

        /// <summary>
        /// List of Score Measures, each consisting of a list of Parts, each with a list of Notes
        /// </summary>
        public Dictionary<int, Measure> Measures;

        public Dictionary<String, Part> Parts;

        private bool _isLoadedCorrectly = false;
        /// <summary>
        /// if false after a score has been laoded an exception occurred during the file parsing
        /// </summary>
        /// 
        public bool IsLoadedCorrectly
        {
            get { return _isLoadedCorrectly; }
            set
            {
                _isLoadedCorrectly = value;
            }
        }

        /// <summary>
        /// If score is Timewise, score is arranged in list of Measures, Containing Parts
        /// </summary>
        public bool IsTimeWise = true;

        /// <summary>
        /// If score is Partwise, score is arranged in list of Parts, Containing Measures
        /// </summary>
        public bool IsPartWise = false;

        public MusicScore()
        {
            Parts = new Dictionary<string, Part>();
            Measures = new Dictionary<int, Measure>();
        }

        public string GetPartName(string partID)
        {
            if (Parts.ContainsKey(partID))
            {
                return Parts[partID].PartName;
            }
            else return "";
        }

        public int GetPartMeasureCount(string partID)
        {
            if (Parts.ContainsKey(partID))
            {
                return Parts[partID].Measures.Count;
            }
            else
            {
                return 0;
            }
        }

        public bool HasPart(string partID)
        {
            if (Parts.ContainsKey(partID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}