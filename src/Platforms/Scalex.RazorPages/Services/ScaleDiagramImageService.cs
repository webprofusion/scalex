using Microsoft.Extensions.Caching.Memory;
using Scalex.RazorPages.Rendering;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Webprofusion.Scalex.Guitar;
using Webprofusion.Scalex.Music;
using Webprofusion.Scalex.Rendering;

namespace Scalex.RazorPages.Services;

public sealed class ScaleDiagramImageService
{
    private readonly IMemoryCache _cache;
    private const float RenderScaleFactor = 4f;

    public ScaleDiagramImageService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public byte[] GetScaleDiagram(ScaleDiagramRequest request)
    {
        var cacheKey = $"scale:{request.ScaleId}:{request.Key}:{request.TuningId}:{request.Frets}:{request.ModeId}";

        return _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(30);
            return RenderDiagram(request);
        }) ?? Array.Empty<byte>();
    }

    public PlaybackDataDto GetPlaybackData(ScaleDiagramRequest request)
    {
        var cacheKey = $"play:{request.ScaleId}:{request.Key}:{request.TuningId}:{request.Frets}:{request.ModeId}";

        return _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(30);

            var guitarModel = BuildModel(request);
            var diagramMap = GetDiagramMap(request);
            var markerPattern = BuildPlayablePattern(diagramMap.Markers, request.Frets ?? guitarModel.GuitarModelSettings.NumberFrets);

            if (markerPattern.Count > 0)
            {
                var rootName = NoteManager.GetNoteName(guitarModel.GetKey(), true);

                // Ensure scale playback always starts on the selected key root
                var firstRootIndex = markerPattern.FindIndex(m => string.Equals(m.NoteName, rootName, StringComparison.OrdinalIgnoreCase));
                if (firstRootIndex > 0)
                {
                    markerPattern = markerPattern.Skip(firstRootIndex).Concat(markerPattern.Take(firstRootIndex)).ToList();
                }

                var patternAscending = markerPattern
                    .Select(m => new PlaybackNoteDto($"{m.NoteName}{m.Octave}", m.Frequency, 320, $"{m.StringNumber}-{m.FretNumber}"))
                    .ToList();

                var patternDescending = markerPattern
                    .AsEnumerable()
                    .Reverse()
                    .Select(m => new PlaybackNoteDto($"{m.NoteName}{m.Octave}", m.Frequency, 320, $"{m.StringNumber}-{m.FretNumber}"))
                    .ToList();

                var melodyRandom = new Random(cacheKey.GetHashCode());
                var melodyRhythmPattern = new[] { 250, 250, 500, 250, 250, 500, 500, 500 };
                var generatedMelodyLength = 16;
                var generatedMelody = new List<PlaybackNoteDto>(generatedMelodyLength);

                var melodyIndex = Math.Min(markerPattern.Count - 1, Math.Max(0, markerPattern.Count / 2));

                for (var i = 0; i < generatedMelodyLength; i++)
                {
                    int duration;

                    if ((i + 1) % 8 == 0)
                    {
                        melodyIndex = 0;
                        duration = 700;
                    }
                    else
                    {
                        var motionRoll = melodyRandom.NextDouble();
                        var step = 0;

                        if (motionRoll < 0.60)
                        {
                            step = melodyRandom.Next(0, 2) == 0 ? -1 : 1;
                        }
                        else if (motionRoll < 0.85)
                        {
                            step = 0;
                        }
                        else
                        {
                            step = melodyRandom.Next(0, 2) == 0 ? -2 : 2;
                        }

                        melodyIndex = Math.Clamp(melodyIndex + step, 0, markerPattern.Count - 1);
                        duration = melodyRhythmPattern[i % melodyRhythmPattern.Length];
                    }

                    var marker = markerPattern[melodyIndex];
                    generatedMelody.Add(new PlaybackNoteDto(
                        $"{marker.NoteName}{marker.Octave}",
                        marker.Frequency,
                        duration,
                        $"{marker.StringNumber}-{marker.FretNumber}"));
                }

                return new PlaybackDataDto(guitarModel.GetDiagramTitle(), patternAscending, patternDescending, generatedMelody);
            }

            var keyNote = guitarModel.GetKey();
            var scaleIntervals = guitarModel.SelectedScale.ScaleIntervals;

            var orderedScaleNotes = new List<Note>();
            for (var i = 0; i < scaleIntervals.Length; i++)
            {
                if (!scaleIntervals[i])
                {
                    continue;
                }

                var noteIndex = (i + (int)keyNote) % 12;
                orderedScaleNotes.Add((Note)noteIndex);
            }

            if (orderedScaleNotes.Count == 0)
            {
                orderedScaleNotes.Add(keyNote);
            }

            var baseOctave = 4;
            var ascendingInstances = new List<NoteInstance>();
            var currentOctave = baseOctave;
            int? previousPitchClass = null;

            foreach (var note in orderedScaleNotes)
            {
                var pitchClass = GetPitchClassFromC(note);

                if (previousPitchClass.HasValue && pitchClass < previousPitchClass.Value)
                {
                    currentOctave++;
                }

                ascendingInstances.Add(new NoteInstance(note, currentOctave));
                previousPitchClass = pitchClass;
            }

            var rootPitchClass = GetPitchClassFromC(keyNote);
            var lastPitchClass = previousPitchClass ?? rootPitchClass;
            var topRootOctave = currentOctave + (rootPitchClass <= lastPitchClass ? 1 : 0);
            var topRoot = new NoteInstance(keyNote, topRootOctave);

            var ascending = ascendingInstances
                .Select(n => new PlaybackNoteDto($"{NoteManager.GetNoteName(n.SelectedNote, true)}{n.Octave}", n.NoteFrequency, 320, null))
                .Append(new PlaybackNoteDto($"{NoteManager.GetNoteName(topRoot.SelectedNote, true)}{topRoot.Octave}", topRoot.NoteFrequency, 320, null))
                .ToList();

            var descending = new[] { topRoot }
                .Concat(ascendingInstances.AsEnumerable().Reverse())
                .Select(n => new PlaybackNoteDto($"{NoteManager.GetNoteName(n.SelectedNote, true)}{n.Octave}", n.NoteFrequency, 320, null))
                .ToList();

            var random = new Random(cacheKey.GetHashCode());

            // Simple musical phrase generator:
            // - 4/4 style rhythm motifs with varied durations
            // - mostly stepwise melodic motion
            // - occasional leaps
            // - phrase endings resolve towards root
            var rhythmPattern = new[] { 250, 250, 500, 250, 250, 500, 500, 500 }; // 8 eighth-note slots with quarters
            var melodyLength = 16;
            var melody = new List<PlaybackNoteDto>(melodyLength);

            var currentIndex = Math.Min(ascendingInstances.Count - 1, Math.Max(0, ascendingInstances.Count / 2));

            for (var i = 0; i < melodyLength; i++)
            {
                int duration;

                // Resolve at phrase boundaries (every 8 notes)
                if ((i + 1) % 8 == 0)
                {
                    currentIndex = 0;
                    duration = 700; // longer cadence note
                }
                else
                {
                    var motionRoll = random.NextDouble();
                    var step = 0;

                    if (motionRoll < 0.60)
                    {
                        // Stepwise motion
                        step = random.Next(0, 2) == 0 ? -1 : 1;
                    }
                    else if (motionRoll < 0.85)
                    {
                        // Repeated tone
                        step = 0;
                    }
                    else
                    {
                        // Occasional leap
                        step = random.Next(0, 2) == 0 ? -2 : 2;
                    }

                    currentIndex = Math.Clamp(currentIndex + step, 0, ascendingInstances.Count - 1);
                    duration = rhythmPattern[i % rhythmPattern.Length];
                }

                var note = ascendingInstances[currentIndex];
                melody.Add(new PlaybackNoteDto(
                    $"{NoteManager.GetNoteName(note.SelectedNote, true)}{note.Octave}",
                    note.NoteFrequency,
                    duration,
                    null));
            }

            return new PlaybackDataDto(guitarModel.GetDiagramTitle(), ascending, descending, melody);
        }) ?? new PlaybackDataDto(string.Empty, Array.Empty<PlaybackNoteDto>(), Array.Empty<PlaybackNoteDto>(), Array.Empty<PlaybackNoteDto>());
    }

    public DiagramMapDto GetDiagramMap(ScaleDiagramRequest request)
    {
        var cacheKey = $"map:{request.ScaleId}:{request.Key}:{request.TuningId}:{request.Frets}:{request.ModeId}";

        return _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(30);

            var guitarModel = BuildModel(request);
            var renderer = new ScaleDiagramRenderer(guitarModel)
            {
                IsExportMode = true,
                PaddingTop = 0
            };

            SkiaThemeHelper.ApplyDarkThemeColours(renderer);

            var diagramWidth = Math.Max(renderer.GetDiagramWidth(), 1);
            var diagramHeight = Math.Max(renderer.GetFretboardHeight() + renderer.PaddingTop, 1);
            var scaledWidth = (int)Math.Ceiling((diagramWidth + 20) * RenderScaleFactor);
            var scaledHeight = (int)Math.Ceiling((diagramHeight + 20) * RenderScaleFactor);

            var info = new SKImageInfo(scaledWidth, scaledHeight, SKColorType.Rgba8888, SKAlphaType.Premul);
            using var surface = SKSurface.Create(info);
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.Black);

            var drawingSurface = new SkiaDrawingSurface(canvas);
            drawingSurface.SetScale(RenderScaleFactor);
            renderer.Render(drawingSurface);

            var markerSize = renderer.MarkerSize;

            var markers = renderer.RenderedNotes.Select(n =>
            {
                var centerX = (n.X + (markerSize / 2.0)) * RenderScaleFactor;
                var centerY = (n.Y + (markerSize / 2.0)) * RenderScaleFactor;
                var openString = guitarModel.GuitarStrings[n.StringNumber].OpenTuning;
                var noteAtFret = GetNoteAtFret(openString, n.FretNumber);

                return new DiagramMarkerDto(
                    centerX / scaledWidth,
                    centerY / scaledHeight,
                    (markerSize * 0.5 * RenderScaleFactor) / scaledWidth,
                    NoteManager.GetNoteName(noteAtFret.SelectedNote, true),
                    noteAtFret.NoteFrequency,
                    n.StringNumber,
                    n.FretNumber,
                    noteAtFret.Octave);
            }).ToList();

            return new DiagramMapDto(scaledWidth, scaledHeight, markers);
        }) ?? new DiagramMapDto(1, 1, Array.Empty<DiagramMarkerDto>());
    }

    private static byte[] RenderDiagram(ScaleDiagramRequest request)
    {
        var guitarModel = BuildModel(request);

        var renderer = new ScaleDiagramRenderer(guitarModel);
        renderer.IsExportMode = true;
        renderer.PaddingTop = 0;

        SkiaThemeHelper.ApplyDarkThemeColours(renderer);

        var diagramWidth = Math.Max(renderer.GetDiagramWidth(), 1);
        var diagramHeight = Math.Max(renderer.GetFretboardHeight() + renderer.PaddingTop, 1);

        var scaledWidth = (int)Math.Ceiling((diagramWidth + 20) * RenderScaleFactor);
        var scaledHeight = (int)Math.Ceiling((diagramHeight + 20) * RenderScaleFactor);
        var info = new SKImageInfo(scaledWidth, scaledHeight, SKColorType.Rgba8888, SKAlphaType.Premul);
        using var surface = SKSurface.Create(info);
        var canvas = surface.Canvas;
        canvas.Clear(SKColors.Black);

        var drawingSurface = new SkiaDrawingSurface(canvas);
        drawingSurface.SetScale(RenderScaleFactor);
        renderer.Render(drawingSurface);

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    private static GuitarModel BuildModel(ScaleDiagramRequest request)
    {
        var guitarModel = new GuitarModel();
        guitarModel.GuitarModelSettings.EnableDiagramTitle = false;

        if (request.ModeId.HasValue)
        {
            guitarModel.SetMode(request.ModeId.Value);
        }
        else if (request.ScaleId.HasValue)
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

        return guitarModel;
    }

    private static int GetPitchClassFromC(Note note)
    {
        var pitchClass = (int)note - (int)Note.C;
        if (pitchClass < 0)
        {
            pitchClass += 12;
        }

        return pitchClass;
    }

    private static List<DiagramMarkerDto> BuildPlayablePattern(IReadOnlyList<DiagramMarkerDto> markers, int maxFrets)
    {
        if (markers == null || markers.Count == 0)
        {
            return new List<DiagramMarkerDto>();
        }

        var highestFret = Math.Max(maxFrets, markers.Max(m => m.FretNumber));
        var windowSize = Math.Min(5, Math.Max(3, highestFret));

        var bestWindowStart = 0;
        var bestWindowScore = -1;
        var searchEnd = Math.Max(0, highestFret - windowSize);

        for (var start = 0; start <= searchEnd; start++)
        {
            var end = start + windowSize;
            var score = markers
                .Where(m => m.FretNumber >= start && m.FretNumber <= end)
                .GroupBy(m => $"{m.NoteName}:{m.Octave}")
                .Count();

            if (score > bestWindowScore)
            {
                bestWindowScore = score;
                bestWindowStart = start;
            }
        }

        var bestWindowEnd = bestWindowStart + windowSize;
        var windowMarkers = markers
            .Where(m => m.FretNumber >= bestWindowStart && m.FretNumber <= bestWindowEnd)
            .ToList();

        if (windowMarkers.Count == 0)
        {
            return new List<DiagramMarkerDto>();
        }

        var markerGroups = windowMarkers
            .GroupBy(m => $"{m.NoteName}:{m.Octave}")
            .OrderBy(g => g.Min(m => m.Frequency))
            .ToList();

        var selected = new List<DiagramMarkerDto>();
        int? previousString = null;
        var windowCenter = bestWindowStart + (windowSize / 2.0);

        foreach (var group in markerGroups)
        {
            DiagramMarkerDto chosen;

            if (previousString.HasValue)
            {
                var preferredNextString = previousString.Value + 1;
                chosen = group
                    .OrderBy(m => Math.Abs(m.StringNumber - preferredNextString))
                    .ThenBy(m => Math.Abs(m.FretNumber - windowCenter))
                    .ThenBy(m => m.FretNumber)
                    .First();
            }
            else
            {
                chosen = group
                    .OrderBy(m => Math.Abs(m.FretNumber - windowCenter))
                    .ThenBy(m => m.StringNumber)
                    .First();
            }

            selected.Add(chosen);
            previousString = chosen.StringNumber;
        }

        return selected;
    }

    private static NoteInstance GetNoteAtFret(NoteInstance openString, int fretNumber)
    {
        var absoluteIndex = openString.NoteIndexPosAcrossOctaves + fretNumber;
        var octave = absoluteIndex / 12;
        var pitchClassFromC = absoluteIndex % 12;

        if (pitchClassFromC < 0)
        {
            pitchClassFromC += 12;
        }

        var noteValue = (pitchClassFromC + (int)Note.C) % 12;
        var note = (Note)noteValue;

        return new NoteInstance(note, octave);
    }
}
