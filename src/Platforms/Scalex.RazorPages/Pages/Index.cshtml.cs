using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using Webprofusion.Scalex.Guitar;
using Webprofusion.Scalex.Music;
using Webprofusion.Scalex.Util;

namespace Scalex.RazorPages.Pages;

public sealed class IndexModel : PageModel
{
    public IReadOnlyList<ScaleItem> Scales { get; private set; } = Array.Empty<ScaleItem>();
    public IReadOnlyList<string> Keys { get; private set; } = Array.Empty<string>();
    public IReadOnlyList<GuitarTuning> Tunings { get; private set; } = Array.Empty<GuitarTuning>();
    public IReadOnlyList<ModeItem> Modes { get; private set; } = Array.Empty<ModeItem>();

    [BindProperty(SupportsGet = true)]
    public int? ScaleId { get; set; }

    [BindProperty(SupportsGet = true, Name = "scale")]
    public string? ScaleSlug { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Key { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? TuningId { get; set; }

    [BindProperty(SupportsGet = true, Name = "tuning")]
    public string? TuningSlug { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? Frets { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? ModeId { get; set; }

    [BindProperty(SupportsGet = true, Name = "mode")]
    public string? ModeSlug { get; set; }

    public string DiagramUrl { get; private set; } = string.Empty;
    public string DiagramName { get; private set; } = string.Empty;
    public string? RelativeScaleInfo { get; private set; }
    public string? ModeInfo { get; private set; }
    public bool IsUsingDefaults { get; private set; }

    public void OnGet()
    {
        // Check if using defaults (no route parameters provided)
        IsUsingDefaults = string.IsNullOrWhiteSpace(Key) && string.IsNullOrWhiteSpace(ScaleSlug) && string.IsNullOrWhiteSpace(TuningSlug) && !Frets.HasValue && string.IsNullOrWhiteSpace(ModeSlug);

        var model = new GuitarModel();
        Scales = model.AllScales;
        Keys = model.AllKeys;
        Tunings = model.AllTunings;

        if (!string.IsNullOrWhiteSpace(Key))
        {
            var keyMatch = model.AllKeys.FirstOrDefault(k =>
                string.Equals(SlugUtility.CreateSlug(k), SlugUtility.CreateSlug(Key), StringComparison.OrdinalIgnoreCase));
            Key = keyMatch ?? Key;
        }

        if (!string.IsNullOrWhiteSpace(ScaleSlug))
        {
            var scaleMatch = model.AllScales.FirstOrDefault(scale =>
                string.Equals(SlugUtility.CreateSlug(scale.Name), SlugUtility.CreateSlug(ScaleSlug), StringComparison.OrdinalIgnoreCase));
            ScaleId = scaleMatch?.ID ?? ScaleId;
        }

        if (!string.IsNullOrWhiteSpace(TuningSlug))
        {
            var tuningMatch = model.AllTunings.FirstOrDefault(tuning =>
                string.Equals(SlugUtility.CreateSlug(tuning.Name), SlugUtility.CreateSlug(TuningSlug), StringComparison.OrdinalIgnoreCase));
            TuningId = tuningMatch?.ID ?? TuningId;
        }

        // Match mode by slug if provided
        if (!string.IsNullOrWhiteSpace(ModeSlug))
        {
            var modeMatch = model.AllModes.FirstOrDefault(mode =>
                string.Equals(SlugUtility.CreateSlug(mode.Name), SlugUtility.CreateSlug(ModeSlug), StringComparison.OrdinalIgnoreCase));
            ModeId = modeMatch?.ID ?? ModeId;
        }

        // Default to E Minor scale when no parameters provided
        ScaleId ??= 2; // Minor scale
        Key ??= "E";
        TuningId ??= model.SelectedTuning?.ID;
        Frets ??= model.GuitarModelSettings.NumberFrets;

        // Get modes applicable to the current scale
        Modes = model.ModeManager.GetModesForScale(ScaleId ?? 0);

        // If scale has modes and no mode is specified, default to the first mode
        if (!ModeId.HasValue && Modes.Any())
        {
            ModeId = Modes.First().ID;
        }

        // Apply mode if selected (overrides scale)
        if (ModeId.HasValue)
        {
            // Verify the mode is valid for the current scale
            var validMode = Modes.FirstOrDefault(m => m.ID == ModeId.Value);
            if (validMode != null)
            {
                model.SetMode(ModeId.Value);
                var selectedMode = model.SelectedMode;
                if (selectedMode != null)
                {
                    var parentScale = model.AllScales.FirstOrDefault(s => s.ID == selectedMode.ParentScaleId);
                    ModeInfo = ModeUtilities.GetModeRelationshipInfo(selectedMode, parentScale?.Name ?? "", Key ?? "");
                }
            }
            else
            {
                // Mode not valid for this scale, clear it and default to first mode if available
                ModeId = Modes.Any() ? Modes.First().ID : null;
                if (ModeId.HasValue)
                {
                    model.SetMode(ModeId.Value);
                }
                else
                {
                    model.SetScale(ScaleId!.Value);
                }
            }
        }
        else if (ScaleId.HasValue)
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
        DiagramUrl = $"/scale-diagram?scaleId={ScaleId}&key={encodedKey}&tuningId={TuningId}&frets={Frets}&modeId={ModeId}";
    }
}
