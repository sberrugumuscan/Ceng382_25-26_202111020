using System.Text.Json;

namespace IndexPage.Helpers
{
    public static class Util
    {
        public static byte[] ExportToJson<T>(T data)
        {
            if (EqualityComparer<T>.Default.Equals(data, default)) 
                return new byte[0]; // Null check geliştirildi 🔥
            
            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.SerializeToUtf8Bytes(data, options);
        }

        public static string GenerateExportFilename(string prefix = "classes")
        {
            return $"{prefix}_{DateTime.Now:yyyyMMddHHmmss}.json";
        }
    }
}