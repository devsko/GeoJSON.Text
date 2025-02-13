using System.Text.Json.Serialization;
using GeoJSON.Text.CoordinateReferenceSystem;
using GeoJSON.Text.Feature;
using GeoJSON.Text.Geometry;

namespace GeoJSON.Text
{
    [JsonSerializable(typeof(double[]))]
    [JsonSerializable(typeof(CRSBase))]
    [JsonSerializable(typeof(NamedCRS))]
    [JsonSerializable(typeof(LinkedCRS))]
    [JsonSerializable(typeof(Point))]
    [JsonSerializable(typeof(MultiPoint))]
    [JsonSerializable(typeof(LineString))]
    [JsonSerializable(typeof(MultiLineString))]
    [JsonSerializable(typeof(Polygon))]
    [JsonSerializable(typeof(MultiPolygon))]
    [JsonSerializable(typeof(GeometryCollection))]
    internal partial class GeoJSONContext : JsonSerializerContext
    {
    }
}
