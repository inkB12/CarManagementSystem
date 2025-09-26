using System.Text.Json;

namespace CarManagementSystem.WebMVC.Extensions
{
    public static class SessionExtensions
    {
        // Helper for setting object value
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Helper for getting object value
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
