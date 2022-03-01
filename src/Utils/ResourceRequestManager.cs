using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scalex.Utils
{
    internal class ResourceRequestManager
    {
        private HttpClient _httpClient;
        public ResourceRequestManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> GetAttachmentWithCaching(string attachmentUrl, bool enableCache, string cacheKey)
        {
            var fileCache = new FileCache();
            if (enableCache)
            {
                var cachedResult = await fileCache.LoadCachedFileBytes(cacheKey);
                if (cachedResult != null)
                {
                    return cachedResult;
                }
            }

            var uri = new Uri(attachmentUrl);

            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                if (enableCache)
                {
                    new FileCache().StoreCachedFileBytes(cacheKey, content);
                }
                return content;
            }
            else
            {
                return null;
            }

        }

        internal async Task<string> GetStringWithCaching(string url, bool enableCache, string cacheKey)
        {
            var fileCache = new FileCache();
            if (enableCache)
            {
                var cachedResult = await fileCache.LoadCachedFileText(cacheKey);
                if (cachedResult != null)
                {
                    return cachedResult;
                }
            }


            var uri = new Uri(url);

            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (enableCache)
                {
                    new FileCache().StoreCachedFileText(cacheKey, content);
                }
                return content;
            }
            else
            {
                return null;
            }
        }

    }
}