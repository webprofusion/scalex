using FFImageLoading.Svg.Forms;
using Scalex.Views;
using System;
using System.Reflection;
using Xamarin.Forms;

namespace Scalex.Helpers
{
    public class ResourceLoader
    {
        private Assembly _assembly;

        public ResourceLoader()
        {
            _assembly = typeof(IntroPage).GetTypeInfo().Assembly;
        }

        public Tuple<string, Assembly> FetchResource(string resourceName)
        {
            foreach (var res in _assembly.GetManifestResourceNames())
            {
                if (res.EndsWith(resourceName))
                {
                    return new Tuple<string, Assembly>(res, _assembly);
                }
            }
            // resource not found
            return null;
        }

        public SvgImageSource GetSVGImageResource(string img)
        {
            var res = FetchResource(img);

            return SvgImageSource.FromResource(res.Item1, res.Item2);
        }

        public ImageSource GetImageResource(string img)
        {
            var res = FetchResource(img);
            return ImageSource.FromResource(res.Item1, res.Item2);
        }
    }
}