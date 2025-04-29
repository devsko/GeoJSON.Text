// Copyright © Joerg Battermann 2014, Matt Hunt 2017

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using GeoJSON.Text.Geometry;

namespace GeoJSON.Text.Converters
{
    /// <summary>
    /// Converts <see cref="IGeometryObject"/> types to and from JSON.
    /// </summary>
    public class GeometryConverter : JsonConverter<IGeometryObject>
    {
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
                    return value.Deserialize((JsonTypeInfo<Point>)options.GetTypeInfo(typeof(Point)));
                case GeoJSONObjectType.MultiPoint:
                    return value.Deserialize((JsonTypeInfo<MultiPoint>)options.GetTypeInfo(typeof(MultiPoint)));
                case GeoJSONObjectType.LineString:
                    return value.Deserialize((JsonTypeInfo<LineString>)options.GetTypeInfo(typeof(LineString)));
                case GeoJSONObjectType.MultiLineString:
                    return value.Deserialize((JsonTypeInfo<MultiLineString>)options.GetTypeInfo(typeof(MultiLineString)));
                case GeoJSONObjectType.Polygon:
                    return value.Deserialize((JsonTypeInfo<Polygon>)options.GetTypeInfo(typeof(Polygon)));
                case GeoJSONObjectType.MultiPolygon:
                    return value.Deserialize((JsonTypeInfo<MultiPolygon>)options.GetTypeInfo(typeof(MultiPolygon)));
                case GeoJSONObjectType.GeometryCollection:
                    return value.Deserialize((JsonTypeInfo<GeometryCollection>)options.GetTypeInfo(typeof(GeometryCollection)));
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
                    JsonSerializer.Serialize(writer, (Point)value, (JsonTypeInfo<Point>)options.GetTypeInfo(typeof(Point)));
                    break;
                case GeoJSONObjectType.MultiPoint:
                    JsonSerializer.Serialize(writer, (MultiPoint)value, (JsonTypeInfo<MultiPoint>)options.GetTypeInfo(typeof(MultiPoint)));
                    break;
                case GeoJSONObjectType.LineString:
                    JsonSerializer.Serialize(writer, (LineString)value, (JsonTypeInfo<LineString>)options.GetTypeInfo(typeof(LineString)));
                    break;
                case GeoJSONObjectType.MultiLineString:
                    JsonSerializer.Serialize(writer, (MultiLineString)value, (JsonTypeInfo<MultiLineString>)options.GetTypeInfo(typeof(MultiLineString)));
                    break;
                case GeoJSONObjectType.Polygon:
                    JsonSerializer.Serialize(writer, (Polygon)value, (JsonTypeInfo<Polygon>)options.GetTypeInfo(typeof(Polygon)));
                    break;
                case GeoJSONObjectType.MultiPolygon:
                    JsonSerializer.Serialize(writer, (MultiPolygon)value, (JsonTypeInfo<MultiPolygon>)options.GetTypeInfo(typeof(MultiPolygon)));
                    break;
                case GeoJSONObjectType.GeometryCollection:
                    JsonSerializer.Serialize(writer, (GeometryCollection)value, (JsonTypeInfo<GeometryCollection>)options.GetTypeInfo(typeof(GeometryCollection)));
                    break;
                case GeoJSONObjectType.Feature:
                case GeoJSONObjectType.FeatureCollection:
                default:
                    throw new NotSupportedException("Feature and FeatureCollection types are Feature objects and not Geometry objects");
            }
        }
    }
}