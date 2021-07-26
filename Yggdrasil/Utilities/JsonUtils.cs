using System;
using System.Text.Json;

namespace Yggdrasil.Utilities {
    public static class JsonUtils {
        private readonly static JsonSerializerOptions _serializerOptions = new JsonSerializerOptions {
            IncludeFields = false,
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        public static string Serialize(object data) => JsonSerializer.Serialize(data, _serializerOptions);
        public static object Deserialize(Type type, string data) => JsonSerializer.Deserialize(data, type, _serializerOptions);
        public static T Deserialize<T>(string data) => JsonSerializer.Deserialize<T>(data, _serializerOptions);
    }
}
