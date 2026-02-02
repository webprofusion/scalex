using Microsoft.Extensions.Caching.Memory;
using Scalex.RazorPages.Rendering;
using SkiaSharp;
using System;
using Webprofusion.Scalex.Guitar;
using Webprofusion.Scalex.Rendering;

namespace Scalex.RazorPages.Services;

public sealed class ScaleDiagramImageService
{
    private readonly IMemoryCache _cache;

    public ScaleDiagramImageService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public byte[] GetScaleDiagram(ScaleDiagramRequest request)
    {
        var cacheKey = $"scale:{request.ScaleId}:{request.Key}:{request.TuningId}:{request.Frets}";

        return _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(30);
            return RenderDiagram(request);
        }) ?? Array.Empty<byte>();
    }

    private static byte[] RenderDiagram(ScaleDiagramRequest request)
    {
        const float scaleFactor = 4f;
        var guitarModel = new GuitarModel();

        guitarModel.GuitarModelSettings.EnableDiagramTitle = false;
        if (request.ScaleId.HasValue)
        {
            guitarModel.SetScale(request.ScaleId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Key))
        {
            guitarModel.SetKey(request.Key);
        }

        if (request.TuningId.HasValue)
        {
            guitarModel.SetTuning(request.TuningId.Value);
        }

        if (request.Frets.HasValue)
        {
            guitarModel.SetNumberOfFrets(request.Frets.Value);
        }

        var renderer = new ScaleDiagramRenderer(guitarModel);
        SkiaThemeHelper.ApplyDarkThemeColours(renderer);

        var diagramWidth = Math.Max(renderer.GetDiagramWidth(), 1);
        var diagramHeight = Math.Max(renderer.GetFretboardHeight() + renderer.PaddingTop + 60, 1);

        var scaledWidth = (int)Math.Ceiling((diagramWidth + 20) * scaleFactor);
        var scaledHeight = (int)Math.Ceiling((diagramHeight + 20) * scaleFactor);
        var info = new SKImageInfo(scaledWidth, scaledHeight, SKColorType.Rgba8888, SKAlphaType.Premul);
        using var surface = SKSurface.Create(info);
        var canvas = surface.Canvas;
        canvas.Clear(SKColors.Black);

        var drawingSurface = new SkiaDrawingSurface(canvas);
        drawingSurface.SetScale(scaleFactor);
        renderer.Render(drawingSurface);

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }
}
