using System.Collections.Generic;

namespace Webprofusion.Scalex.Lessons
{
    public enum MediaContentType
    {
        GuitarPro,
        VideoEmbed,
        Audio
    }

    public class MediaItem
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string URL { get; set; }
        public MediaContentType ContentType { get; set; }
        public string SourceType { get; set; }
        public string SourceValue { get; set; }
        public int? ItemIndex { get; set; }
        public decimal Position { get; set; }
    }

    /// <summary>
    /// Time sync point for a media item (used relative to other media items) 
    /// </summary>
    public class MediaSyncItem
    {
        public string Id { get; set; }
        public decimal Position { get; set; }
    }

    /// <summary>
    /// Time Syncronisation points for one or more media items 
    /// </summary>
    public class MediaSyncPoint
    {
        public string Title { get; set; }
        public List<MediaSyncItem> SyncItems { get; set; }
    }

    public class Section
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Section> Subsections { get; set; }
        public List<string> MediaItemIds { get; set; }
        public List<MediaSyncPoint> SyncPoints { get; set; }
    }

    public class CourseUnit
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Level { get; set; }
        public string Genres { get; set; }
        public string Instrument { get; set; }
        public string Authors { get; set; }
        public string ContactEmail { get; set; }
        public string WebsiteURL { get; set; }
    }

    public class Lesson : CourseUnit
    {
        public List<Section> Sections { get; set; }
        public List<MediaItem> MediaItems { get; set; }
    }

    public class Course : CourseUnit
    {
        public Course()
            : base()
        {
            this.Lessons = new List<Lesson>();
        }

        public List<Lesson> Lessons { get; set; }
    }

    public class LessonListItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }

        public string LessonDataUri { get; set; }
        public string Levels { get; set; }
        public string Instrument { get; set; }
    }

    public class ExampleCourse
    {
        public Course GetExample()
        {
            var course = new Course();
            course.Title = "Guitar For Beginners";
            course.Instrument = "Guitar";
            course.Summary = "An introduction to guitar playing.";
            course.Level = "Beginner";
            course.Genres = "All";

            var lesson1 = new Lesson
            {
                Title = "Getting Started",
                Level = "Beginner",
                Summary = "Introduction to the guitar and types of playing."
            };

            lesson1.Sections.Add(new Section
            {
                Title = "Parts of the Guitar",
                Content = "Your guitar probably has 6 strings, some designs have more, some have less. It may be an acoustic guitar (large, wooden and hollow) or electric (solid body with a socket to plug into an amplifier)."
            });

            lesson1.Sections.Add(new Section
            {
                Title = "Types of playing",
                Content = "Generally when playing guitar you are either playing Chords (several notes strummed together to play at the same time) or individual notes (one finger at a time)."
            });

            course.Lessons.Add(lesson1);

            return course;
        }
    }
}