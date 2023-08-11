using System.Drawing;

namespace Model.SystemConfig
{
    public class ThemeColor
    {
        public string BackColor { get; set; } = default!;
        public string FontColor { get; set; } = default!;
    }

    public class ThemeConfig
    {
        public string BackColor { get; set; } = default!;
        public FontConfig Font { get; set; } = default!;
        public DateConfig Date { get; set; } = default!;
        public TimeConfig Time { get; set; } = default!;
    }

    public class FontConfig
    {
        public string Name { get; set; } = default!;
        public FontStyle Style { get; set; }
    }

    public class DateConfig
    {
        public bool Enable { get; set; } = false;
        public string YearColor { get; set; } = default!;
        public string MonthColor { get; set; } = default!;
        public string DayColor { get; set; } = default!;
        public string HyphenColor { get; set; } = default!;
    }

    public class TimeConfig
    {
        public string HourColor { get; set; } = default!;
        public string MinuteColor { get; set; } = default!;
        public string SecondColor { get; set; } = default!;
        public string HyphenColor { get; set; } = default!;
    }
}
