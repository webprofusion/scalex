﻿using ModernHttpClient;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Webprofusion.Scalex.Lessons;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scalex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LessonPage : ContentPage
    {
        private Func<string, Task<string>> _evaluateJavascript;

        public Func<string, Task<string>> EvaluateJavascript
        {
            get { return _evaluateJavascript; }
            set { _evaluateJavascript = value; }
        }

        private Lesson _lesson = null;


        private string _serviceBaseUri = "https://api.soundshed.com";

        private HttpClient GetDefaultHttpClient()
        {
            return new HttpClient(new NativeMessageHandler());
        }

        public LessonPage()
        {
            InitializeComponent();
            MediaPlayerWebView.BindingContext = this;

            MediaPlayerWebView.Source = _serviceBaseUri + "/assets/mediaview.html";
        }

        /// <summary>
        /// Attempts to find and return the given resource from within the specified assembly. 
        /// </summary>
        /// <returns> The embedded resource stream. </returns>
        /// <param name="assembly"> Assembly. </param>
        /// <param name="resourceFileName"> Resource file name. </param>
		public static Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
        {
            var resourceNames = assembly.GetManifestResourceNames();

            var resourcePaths = resourceNames
                .Where(x => x.EndsWith(resourceFileName, StringComparison.CurrentCultureIgnoreCase))
                .ToArray();

            if (!resourcePaths.Any())
            {
                throw new Exception(string.Format("Resource ending with {0} not found.", resourceFileName));
            }

            if (resourcePaths.Count() > 1)
            {
                throw new Exception(string.Format("Multiple resources ending with {0} found: {1}{2}", resourceFileName, Environment.NewLine, string.Join(Environment.NewLine, resourcePaths)));
            }

            return assembly.GetManifestResourceStream(resourcePaths.Single());
        }

        /// <summary>
        /// Attempts to find and return the given resource from within the specified assembly. 
        /// </summary>
        /// <returns> The embedded resource as a byte array. </returns>
        /// <param name="assembly"> Assembly. </param>
        /// <param name="resourceFileName"> Resource file name. </param>
        public static byte[] GetEmbeddedResourceBytes(Assembly assembly, string resourceFileName)
        {
            var stream = GetEmbeddedResourceStream(assembly, resourceFileName);

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Attempts to find and return the given resource from within the specified assembly. 
        /// </summary>
        /// <returns> The embedded resource as a string. </returns>
        /// <param name="assembly"> Assembly. </param>
        /// <param name="resourceFileName"> Resource file name. </param>
		public static string GetEmbeddedResourceString(Assembly assembly, string resourceFileName)
        {
            var stream = GetEmbeddedResourceStream(assembly, resourceFileName);

            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }

        private string LoadStringResource(string name)
        {
            var assembly = typeof(LessonPage).GetTypeInfo().Assembly;
            return GetEmbeddedResourceString(assembly, name);
        }

        private async Task<string> FetchData(string url)
        {
            using (var httpClient = GetDefaultHttpClient())
            {
                var uri = new Uri(url);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    return content;
                }
                else
                {
                    return null;
                }
            }
        }

        public void SetPageTitle()
        {
            if (this._lesson != null)
            {
                this.Title = _lesson.Title;
            }
        }

        public async Task LoadLesson(string id)
        {
            var lessonData = await FetchData($"{_serviceBaseUri}/lesson/{id}");

            _lesson = Newtonsoft.Json.JsonConvert.DeserializeObject<Lesson>(lessonData);

            SetPageTitle();
            Device.BeginInvokeOnMainThread(async () =>
            {
                await this.BeginLessonSection(1);
            });

            // var htmlSource = new HtmlWebViewSource();

            //string html = LoadStringResource("mediaview.html").ToString();
            // htmlSource.Html = html;

            //MediaPlayerWebView.Source = htmlSource;
        }

        public async Task BeginLessonSection(int lessonIndex)
        {
            var retryCount = 0;
            while (!await IsMediaPlayerReady() && retryCount < 20)
            {
                await Task.Delay(500);
                retryCount++;
            }
            //start lesson
            var section = _lesson.Sections[lessonIndex];

            if (section.MediaItemIds != null)
            {
                //get video (if any)
                string videoId = "";
                foreach (var m in _lesson.MediaItems)
                {
                    if (m.SourceType == "YouTube" && section.MediaItemIds.Contains(m.Id))
                    {
                        videoId = m.SourceValue;
                        break;
                    }
                }

                if (!String.IsNullOrEmpty(videoId))
                {
                    await LoadYoutubeVideo(videoId);
                }

                //show tab (if any)
                string tabUrl = null;
                int itemIndex = 0;
                foreach (var m in _lesson.MediaItems)
                {
                    if (m.SourceType == "GuitarPro" && section.MediaItemIds.Contains(m.Id))
                    {
                        tabUrl = m.SourceValue;
                        itemIndex = m.ItemIndex > 0 ? (int)m.ItemIndex : 0;
                        break;
                    }
                }

                if (!String.IsNullOrEmpty(tabUrl))
                {
                    tabUrl = _serviceBaseUri + "/assets/tablature/" + tabUrl;

                //    await LoadTablature(tabUrl, itemIndex);
                }
            }

            //populate intro text
            SectionDescription.Text = section.Content;
        }

        private async Task<bool> IsMediaPlayerReady()
        {
            try
            {
                var js = "checkPlayerReady().toString();";

                var result = await EvaluateJavascript(js);
                if (result == "true")
                {
                    return true;
                }
                System.Diagnostics.Debug.WriteLine(result);
            }
            catch (Exception) { }
            return false;
        }

        private async Task LoadYoutubeVideo(string id)
        {
            try
            {
                var js = "loadVideoById('" + id + "',0);";

                var result = await EvaluateJavascript(js);
                System.Diagnostics.Debug.WriteLine(result);
            }
            catch (Exception) { }
        }


        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await LoadLesson("lesson-goulding-lickofdoom");
        }
    }
}