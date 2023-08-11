using Microsoft.Extensions.Configuration;
using Model;
using Model.SystemConfig;
using Newtonsoft.Json;

namespace Util
{
    public static class AppHelper
    {
        private static IConfigurationRoot ConfigurationRoot { get; set; } = default!;

        private const string SettingsFileName = "appsettings.json";
        private const string RootSection = "Settings";

        /// <summary>
        /// 映射 appsettings.json 文件中的配置到 GlobalVariable.Settings
        /// </summary>
        public static void GetSettings()
        {
            ConfigurationRoot = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(SettingsFileName, optional: false, reloadOnChange: true)
               .Build();

            GlobalVariable.Settings = ConfigurationRoot
                .GetSection(RootSection)
                .Get<Settings>() ?? new Settings();
        }

        /// <summary>
        /// 更新设置到 appsettings.json 文件中
        /// </summary>
        public static void UpdateSettings()
        {
            // 将 GlobalVariable.Settings 序列化为 Json 字符串
            // 由于 GlobalVariable 是静态类，无法序列化，
            // 所以使用匿名类包一层，保证序列化后的 Json 字符串包含名为 Settings 的 JsonObject 
            string jsonString = JsonConvert.SerializeObject(
                new {
                    GlobalVariable.Settings 
                }, 
                Formatting.Indented
            );

            // 覆写文件
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), SettingsFileName), jsonString);
        }
    }
}