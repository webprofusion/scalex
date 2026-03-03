using System.Collections.Generic;

namespace Scalex.RazorPages.Services;

public sealed record PlaybackNoteDto(
    string NoteName,
    double Frequency,
    int DurationMs,
    string? MarkerKey);

public sealed record PlaybackDataDto(
    string Title,
    IReadOnlyList<PlaybackNoteDto> ScaleAscending,
    IReadOnlyList<PlaybackNoteDto> ScaleDescending,
    IReadOnlyList<PlaybackNoteDto> Melody);

public sealed record DiagramMarkerDto(
    double XRatio,
    double YRatio,
    double RadiusRatio,
    string NoteName,
    double Frequency,
    int StringNumber,
    int FretNumber,
    int Octave);

public sealed record DiagramMapDto(
    int Width,
    int Height,
    IReadOnlyList<DiagramMarkerDto> Markers);