﻿// Copyright © Joerg Battermann 2014, Matt Hunt 2017

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using GeoJSON.Text.CoordinateReferenceSystem;

namespace GeoJSON.Text.Converters
{
    /// <summary>
    /// Converts <see cref="ICRSObject"/> types to and from JSON.
    /// </summary>
    public class CrsConverter : JsonConverter<ICRSObject>
    {
        public override bool HandleNull => true;

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(ICRSObject).IsAssignableFromType(objectType);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        /// <exception cref="Newtonsoft.Json.JsonReaderException">
        /// CRS must be null or a json object
        ///     or
        /// CRS must have a "type" property
        /// </exception>
        public override ICRSObject Read(
            ref Utf8JsonReader reader,
            Type type,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return new UnspecifiedCRS();
            }
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("CRS must be null or a json object");
            }

            var jObject = JsonDocument.ParseValue(ref reader).RootElement;
            JsonElement token;
            if (!jObject.TryGetProperty("type", out token))
            {
                throw new JsonException("CRS must have a \"type\" property");
            }

            var crsType = token.GetString();

            if (string.Equals("name", crsType, StringComparison.OrdinalIgnoreCase))
            {
                if (jObject.TryGetProperty("properties", out var properties))
                {
                    var name = properties.GetProperty("name").GetString();

                    var target = new NamedCRS(name);
                    var converted = jObject.Deserialize(GeoJSONContext.Default.NamedCRS);

                    if (converted.Properties != null)
                    {
                        foreach (var item in converted?.Properties)
                        {
                            target.Properties[item.Key] = item.Value;
                        }
                    }

                    return target;
                }
            }
            else if (string.Equals("link", crsType, StringComparison.OrdinalIgnoreCase))
            {
                if (jObject.TryGetProperty("properties", out var properties))
                {
                    var href = properties.GetProperty("href").GetString();

                    var target = new LinkedCRS(href);

                    var converted = jObject.Deserialize(GeoJSONContext.Default.LinkedCRS);

                    if (converted.Properties != null)
                    {
                        foreach (var item in converted?.Properties)
                        {
                            target.Properties[item.Key] = item.Value;
                        }
                    }

                    return target;
                }
            }

            throw new NotSupportedException(string.Format("Type {0} unexpected.", crsType));
        }

        /// <summary>
        ///     Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public override void Write(
            Utf8JsonWriter writer,
            ICRSObject value,
            JsonSerializerOptions options)
        {
            if(value == null)
                return;

            switch (value.Type)
            {
                case CRSType.Name:
                    JsonSerializer.Serialize(writer, value, GeoJSONContext.Default.NamedCRS);
                    break;
                case CRSType.Link:
                    JsonSerializer.Serialize(writer, value, GeoJSONContext.Default.LinkedCRS);
                    break;
                case CRSType.Unspecified:
                    writer.WriteNullValue();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
