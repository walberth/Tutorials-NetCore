namespace EFCore_Demo.Transversal
{
    using System.Text.Json;

    public static class Json {
        public static string Serialize(this object value) {
            return JsonSerializer.Serialize(value);
        }
    }
}
