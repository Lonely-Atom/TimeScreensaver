namespace Model.SystemConfig
{
    public class SettingsModel
    {
        public int MinimumWidth { get; } = 60;
        public int MinimumHeight { get; } = 40;
        public int ThemeColorIndex { get; set; } = 0;
        public List<ThemeColorModel> ThemeColors { get; set; } = default!;
    }
}