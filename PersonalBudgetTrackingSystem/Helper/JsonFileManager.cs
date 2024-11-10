using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace PersonalBudgetTrackingSystem.Helper
{
    
    public static class JsonFileManager
    {
        public static List<T> LoadData<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<T>();

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<T>>(json)!;
        }

        public static void SaveData<T>(string filePath, List<T> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }

}
