using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scalex.Models
{
    #region Songsterr XML API Objects

    public class SearchResult
    {
        public int SongID { get; set; }
        public int ArtistID { get; set; }
        public string SongTitle { get; set; }
        public string ArtistName { get; set; }
    }

    public class Result
    {
        [XmlAttribute(AttributeName = "id")]
        public int ID { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    public class Artist : Result
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
    }

    public class Tab : Result
    {
        [XmlElement(ElementName = "rendererVersion")]
        public int RendererVersion { get; set; }
    }

    public class Attachment : Result
    {
        [XmlElement(ElementName = "attachmentUrl")]
        public string AttachmentUrl { get; set; }

        [XmlIgnore]
        public byte[] BinaryData { get; set; }
    }

    public class Instrument
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "value")]
        public int Value { get; set; }
    }

    public class TrackLayoutImage : Result
    {
        [XmlElement(ElementName = "sliced")]
        public bool Sliced { get; set; }

        [XmlElement(ElementName = "widthLimit")]
        public int WidthLimit { get; set; }

        [XmlElement(ElementName = "image")]
        public Attachment Image { get; set; }

        [XmlElement(ElementName = "timelineMap")]
        public Attachment TimelineMap { get; set; }

        [XmlIgnore]
        public List<byte[]> ImageSlices { get; set; }

        /// <summary>
        /// parsed timeline data from Timeline Map file 
        /// </summary>
        [XmlIgnore]
        public TimeLine TimeLine { get; set; }
    }

    public class TrackAudio : Result
    {
        [XmlElement(ElementName = "speed")]
        public int Speed { get; set; }

        [XmlElement(ElementName = "mp3File")]
        public Attachment MP3File { get; set; }
    }

    [XmlType("Track")]
    public class Track : Result
    {
        [XmlElement(ElementName = "capoHeight")]
        public int CapoHeight { get; set; }

        [XmlElement(ElementName = "instrument")]
        public Instrument Instrument { get; set; }

        [XmlElement(ElementName = "position")]
        public int Position { get; set; }

        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "tuning")]
        public string Tuning { get; set; }

        [XmlElement(ElementName = "withLyrics")]
        public bool WithLyrics { get; set; }

        [XmlArray("trackLayoutImages")]
        public List<TrackLayoutImage> TrackLayoutImages { get; set; }

        [XmlArray("trackAudios")]
        public List<TrackAudio> TrackAudio { get; set; }
    }

    public class SongRevision : Result
    {
        [XmlElement(ElementName = "tab")]
        public Tab Tab { get; set; }

        [XmlElement(ElementName = "guitarProTab")]
        public Attachment GuitarProTab { get; set; }

        [XmlArray("tracks")]
        public List<Track> Tracks { get; set; }

        [XmlElement(ElementName = "mostPopularTrack")]
        public Result MostPopularTrack { get; set; }
    }

    public class Song : Result
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "artist")]
        public Artist Artist { get; set; }

        [XmlElement(ElementName = "latestAvailableRevision")]
        public SongRevision LatestAvailableRevision { get; set; }
    }

    public class TimeLineElementExtended : TimeLineElement
    {
        public int BoundsHeight { get; set; }
        public int BoundsY { get; set; }
        public int SelectionBoundsHeight { get; set; }
        public int SelectionBoundsY { get; set; }
    }

    public class TimeLineElement
    {
        public int BoundsWidth;
        public int BoundsX;
        public int EndTime;
        public int Flag;
        public int MeasureNumber;
        public int SelectionBoundsWidth;
        public int SelectionBoundsX;
        public int StartTime;

        //additional property
        public int ElementIndex { get; set; }
    }

    public class TimeLine
    {
        public List<TimeLineElement> TimeLineElements { get; set; }
        public int StaffHeight { get; set; }
        public int StaffY;
    }

    public enum TabType
    {
        [XmlEnum(Name = "PLAYER")]
        PLAYER,

        [XmlEnum(Name = "TEXT_GUITAR_TAB")]
        TEXT_GUITAR_TAB,

        [XmlEnum(Name = "CHORDS")]
        CHORDS,

        [XmlEnum(Name = "TEXT_BASS_TAB")]
        TEXT_BASS_TAB
    }

    [XmlType("Song")]
    public class SongListItem : Result
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "artist")]
        public Artist Artist { get; set; }

        [XmlArray("TabTypes")]
        public List<TabType> TabTypes { get; set; }
    }

    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "NSArray", Namespace = "", IsNullable = false)]
    public class MostViewedSongList
    {
        [XmlElement("Song", Form = XmlSchemaForm.Unqualified)]
        public SongListItem[] Songs { get; set; }
    }

    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "NSArray", Namespace = "", IsNullable = false)]
    public class SearchResultList
    {
        [XmlElement("Song", Form = XmlSchemaForm.Unqualified)]
        public SongListItem[] Songs { get; set; }
    }

    #endregion Songsterr XML API Objects
}