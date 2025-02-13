// Copyright © Joerg Battermann 2014, Matt Hunt 2017

using GeoJSON.Text.Geometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeoJSON.Text.Converters
{
    /// <summary>
    /// Converts <see cref="IGeometryObject"/> types to and from JSON.
    /// </summary>
    public class GeometryConverter : JsonConverter<IGeometryObject>
    {
        /// <summary>
        ///     Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///     <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(IGeometryObject).IsAssignableFromType(objectType);
        }

        /// <summary>
        ///     Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        ///     The object value.
        /// </returns>
        public override IGeometryObject Read(
            ref Utf8JsonReader reader,
            Type type,
            JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.StartObject:
                    return ReadGeoJson(ref reader, options);
            }

            throw new JsonException($"expected null, object or array token but received {reader.TokenType}");
        }

        /// <summary>
        /// Reads the geo json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="Newtonsoft.Json.JsonReaderException">
        /// json must contain a "type" property
        /// or
        /// type must be a valid geojson geometry object type
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// Feature and FeatureCollection types are Feature objects and not Geometry objects
        /// </exception>
        private static IGeometryObject ReadGeoJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var document = JsonDocument.ParseValue(ref reader);
            JsonElement value = document.RootElement;
            JsonElement token;

            if (!value.TryGetProperty("type", out token))
            {
                throw new JsonException("json must contain a \"type\" property");
            }

            GeoJSONObjectType geoJsonType;

            if (!Enum.TryParse(token.GetString(), true, out geoJsonType))
            {
                throw new JsonException("type must be a valid geojson geometry object type");
            }

            switch (geoJsonType)
            {
                case GeoJSONObjectType.Point:
                    return value.Deserialize(GeoJSONContext.Default.Point);
                case GeoJSONObjectType.MultiPoint:
                    return value.Deserialize(GeoJSONContext.Default.MultiPoint);
                case GeoJSONObjectType.LineString:
                    return value.Deserialize(GeoJSONContext.Default.LineString);
                case GeoJSONObjectType.MultiLineString:
                    return value.Deserialize(GeoJSONContext.Default.MultiLineString);
                case GeoJSONObjectType.Polygon:
                    return value.Deserialize(GeoJSONContext.Default.Polygon);
                case GeoJSONObjectType.MultiPolygon:
                    return value.Deserialize(GeoJSONContext.Default.MultiPolygon);
                case GeoJSONObjectType.GeometryCollection:
                    return value.Deserialize(GeoJSONContext.Default.GeometryCollection);
                case GeoJSONObjectType.Feature:
                case GeoJSONObjectType.FeatureCollection:
                default:
                    throw new NotSupportedException("Feature and FeatureCollection types are Feature objects and not Geometry objects");
            }
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void Write(
            Utf8JsonWriter writer,
            IGeometryObject value,
            JsonSerializerOptions options)
        {
            // Standard serialization
            switch (value.Type)
            {
                case GeoJSONObjectType.Point:
                    JsonSerializer.Serialize(writer, (Point)value, GeoJSONContext.Default.Point);
                    break;
                case GeoJSONObjectType.MultiPoint:
                    JsonSerializer.Serialize(writer, (MultiPoint)value, GeoJSONContext.Default.MultiPoint);
                    break;
                case GeoJSONObjectType.LineString:
                    JsonSerializer.Serialize(writer, (LineString)value, GeoJSONContext.Default.LineString);
                    break;
                case GeoJSONObjectType.MultiLineString:
                    JsonSerializer.Serialize(writer, (MultiLineString)value, GeoJSONContext.Default.MultiLineString);
                    break;
                case GeoJSONObjectType.Polygon:
                    JsonSerializer.Serialize(writer, (Polygon)value, GeoJSONContext.Default.Polygon);
                    break;
                case GeoJSONObjectType.MultiPolygon:
                    JsonSerializer.Serialize(writer, (MultiPolygon)value, GeoJSONContext.Default.MultiPolygon);
                    break;
                case GeoJSONObjectType.GeometryCollection:
                    JsonSerializer.Serialize(writer, (GeometryCollection)value, GeoJSONContext.Default.GeometryCollection);
                    break;
                case GeoJSONObjectType.Feature:
                case GeoJSONObjectType.FeatureCollection:
                default:
                    throw new NotSupportedException("Feature and FeatureCollection types are Feature objects and not Geometry objects");
            }
        }

    }
}