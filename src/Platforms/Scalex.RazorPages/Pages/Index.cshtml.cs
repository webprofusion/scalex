using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using Webprofusion.Scalex.Guitar;
using Webprofusion.Scalex.Music;

namespace Scalex.RazorPages.Pages;

public sealed class IndexModel : PageModel
{
    public IReadOnlyList<ScaleItem> Scales { get; private set; } = Array.Empty<ScaleItem>();
    public IReadOnlyList<string> Keys { get; private set; } = Array.Empty<string>();
    public IReadOnlyList<GuitarTuning> Tunings { get; private set; } = Array.Empty<GuitarTuning>();

    [BindProperty(SupportsGet = true)]
    public int? ScaleId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Key { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? TuningId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? Frets { get; set; }

    public string DiagramUrl { get; private set; } = string.Empty;
    public string DiagramName { get; private set; } = string.Empty;
    public string? RelativeScaleInfo { get; private set; }

    public void OnGet()
    {
        var model = new GuitarModel();
        Scales = model.AllScales;
        Keys = model.AllKeys;
        Tunings = model.AllTunings;

        ScaleId = ScaleId ?? model.SelectedScale?.ID ?? 2; // default to minor
        Key ??= model.SelectedKey;
        TuningId ??= model.SelectedTuning?.ID;
        Frets ??= model.GuitarModelSettings.NumberFrets;
        if (ScaleId.HasValue)
        {
            model.SetScale(ScaleId.Value);
        }

        if (!string.IsNullOrWhiteSpace(Key))
        {
            model.SetKey(Key);
        }

        if (TuningId.HasValue)
        {
            model.SetTuning(TuningId.Value);
        }

        if (Frets.HasValue)
        {
            model.SetNumberOfFrets(Frets.Value);
        }

        DiagramName = model.GetDiagramTitle();
        RelativeScaleInfo = ScaleUtilities.GetRelativeScaleInfo(model.SelectedScale, model.SelectedKey, model.GuitarModelSettings.EnableDiagramNoteNamesSharp);

        var encodedKey = Uri.EscapeDataString(Key ?? string.Empty);
        DiagramUrl = $"/scale-diagram?scaleId={ScaleId}&key={encodedKey}&tuningId={TuningId}&frets={Frets}";
    }
}
