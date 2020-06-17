# Scalex Guitar Toolkit
A cross-platform .net based guitar toolkit providing Scales, Chords and Tablature.

This app is a work in progress cross platform version (mainly using Xamarin Forms).

## Scales
![Scales in any tuning for multiple instrument types](docs/screenshots/scalex.scales.windows10.png)

## Chords
![](docs/screenshots/scalex.chords.windows10.png)

## Tablature
![](docs/screenshots/scalex.tablature.windows10.png)
The Tablature feature uses AlphaTab for rendering and the Songsterr API for content.

## Bonus Content - 3D Guitar Designer
![](docs/screenshots/scalex.guitardesigner.windows10.png)
Not part of the app codebase but a pretty interesting distraction for all guitarists.

## History
Scalex was originally written in Turbo Pascal in 1994. In 2001 the scale diagram logic was ported to C# and some of that code remains today (sort of). 

Over the years versions of this app have been ported to a number of platforms:
- A windows forms desktop app
- A WPF desktop app
- Web based (both aspx rendering to PNG and JavaScript transpiled using SharpKit)
- A WPF app for Windows Phone and Windows Store (Soundshed.com Guitar Toolkit)
- A mobile web app (iOS and Android)


Nuget for AlphaTab:
https://www.myget.org/feed/coderline/package/nuget/AlphaTab
Install-Package AlphaTab -Version 0.9.3.175 -Source https://www.myget.org/F/coderline/api/v3/index.json