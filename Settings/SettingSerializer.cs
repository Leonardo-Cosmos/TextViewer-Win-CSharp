/* 2021/4/16 */
using System;
using System.IO;
using System.Text.Json;

namespace TextViewer.Settings
{
    static class SettingSerializer
    {
        private const string settingFilePath = "./setting.json";


        public static void Save(TextViewerSetting setting)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize<TextViewerSetting>(setting, options);

            try
            {
                File.WriteAllText(settingFilePath, json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        public static TextViewerSetting Load()
        {
            string json;
            try
            {
               json  = File.ReadAllText(settingFilePath);
            } catch (Exception ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
                return null;
            }

            var setting = JsonSerializer.Deserialize<TextViewerSetting>(json);
            return setting;
        }

    }
}
