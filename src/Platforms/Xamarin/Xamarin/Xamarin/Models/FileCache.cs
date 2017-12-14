using System;
using System.IO;
using System.Threading.Tasks;

#if WINDOWS_UWP
using Windows.Storage;

#endif

namespace Scalex.Models
{
    public class FileCache
    {
        public async void StoreCachedFileText(string filename, string value)
        {
#if __ANDROID___ || __IOS__
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filePath = System.IO.Path.Combine(documentsPath, filename);
                System.IO.File.WriteAllText(filePath, value.ToString());
#endif
#if WINDOWS_UWP
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await localFolder.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, value);
#endif
        }

        public async Task<string> LoadCachedFileText(string filename)
        {
#if __ANDROID__ || __IOS__

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = System.IO.Path.Combine(documentsPath, filename);
            if (System.IO.File.Exists(filePath))
            {
                var content = System.IO.File.ReadAllText(filePath);
                return content;
            }
            else
            {
                return null;
            }
#endif
#if WINDOWS_UWP

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = (StorageFile)await storageFolder.TryGetItemAsync(filename);
         if (sampleFile!=null)
            {
            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            return text;
            } else {
            return null;
            };

#endif
        }

        public async void StoreCachedFileBytes(string filename, byte[] value)
        {
#if __ANDROID___ || __IOS__
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filePath = System.IO.Path.Combine(documentsPath, filename);
                System.IO.File.WriteAllBytes(filePath, value);
#endif
#if WINDOWS_UWP
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await localFolder.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteBytesAsync(sampleFile, value);
#endif
        }

        public async Task<byte[]> LoadCachedFileBytes(string filename)
        {
#if __ANDROID__ || __IOS__

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = System.IO.Path.Combine(documentsPath, filename);
            if (System.IO.File.Exists(filename))
            {
                var content = System.IO.File.ReadAllBytes(filePath);
                return content;
            }
            else
            {
                return null;
            }
#endif

#if WINDOWS_UWP
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            System.Diagnostics.Debug.WriteLine("Cached:"+storageFolder.Path);
            StorageFile sampleFile = (StorageFile)await storageFolder.TryGetItemAsync(filename);
         if (sampleFile!=null)
            {
             byte[] result;
        using (Stream stream = await sampleFile.OpenStreamForReadAsync())
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                result = memoryStream.ToArray();
            }
        }
           return result;
            } else {
            return null;
            };

#endif
        }
    }
}