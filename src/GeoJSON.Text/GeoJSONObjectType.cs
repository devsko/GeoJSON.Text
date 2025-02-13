// Copyright © Joerg Battermann 2014, Matt Hunt 2017

using System.Text.Json.Serialization;

namespace GeoJSON.Text
{
    /// <summary>
    /// Defines the GeoJSON Objects types.
    /// </summary>
    public enum GeoJSONObjectType
    {
        /// <summary>
        /// Defines the Point type.
        /// </summary>
        /// <remarks>
        /// See https://tools.ietf.org/html/rfc7946#section-3.1.2
        /// </remarks>
        [JsonStringEnumMemberName("Point")]
        Point,

        /// <summary>
        /// Defines the MultiPoint type.
        /// </summary>
        /// <remarks>
        /// See https://tools.ietf.org/html/rfc7946#section-3.1.3
        /// </remarks>
        [JsonStringEnumMemberName("MultiPoint")]
        MultiPoint,

        /// <summary>
        /// Defines the LineString type.
        /// </summary>
        /// <remarks>
        /// See https://tools.ietf.org/html/rfc7946#section-3.1.4
        /// </remarks>
        [JsonStringEnumMemberName("LineString")]
        LineString,

        /// <summary>
        /// Defines the MultiLineString type.
        /// </summary>
        /// <remarks>
        /// See https://tools.ietf.org/html/rfc7946#section-3.1.5
        /// </remarks>
        [JsonStringEnumMemberName("MultiLineString")]
        MultiLineString,

        /// <summary>
        /// Defines the Polygon type.
        /// </summary>
        /// <remarks>
        /// See https://tools.ietf.org/html/rfc7946#section-3.1.6
        /// </remarks>
        [JsonStringEnumMemberName("Polygon")]
        Polygon,

        /// <summary>
        /// Defines the MultiPolygon type.
        /// </summary>
        /// <remarks>
        /// See https://tools.ietf.org/html/rfc7946#section-3.1.7
        /// </remarks>
        [JsonStringEnumMemberName("MultiPolygon")]
        MultiPolygon,

        /// <summary>
        /// Defines the GeometryCollection type.
        /// </summary>
        /// <remarks>
        /// See https://tools.ietf.org/html/rfc7946#section-3.1.8
        /// </remarks>
        [JsonStringEnumMemberName("GeometryCollection")]
        GeometryCollection,

        /// <summary>
        /// Defines the Feature type.
        /// </summary>
        /// <remarks>
        /// See https://tools.ietf.org/html/rfc7946#section-3.2
        /// </remarks>
        [JsonStringEnumMemberName("Feature")]
        Feature,

        /// <summary>
        /// Defines the FeatureCollection type.
        /// </summary>
        /// <remarks>
        /// See https://tools.ietf.org/html/rfc7946#section-3.3
        /// </remarks>
        [JsonStringEnumMemberName("FeatureCollection")]
        FeatureCollection
    }
}
