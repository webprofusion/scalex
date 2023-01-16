﻿
using AlphaTab.Importer;
using AlphaTab.Model;
using Scalex.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Scalex.Utils
{
    public class ScoreServiceManager
    {
        public bool EnableFileCaching { get; set; }

        private static string SongSearchURL = "https://www.songsterr.com/a/ra/songs.xml?pattern={keywords}";

        //private static string ArtistSearchURL = "http://www.songsterr.com/a/ra/songs/byartists.xml?artists={keywords}";
        private static string SongDetailsURL = "https://www.songsterr.com/a/ra/player/song/{songid}.xml";

        private static string SongsMostViewedURL = "https://www.songsterr.com/a/ra/songs/popular.xml";

        private string RootFolder = "";
        private Dictionary<string, byte[]> dataCache { get; set; }

        private ResourceRequestManager _resourceManager;


        public ScoreServiceManager()
        {
            EnableFileCaching = false;
            dataCache = new Dictionary<string, byte[]>();
            _resourceManager = new ResourceRequestManager(new HttpClient());
        }


        public async Task<Song> FetchSongDetailsAsync(int songID)
        {
            LogMessage("Fetching Song Details Async: " + songID);

            string url = SongDetailsURL.Replace("{songid}", songID.ToString());

            string songXML = await _resourceManager.GetStringWithCaching(url, true, songID.ToString() + ".xml");
            if (songXML != null)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Song));
                Song songDetails = (Song)xmlSerializer.Deserialize(new StringReader(songXML));

                return songDetails;
            }
            else return null;
        }

        public async Task<List<SongListItem>> FetchSongsMostViewed(int maxResults = 50)
        {
            LogMessage("Fetching Most Viewed Song Details Async.");

            string url = SongsMostViewedURL;
            string xmlresult = await _resourceManager.GetStringWithCaching(url, true, "mostviewed");

            if (xmlresult != null)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(MostViewedSongList));

                MostViewedSongList songList = (MostViewedSongList)xmlSerializer.Deserialize(new StringReader(xmlresult));
                return songList.Songs.Take(maxResults).ToList();
            }
            return null;
        }

        public async Task<List<SongListItem>> FetchFavouritesSongs()
        {
            LogMessage("Fetching Favourites Song Details Async.");

            string url = SongsMostViewedURL;
            string xmlresult = await _resourceManager.GetStringWithCaching(url, true, "favourites");

            if (xmlresult != null)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(MostViewedSongList));

                MostViewedSongList songList = (MostViewedSongList)xmlSerializer.Deserialize(new StringReader(xmlresult));
                return songList.Songs.ToList();
            }
            return null;
        }

        public async Task<List<SongListItem>> FetchSearchResults(string keywords, int maxResults)
        {
            LogMessage("Fetching Song Search Results Async.");

            try
            {
                string url = SongSearchURL.Replace("{keywords}", keywords);
                string xmlresult = await _resourceManager.GetStringWithCaching(url, false, "searchresults");

                if (xmlresult != null)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(SearchResultList));

                    SearchResultList songList = (SearchResultList)xmlSerializer.Deserialize(new StringReader(xmlresult));
                    if (songList.Songs != null)
                    {
                        return songList.Songs.Take(maxResults).ToList();
                    }
                    else
                    {
                        return new List<SongListItem>();
                    }
                }
            }
            catch (Exception exp)
            {
                //failed to get search results
                LogMessage("Failed to fetch search results: " + exp.ToString());
            }
            return null;
        }



        private void LogMessage(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        private async Task<bool> IsObjectCachedCheck(string key, string groupId)
        {
            bool isObjectCached = await IsObjectCached(key, groupId);
            return isObjectCached;
        }

        public List<byte[]> GetTrackImagePNG(Song song, int trackIndex, int trackWidth)
        {
            var trackLayout = song.LatestAvailableRevision.Tracks[trackIndex].TrackLayoutImages.FirstOrDefault(t => t.WidthLimit == trackWidth);
            if (trackLayout != null)
            {
                return trackLayout.ImageSlices;
            }
            else return null;
        }

        public Uri GetTrackAudioStreamUri(Song song, int trackIndex, int trackSpeed)
        {
            string audioUrl = song.LatestAvailableRevision.Tracks[trackIndex].TrackAudio.FirstOrDefault(t => t.Speed == trackSpeed).MP3File.AttachmentUrl;
            return new Uri(audioUrl);
        }

        public List<Uri> GetTrackAudioStreamUris(Song song, int trackSpeed)
        {
            List<Uri> uriList = new List<Uri>();

            for (int i = 0; i < song.LatestAvailableRevision.Tracks.Count; i++)
            {
                uriList.Add(new Uri(song.LatestAvailableRevision.Tracks[i].TrackAudio.FirstOrDefault(t => t.Speed == trackSpeed).MP3File.AttachmentUrl));
            }
            return uriList;
        }

        private async Task<bool> IsObjectCached(string key, string groupId)
        {
            /* if (EnableFileCaching)
             {
                 key = key.Replace("\\", "_");

                 StorageFile filetest = null;
                 try
                 {
                     var folder = await ResourceRequestManager.GetCacheFolder("cache_" + groupId);
                     if (folder != null)
                     {
                         filetest = await folder.GetFileAsync(key);
                     }
                 }
                 catch (Exception)
                 {
                     ; ;
                 }
                 finally
                 {
                 }

                 if (filetest == null)
                 {
                     LogMessage("File is not found/cached: " + key);
                     return false;
                 }

                 LogMessage("File exists:" + key);
                 return true;
             }
             else
             {
                 if (dataCache.ContainsKey(key)) return true;
             }
             */
            return false;
        }

        public async Task<Score> FetchSongScore(Song song, bool skipIfCached)
        {
            if (song.LatestAvailableRevision?.GuitarProTab?.BinaryData != null)
            {
                return ScoreLoader.LoadScoreFromBytes(song.LatestAvailableRevision.GuitarProTab.BinaryData);
            }

            //if not cached, download it again
            string datafileUrl = song.LatestAvailableRevision.GuitarProTab.AttachmentUrl;
            string fileName = RootFolder + "s" + song.ID + "." + datafileUrl.Substring(datafileUrl.LastIndexOf('.') + 1, 3);

            LogMessage("Fetching Track GP File: " + song.ID + " " + song.LatestAvailableRevision.GuitarProTab.AttachmentUrl);

            byte[] data = await _resourceManager.GetAttachmentWithCaching(song.LatestAvailableRevision.GuitarProTab.AttachmentUrl, true, song.ID.ToString());
            song.LatestAvailableRevision.GuitarProTab.BinaryData = data;

            //save file to local cache
            //await CacheBinaryData(fileName, song.ID.ToString(), data);
            if (data != null)
            {
                return ScoreLoader.LoadScoreFromBytes(data);
            }
            else
            {
                return null;
            }
        }

        public async Task<Score> LoadScoreFromUrl(string url)
        {
            byte[] data = await _resourceManager.GetAttachmentWithCaching(url, false, "notcached");
            return ScoreLoader.LoadScoreFromBytes(data, new AlphaTab.Settings { });
        }

    }
}