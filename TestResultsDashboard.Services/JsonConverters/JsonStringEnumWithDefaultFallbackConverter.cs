using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestResultsDashboard.Services.JsonConverters;

public class JsonStringEnumWithDefaultFallbackConverter: JsonConverterFactoryDecorator
{
    public JsonStringEnumWithDefaultFallbackConverter(JsonStringEnumConverter inner) : base(inner) { }
    public JsonStringEnumWithDefaultFallbackConverter() : this(new JsonStringEnumConverter()) { }

    protected virtual T GetDefaultValue<T>() where T : struct, Enum => default(T);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var inner = base.CreateConverter(typeToConvert, options);
        return (JsonConverter?)Activator.CreateInstance(typeof(EnumConverterDecorator<>).MakeGenericType(Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert), new object? [] { this, inner });
    }

    private sealed class EnumConverterDecorator<T> : JsonConverter<T> where T : struct, Enum
    {
        private readonly JsonStringEnumWithDefaultFallbackConverter _parent;
        private readonly JsonConverter<T> _inner;
        public EnumConverterDecorator(JsonStringEnumWithDefaultFallbackConverter parent, JsonConverter inner) => 
            (_parent, _inner)= (parent ?? throw new ArgumentException(nameof(parent)), (inner as JsonConverter<T>) ?? throw new ArgumentException(nameof(inner)));

        public override bool CanConvert(Type typeToConvert) => _inner.CanConvert(typeToConvert);
        
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                return _inner.Read(ref reader, typeToConvert, options);
            }
            catch (JsonException)
            {
                return _parent.GetDefaultValue<T>();
            }
        }
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => _inner.Write(writer, value, options);
    }
}

public class JsonConverterFactoryDecorator : JsonConverterFactory
{
    private readonly JsonConverterFactory _inner;
    protected JsonConverterFactoryDecorator(JsonConverterFactory inner) => _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    public override bool CanConvert(Type typeToConvert) => _inner.CanConvert(typeToConvert);
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) => _inner.CreateConverter(typeToConvert, options);
}