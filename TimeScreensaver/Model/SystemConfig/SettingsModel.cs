namespace Model.SystemConfig
{
    public class SettingsModel
    {
        public int ThemeColorIndex { get; set; } = 0;
        public List<ThemeColorModel> ThemeColors { get; set; } = default!;
    }
}