using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using GeoJSON.Text.CoordinateReferenceSystem;
using GeoJSON.Text.Feature;
using GeoJSON.Text.Geometry;
using NUnit.Framework;

namespace GeoJSON.Text.Tests.Feature;

[TestFixture]
public partial class FeatureCollectionTests : TestBase
{
    [Test]
    public void Ctor_Throws_ArgumentNullException_When_Features_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var featureCollection = new FeatureCollection(null);
        });
    }

    [Test]
    public void Can_Deserialize()
    {   
        string json = GetExpectedJson();

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json);
        var featureCollection_sg = JsonSerializer.Deserialize(json, GeoJSONSerializerContext.Default.FeatureCollection);

        Assert.IsNotNull(featureCollection);
        Assert.IsNotNull(featureCollection_sg);
        Assert.IsNotNull(featureCollection.Features);
        Assert.IsNotNull(featureCollection_sg.Features);
        Assert.AreEqual(featureCollection.Features.Count, 3);
        Assert.AreEqual(featureCollection_sg.Features.Count, 3);
        Assert.AreEqual(featureCollection.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.Point), 1);
        Assert.AreEqual(featureCollection_sg.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.Point), 1);
        Assert.AreEqual(featureCollection.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.MultiPolygon), 1);
        Assert.AreEqual(featureCollection_sg.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.MultiPolygon), 1);
        Assert.AreEqual(featureCollection.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.Polygon), 1);
        Assert.AreEqual(featureCollection_sg.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.Polygon), 1);
    }

    [Test]
    public void Can_DeserializeGeneric()
    {
        string json = GetExpectedJson();

        var featureCollection =
            JsonSerializer.Deserialize<FeatureCollection<FeatureCollectionTestPropertyObject>>(json);

        var featureCollection_sg =
            JsonSerializer.Deserialize(json, FeatureCollectionTestsContext.Default.FeatureCollectionFeatureCollectionTestPropertyObject);

        Assert.IsNotNull(featureCollection);
        Assert.IsNotNull(featureCollection_sg);
        Assert.IsNotNull(featureCollection.Features);
        Assert.IsNotNull(featureCollection_sg.Features);
        Assert.AreEqual(featureCollection.Features.Count, 3);
        Assert.AreEqual(featureCollection_sg.Features.Count, 3);
        Assert.AreEqual("DD", featureCollection.Features.First().Properties.name);
        Assert.AreEqual("DD", featureCollection_sg.Features.First().Properties.name);
        Assert.AreEqual(123, featureCollection.Features.First().Properties.size);
        Assert.AreEqual(123, featureCollection_sg.Features.First().Properties.size);
    }

    [Test]
    public void FeatureCollectionSerialization()
    {
        var model = new FeatureCollection();
        for (var i = 10; i-- > 0;)
        {
            var geom = new LineString(new[]
            {
                new Position(51.010, -1.034),
                new Position(51.010, -0.034)
            });

            var props = new Dictionary<string, object>
            {
                { "test1", "1" },
                { "test2", 2 }
            };

            var feature = new Text.Feature.Feature(geom, props);
            model.Features.Add(feature);
        }

        var actualJson = JsonSerializer.Serialize(model);
        var actualJson_sg = JsonSerializer.Serialize(model, FeatureCollectionTestsContext.Default.FeatureCollection);

        Assert.IsNotNull(actualJson);
        Assert.IsNotNull(actualJson_sg);

        Assert.IsFalse(string.IsNullOrEmpty(actualJson));
        Assert.IsFalse(string.IsNullOrEmpty(actualJson_sg));
    }

    [Test]
    public void FeatureCollection_Equals_GetHashCode_Contract()
    {
        var left = GetFeatureCollection();
        var right = GetFeatureCollection();

        Assert_Are_Equal(left, right);
    }

    [Test]
    public void Serialized_And_Deserialized_FeatureCollection_Equals_And_Share_HashCode()
    {
        var leftFc = GetFeatureCollection();
        var leftJson = JsonSerializer.Serialize(leftFc);
        var leftJson_sg = JsonSerializer.Serialize(leftFc, FeatureCollectionTestsContext.Default.FeatureCollection);
        var left = JsonSerializer.Deserialize<FeatureCollection>(leftJson);
        var left_sg = JsonSerializer.Deserialize(leftJson, FeatureCollectionTestsContext.Default.FeatureCollection);

        var rightFc = GetFeatureCollection();
        var rightJson = JsonSerializer.Serialize(rightFc);
        var rightJson_sg = JsonSerializer.Serialize(rightFc, FeatureCollectionTestsContext.Default.FeatureCollection);
        var right = JsonSerializer.Deserialize<FeatureCollection>(rightJson);
        var right_sg = JsonSerializer.Deserialize<FeatureCollection>(rightJson, FeatureCollectionTestsContext.Default.FeatureCollection);

        Assert_Are_Equal(left, right);
        Assert_Are_Equal(left_sg, right_sg);
        Assert_Are_Equal(left, left_sg);
    }

    [Test]
    public void FeatureCollection_Test_IndexOf()
    {
        var model = new FeatureCollection();
        var expectedIds = new List<string>();
        var expectedIndexes = new List<int>();

        for (var i = 0; i < 10; i++)
        {
            var id = "id" + i;

            expectedIds.Add(id);
            expectedIndexes.Add(i);

            var geom = new LineString(new[]
            {
                new Position(51.010, -1.034),
                new Position(51.010, -0.034)
            });

            var props = FeatureTests.GetPropertiesInRandomOrder();

            var feature = new Text.Feature.Feature(geom, props, id);
            model.Features.Add(feature);
        }

        for (var i = 0; i < 10; i++)
        {
            var actualFeature = model.Features[i];
            var actualId = actualFeature.Id;
            var actualIndex = model.Features.IndexOf(actualFeature);

            var expectedId = expectedIds[i];
            var expectedIndex = expectedIndexes[i];

            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedIndex, actualIndex);

            Assert.Inconclusive("not supported. the Feature.Id is optional. " +
                                " create a new class that inherits from" +
                                " Feature and then override Equals and GetHashCode");
        }
    }


    private FeatureCollection GetFeatureCollection()
    {
        var model = new FeatureCollection();
        for (var i = 10; i-- > 0;)
        {
            var geom = new LineString(new[]
            {
                new Position(51.010, -1.034),
                new Position(51.010, -0.034)
            });

            var props = FeatureTests.GetPropertiesInRandomOrder();

            var feature = new Text.Feature.Feature(geom, props);
            model.Features.Add(feature);
        }

        return model;
    }

    private void Assert_Are_Equal(FeatureCollection left, FeatureCollection right)
    {
        Assert.AreEqual(left, right);

        Assert.IsTrue(left.Equals(right));
        Assert.IsTrue(right.Equals(left));

        Assert.IsTrue(left.Equals(left));
        Assert.IsTrue(right.Equals(right));

        Assert.IsTrue(left == right);
        Assert.IsTrue(right == left);

        Assert.IsFalse(left != right);
        Assert.IsFalse(right != left);

        Assert.AreEqual(left.GetHashCode(), right.GetHashCode());
    }

    private class FeatureCollectionTestPropertyObject
    {
        public string name { get; set; }
        public int size { get; set; }
    }

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
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(bool))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(TestFeatureEnum))]
    [JsonSerializable(typeof(FeatureCollection))]
    [JsonSerializable(typeof(FeatureCollection<FeatureCollectionTestPropertyObject>))]
    private partial class FeatureCollectionTestsContext : JsonSerializerContext
    {
    }
}