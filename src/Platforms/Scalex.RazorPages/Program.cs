using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Scalex.RazorPages.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ScaleDiagramImageService>();

var app = builder.Build();

app.UseStaticFiles();
app.MapRazorPages();

app.MapGet("/scale-diagram", (ScaleDiagramImageService service, int? scaleId, string? key, int? tuningId, int? frets, string? modeId, HttpResponse response) =>
{
    // Parse modeId manually to handle empty strings gracefully
    int? parsedModeId = int.TryParse(modeId, out var m) ? m : null;

    var request = new ScaleDiagramRequest(scaleId, key, tuningId, frets, parsedModeId);
    var pngBytes = service.GetScaleDiagram(request);
    response.Headers.CacheControl = "public, max-age=3600, s-maxage=86400";
    return Results.File(pngBytes, "image/png");
});

app.MapGet("/scale-play-data", (ScaleDiagramImageService service, int? scaleId, string? key, int? tuningId, int? frets, string? modeId) =>
{
    int? parsedModeId = int.TryParse(modeId, out var m) ? m : null;
    var request = new ScaleDiagramRequest(scaleId, key, tuningId, frets, parsedModeId);
    return Results.Json(service.GetPlaybackData(request));
});

app.MapGet("/scale-diagram-map", (ScaleDiagramImageService service, int? scaleId, string? key, int? tuningId, int? frets, string? modeId) =>
{
    int? parsedModeId = int.TryParse(modeId, out var m) ? m : null;
    var request = new ScaleDiagramRequest(scaleId, key, tuningId, frets, parsedModeId);
    return Results.Json(service.GetDiagramMap(request));
});

app.Run();
