using System;
using System.Text.Json.Serialization;
using GeoJSON.Text.Converters;
using GeoJSON.Text.CoordinateReferenceSystem;
using GeoJSON.Text.Feature;
using GeoJSON.Text.Geometry;

namespace GeoJSON.Text;

[JsonSerializable(typeof(NamedCRS))]
[JsonSerializable(typeof(LinkedCRS))]
[JsonSerializable(typeof(Feature<Point>))]
[JsonSerializable(typeof(Feature<MultiPoint>))]
[JsonSerializable(typeof(Feature<LineString>))]
[JsonSerializable(typeof(Feature<MultiLineString>))]
[JsonSerializable(typeof(Feature<Polygon>))]
[JsonSerializable(typeof(Feature<MultiPolygon>))]
[JsonSerializable(typeof(Feature<GeometryCollection>))]
[JsonSerializable(typeof(FeatureCollection))]
[JsonSerializable(typeof(IGeometryObject))]
[JsonSerializable(typeof(FeatureCollection))]
public partial class GeoJSONSerializerContext : JsonSerializerContext
{ 
}
