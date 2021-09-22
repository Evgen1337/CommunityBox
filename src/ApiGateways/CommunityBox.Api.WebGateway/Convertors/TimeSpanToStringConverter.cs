using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CommunityBox.Api.WebGateway.Convertors
{
    public class TimeSpanToStringConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            return value == null 
                ? TimeSpan.Zero 
                : TimeSpan.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}