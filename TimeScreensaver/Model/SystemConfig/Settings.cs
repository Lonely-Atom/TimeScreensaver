using System.Drawing;

namespace Model.SystemConfig
{
    public class Settings
    {
        public int MinimumWidth { get; set; }
        public int MinimumHeight { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public float FontSize { get; set; }

        public Point Location { get; set; }

        public double Opacity { get; set; }

        public bool IsTransparent { get; set; }
        public bool IsSingleColor { get; set; }
        public bool Is24Hour { get; set; }

        public string TimeHyphen { get; set; } = default!;

        public int ThemeIndex { get; set; }

        public List<ThemeColor> ThemeColors { get; set; } = default!;

        public List<ThemeConfig> Themes { get; set; } = default!;
    }    
}