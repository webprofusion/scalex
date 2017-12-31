using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scalex
{
    public class TrackArtwork
    {
        public string URL { get; set; }
        public string Size { get; set; }
        public object Bitmap { get; set; }
    }

    public class TrackArtist
    {
        public string Title { get; set; }
        public string MusicBrainzID { get; set; }
    }

    public class TrackAlbum
    {
        public string Title { get; set; }
        public string MusicBrainzID { get; set; }
    }

    public class TrackMetadata
    {
        public string MusicBrainzID { get; set; }
        public string Title { get; set; }
        public List<TrackArtwork> TrackArtwork { get; set; }
        public TrackArtist Artist { get; set; }
        public TrackAlbum Album { get; set; }
    }

    public class TrackMetadataManager
    {
        public static async Task<TrackMetadata> GetTrackMetadata(string artist, string track)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=track.getInfo&api_key=510d5833237825522124952ecb00465c&format=json&artist=" + artist + "&track=" + track;

            string result = null;

            try
            {
                result = await ResourceRequestManager.GetStringWithCaching(url, true, "metadata");
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.WriteLine("Failed to fetch resource [GetTrackMetadata]" + exp.ToString());
            }

            if (!String.IsNullOrEmpty(result))
            {
                try
                {
                    JObject o = JObject.Parse(result);
                    if (o["error"] == null && o["track"]["album"] != null)
                    {
                        TrackMetadata metadata = new TrackMetadata();
                        if (o["track"]["mbid"] != null)
                        {
                            metadata.MusicBrainzID = o["track"]["mbid"].ToString();
                        }
                        metadata.Title = o["track"]["name"].ToString();

                        if (o["track"]["artist"] != null)
                        {
                            metadata.Artist = new TrackArtist();
                            metadata.Artist.Title = o["track"]["artist"]["name"].ToString();
                            metadata.Artist.MusicBrainzID = o["track"]["artist"]["mbid"].ToString();
                        }

                        metadata.Album = new TrackAlbum();
                        metadata.Album.Title = o["track"]["album"]["title"].ToString();
                        metadata.Album.MusicBrainzID = o["track"]["album"]["mbid"].ToString();

                        if (o["track"]["album"]["image"] != null)
                        {
                            //string imgUrl = null;
                            var imageArray = o["track"]["album"]["image"];
                            foreach (var img in imageArray)
                            {
                                TrackArtwork artwork = new TrackArtwork();
                                artwork.Size = img["size"].ToString();
                                artwork.URL = img["#text"].ToString();

                                if (metadata.TrackArtwork == null) metadata.TrackArtwork = new List<TrackArtwork>();
                                metadata.TrackArtwork.Add(artwork);
                            }
                        }
                        return metadata;
                    }
                }
                catch (Exception exp)
                {
                    System.Diagnostics.Debug.WriteLine("Error parsing metadata:" + result + " ::" + exp.ToString());
                }
            }
            return null;
        }
    }
}