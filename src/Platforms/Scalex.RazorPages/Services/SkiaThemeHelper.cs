using Webprofusion.Scalex.Rendering;
using Webprofusion.Scalex.Util;
using SkiaSharp;

namespace Scalex.RazorPages.Services;

public static class SkiaThemeHelper
{
    public static void ApplyThemeColours(Generic2DRenderer surface)
    {
        var foregroundThemeColor = new SKColor(140, 140, 140);
        var mutedForegroundThemeColor = new SKColor(64, 64, 64);
        surface.ColorPalette[ThemeColorPreset.Background] = new ColorValue(0, 0, 0);
        surface.ColorPalette[ThemeColorPreset.Subtle] = new ColorValue(64, 64, 64);
        surface.ColorPalette[ThemeColorPreset.ForegroundText] = new ColorValue(foregroundThemeColor.Red, foregroundThemeColor.Green, foregroundThemeColor.Blue);
        surface.ColorPalette[ThemeColorPreset.Foreground] = new ColorValue(foregroundThemeColor.Red, foregroundThemeColor.Green, foregroundThemeColor.Blue);
        surface.ColorPalette[ThemeColorPreset.MutedForeground] = new ColorValue(mutedForegroundThemeColor.Red, mutedForegroundThemeColor.Green, mutedForegroundThemeColor.Blue);
        surface.ColorPalette[ThemeColorPreset.TextShadow] = new ColorValue(mutedForegroundThemeColor.Red, mutedForegroundThemeColor.Green, mutedForegroundThemeColor.Blue);
    }

    public static void ApplyDarkThemeColours(Generic2DRenderer surface)
    {
        var foregroundThemeColor = new SKColor(224, 224, 224);
        var mutedForegroundThemeColor = new SKColor(144, 144, 144);
        surface.ColorPalette[ThemeColorPreset.Background] = new ColorValue(10, 10, 12);
        surface.ColorPalette[ThemeColorPreset.MutedBackground] = new ColorValue(20, 20, 24);
        surface.ColorPalette[ThemeColorPreset.Subtle] = new ColorValue(64, 64, 70);
        surface.ColorPalette[ThemeColorPreset.ForegroundText] = new ColorValue(foregroundThemeColor.Red, foregroundThemeColor.Green, foregroundThemeColor.Blue);
        surface.ColorPalette[ThemeColorPreset.Foreground] = new ColorValue(foregroundThemeColor.Red, foregroundThemeColor.Green, foregroundThemeColor.Blue);
        surface.ColorPalette[ThemeColorPreset.MutedForeground] = new ColorValue(mutedForegroundThemeColor.Red, mutedForegroundThemeColor.Green, mutedForegroundThemeColor.Blue);
        surface.ColorPalette[ThemeColorPreset.TextShadow] = new ColorValue(mutedForegroundThemeColor.Red, mutedForegroundThemeColor.Green, mutedForegroundThemeColor.Blue);
        surface.ColorPalette[ThemeColorPreset.Accent] = new ColorValue(227, 6, 19);
    }
}
