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

app.MapGet("/scale-diagram", (ScaleDiagramImageService service, int? scaleId, string? key, int? tuningId, int? frets, HttpResponse response) =>
{
    var request = new ScaleDiagramRequest(scaleId, key, tuningId, frets);
    var pngBytes = service.GetScaleDiagram(request);
    response.Headers.CacheControl = "public, max-age=3600, s-maxage=86400";
    return Results.File(pngBytes, "image/png");
});

app.Run();
