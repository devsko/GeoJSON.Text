using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using GeoJSON.Text.CoordinateReferenceSystem;
using GeoJSON.Text.Feature;
using GeoJSON.Text.Geometry;
using NUnit.Framework;

namespace GeoJSON.Text.Tests.Feature
{
    [TestFixture]
    public partial class FeatureTests : TestBase
    {
        [Test]
        public void Can_Deserialize_Point_Feature()
        {
            var json = GetExpectedJson();

            var feature = JsonSerializer.Deserialize<Text.Feature.Feature>(json);
            var feature_sg = JsonSerializer.Deserialize(json, FeatureContext.Default.Feature);

            Assert.IsNotNull(feature);
            Assert.IsNotNull(feature_sg);
            Assert.IsNotNull(feature.Properties);
            Assert.IsNotNull(feature_sg.Properties);
            Assert.IsTrue(feature.Properties.Any());
            Assert.IsTrue(feature_sg.Properties.Any());

            Assert.IsTrue(feature.Properties.ContainsKey("name"));
            Assert.IsTrue(feature_sg.Properties.ContainsKey("name"));
            Assert.AreEqual(feature.Properties["name"].ToString(), "Dinagat Islands");
            Assert.AreEqual(feature_sg.Properties["name"].ToString(), "Dinagat Islands");

            Assert.AreEqual("test-id", feature.Id);
            Assert.AreEqual("test-id", feature_sg.Id);

            Assert.AreEqual(GeoJSONObjectType.Point, feature.Geometry.Type);
            Assert.AreEqual(GeoJSONObjectType.Point, feature_sg.Geometry.Type);
        }

        [Test]
        public void Can_Serialize_LineString_Feature()
        {
            var coordinates = new[]
            {
                new List<IPosition>
                {
                    new Position(52.370725881211314, 4.889259338378906),
                    new Position(52.3711451105601, 4.895267486572266),
                    new Position(52.36931095278263, 4.892091751098633),
                    new Position(52.370725881211314, 4.889259338378906)
                },
                new List<IPosition>
                {
                    new Position(52.370725881211314, 4.989259338378906),
                    new Position(52.3711451105601, 4.995267486572266),
                    new Position(52.36931095278263, 4.992091751098633),
                    new Position(52.370725881211314, 4.989259338378906)
                }
            };

            var geometry = new LineString(coordinates[0]);

            var actualJson = JsonSerializer.Serialize(new Text.Feature.Feature(geometry));
            var actualJson_sg = JsonSerializer.Serialize(new Text.Feature.Feature(geometry), FeatureContext.Default.Feature);

            Console.WriteLine(actualJson);

            var expectedJson = GetExpectedJson();

            JsonAssert.AreEqual(expectedJson, actualJson);
            JsonAssert.AreEqual(expectedJson, actualJson_sg);
        }

        [Test]
        public void Can_Serialize_MultiLineString_Feature()
        {
            var geometry = new MultiLineString(new List<LineString>
            {
                new LineString(new List<IPosition>
                {
                    new Position(52.370725881211314, 4.889259338378906),
                    new Position(52.3711451105601, 4.895267486572266),
                    new Position(52.36931095278263, 4.892091751098633),
                    new Position(52.370725881211314, 4.889259338378906)
                }),
                new LineString(new List<IPosition>
                {
                    new Position(52.370725881211314, 4.989259338378906),
                    new Position(52.3711451105601, 4.995267486572266),
                    new Position(52.36931095278263, 4.992091751098633),
                    new Position(52.370725881211314, 4.989259338378906)
                })
            });

            var expectedJson = GetExpectedJson();

            var actualJson = JsonSerializer.Serialize(new Text.Feature.Feature(geometry));
            var actualJson_sg = JsonSerializer.Serialize(new Text.Feature.Feature(geometry), FeatureContext.Default.Feature);

            JsonAssert.AreEqual(expectedJson, actualJson);
            JsonAssert.AreEqual(expectedJson, actualJson_sg);
        }

        [Test]
        public void Can_Serialize_Point_Feature()
        {
            var geometry = new Point(new Position(1, 2));
            var expectedJson = GetExpectedJson();

            var actualJson = JsonSerializer.Serialize(new Text.Feature.Feature(geometry));
            var actualJson_sg = JsonSerializer.Serialize(new Text.Feature.Feature(geometry), FeatureContext.Default.Feature);

            JsonAssert.AreEqual(expectedJson, actualJson);
            JsonAssert.AreEqual(expectedJson, actualJson_sg);
        }

        [Test]
        public void Can_Serialize_Polygon_Feature()
        {
            var coordinates = new List<IPosition>
            {
                new Position(52.370725881211314, 4.889259338378906),
                new Position(52.3711451105601, 4.895267486572266),
                new Position(52.36931095278263, 4.892091751098633),
                new Position(52.370725881211314, 4.889259338378906)
            };

            var polygon = new Polygon(new List<LineString> { new LineString(coordinates) });
            var properties = new Dictionary<string, object> { { "Name", "Foo" } };
            var feature = new Text.Feature.Feature(polygon, properties);

            var expectedJson = GetExpectedJson();
            var actualJson = JsonSerializer.Serialize(feature);
            var actualJson_sg = JsonSerializer.Serialize(feature, FeatureContext.Default.Feature);

            JsonAssert.AreEqual(expectedJson, actualJson);
            JsonAssert.AreEqual(expectedJson, actualJson_sg);
        }

        [Test]
        public void Can_Serialize_MultiPolygon_Feature()
        {
            var multiPolygon = new MultiPolygon(new List<Polygon>
            {
                new Polygon(new List<LineString>
                {
                    new LineString(new List<IPosition>
                    {
                        new Position(0, 0),
                        new Position(0, 1),
                        new Position(1, 1),
                        new Position(1, 0),
                        new Position(0, 0)
                    })
                }),
                new Polygon(new List<LineString>
                {
                    new LineString(new List<IPosition>
                    {
                        new Position(70, 70),
                        new Position(70, 71),
                        new Position(71, 71),
                        new Position(71, 70),
                        new Position(70, 70)
                    }),
                    new LineString(new List<IPosition>
                    {
                        new Position(80, 80),
                        new Position(80, 81),
                        new Position(81, 81),
                        new Position(81, 80),
                        new Position(80, 80)
                    })
                })
            });

            var feature = new Text.Feature.Feature(multiPolygon);

            var expectedJson = GetExpectedJson();
            var actualJson = JsonSerializer.Serialize(feature);
            var actualJson_sg = JsonSerializer.Serialize(feature, FeatureContext.Default.Feature);

            JsonAssert.AreEqual(expectedJson, actualJson);
            JsonAssert.AreEqual(expectedJson, actualJson_sg);
        }

        [Test]
        public void Can_Serialize_Dictionary_Subclass()
        {
            var properties =
                new TestFeaturePropertyDictionary()
                {
                     BooleanProperty = true,
                     DoubleProperty = 1.2345d,
                     EnumProperty = TestFeatureEnum.Value1,
                     IntProperty = -1,
                     StringProperty = "Hello, GeoJSON !"
                };

            Text.Feature.Feature feature = new Text.Feature.Feature(new Point(new Position(10, 10)), properties);

            var expectedJson = this.GetExpectedJson();
            var actualJson = JsonSerializer.Serialize(feature);
            var actualJson_sg = JsonSerializer.Serialize(feature, FeatureContext.Default.Feature);

            Assert.False(string.IsNullOrEmpty(expectedJson));
            JsonAssert.AreEqual(expectedJson, actualJson);
            JsonAssert.AreEqual(expectedJson, actualJson_sg);
        }

        [Test]
        public void Ctor_Can_Add_Properties_Using_Object_Inheriting_Dictionary()
        {
            int expectedProperties = 6;

            var properties = new TestFeaturePropertyDictionary()
            {
                BooleanProperty = true,
                DateTimeProperty = DateTime.Now,
                DoubleProperty = 1.2345d,
                EnumProperty = TestFeatureEnum.Value1,
                IntProperty = -1,
                StringProperty = "Hello, GeoJSON !"
            };

            Text.Feature.Feature feature = new Text.Feature.Feature(new Point(new Position(10, 10)), properties);

            Assert.IsNotNull(feature.Properties);
            Assert.IsTrue(feature.Properties.Count > 1);
            Assert.AreEqual(
                feature.Properties.Count,
                expectedProperties,
                $"Expected: {expectedProperties} Actual: {feature.Properties.Count}");
        }

        [Test]
        public void Feature_Equals_GetHashCode_Contract_Dictionary()
        {
            var leftDictionary = GetPropertiesInRandomOrder();
            var rightDictionary = GetPropertiesInRandomOrder();

            var geometry10 = new Position(10, 10);
            var geometry20 = new Position(20, 20);

            var left = new Text.Feature.Feature(new Point(
                geometry10),
                leftDictionary,
                "abc");
            var right = new Text.Feature.Feature(new Point(
                geometry20),
                rightDictionary,
                "abc");

            Assert_Are_Not_Equal(left, right); // different geometries


            left = new Text.Feature.Feature(new Point(
                geometry10),
                leftDictionary,
                "abc");
            right = new Text.Feature.Feature(new Point(
                geometry10),
                rightDictionary,
                "abc"); // identical geometries, different ids and or properties or not compared

            Assert_Are_Equal(left, right);

        }

        [Test]
        public void Serialized_And_Deserialized_Feature_Equals_And_Share_HashCode()
        {
            var geometry = GetGeometry();

            var leftFeature = new Text.Feature.Feature(geometry);
            var leftJson = JsonSerializer.Serialize(leftFeature);
            var leftJson_sg = JsonSerializer.Serialize(leftFeature, FeatureContext.Default.Feature);
            var left = JsonSerializer.Deserialize<Text.Feature.Feature>(leftJson);
            var left_sg = JsonSerializer.Deserialize(leftJson, FeatureContext.Default.Feature);

            var rightFeature = new Text.Feature.Feature(geometry);
            var rightJson = JsonSerializer.Serialize(rightFeature);
            var rightJson_sg = JsonSerializer.Serialize(rightFeature, FeatureContext.Default.Feature);
            var right = JsonSerializer.Deserialize<Text.Feature.Feature>(rightJson);
            var right_sg = JsonSerializer.Deserialize(rightJson, FeatureContext.Default.Feature);

            Assert_Are_Equal(left, right);
            Assert_Are_Equal(left_sg, right_sg);
            Assert_Are_Equal(left, left_sg);

            leftFeature = new Text.Feature.Feature(geometry, GetPropertiesInRandomOrder());
            leftJson = JsonSerializer.Serialize(leftFeature);
            leftJson_sg = JsonSerializer.Serialize(leftFeature, FeatureContext.Default.Feature);
            left = JsonSerializer.Deserialize<Text.Feature.Feature>(leftJson);
            left_sg = JsonSerializer.Deserialize(leftJson, FeatureContext.Default.Feature);

            rightFeature = new Text.Feature.Feature(geometry, GetPropertiesInRandomOrder());
            rightJson = JsonSerializer.Serialize(rightFeature);
            rightJson_sg = JsonSerializer.Serialize(rightFeature, FeatureContext.Default.Feature);
            right = JsonSerializer.Deserialize<Text.Feature.Feature>(rightJson);
            right_sg = JsonSerializer.Deserialize(rightJson, FeatureContext.Default.Feature);

            Assert_Are_Equal(left, right); // assert properties doesn't influence comparison and hashcode
            Assert_Are_Equal(left_sg, right_sg);
            Assert_Are_Equal(left, left_sg);

            leftFeature = new Text.Feature.Feature(geometry, null, "abc_abc");
            leftJson = JsonSerializer.Serialize(leftFeature);
            leftJson_sg = JsonSerializer.Serialize(leftFeature, FeatureContext.Default.Feature);
            left = JsonSerializer.Deserialize<Text.Feature.Feature>(leftJson);
            left_sg = JsonSerializer.Deserialize(leftJson, FeatureContext.Default.Feature);

            rightFeature = new Text.Feature.Feature(geometry, null, "xyz_XYZ");
            rightJson = JsonSerializer.Serialize(rightFeature);
            rightJson_sg = JsonSerializer.Serialize(rightFeature, FeatureContext.Default.Feature);
            right = JsonSerializer.Deserialize<Text.Feature.Feature>(rightJson);
            right_sg = JsonSerializer.Deserialize(rightJson, FeatureContext.Default.Feature);

            Assert_Are_Equal(left, right); // assert id's doesn't influence comparison and hashcode
            Assert_Are_Equal(left_sg, right_sg);
            Assert_Are_Equal(left, left_sg);

            leftFeature = new Text.Feature.Feature(geometry, GetPropertiesInRandomOrder(), "abc");
            leftJson = JsonSerializer.Serialize(leftFeature);
            leftJson_sg = JsonSerializer.Serialize(leftFeature, FeatureContext.Default.Feature);
            left = JsonSerializer.Deserialize<Text.Feature.Feature>(leftJson);
            left_sg = JsonSerializer.Deserialize(leftJson, FeatureContext.Default.Feature);

            rightFeature = new Text.Feature.Feature(geometry, GetPropertiesInRandomOrder(), "abc");
            rightJson = JsonSerializer.Serialize(rightFeature);
            rightJson_sg = JsonSerializer.Serialize(rightFeature, FeatureContext.Default.Feature);
            right_sg = JsonSerializer.Deserialize(rightJson, FeatureContext.Default.Feature);

            Assert_Are_Equal(left, right); // assert id's + properties doesn't influence comparison and hashcode
            Assert_Are_Equal(left_sg, right_sg);
            Assert_Are_Equal(left, left_sg);
        }

        [Test]
        public void Feature_Equals_Null_Issue94()
        {
            bool equal1 = true;
            bool equal2 = true;

            var feature = new Text.Feature.Feature(new Point(new Position(12, 123)));
            Assert.DoesNotThrow(() =>
            {
                equal1 = feature.Equals(null);
                equal2 = feature == null;
            });

            Assert.IsFalse(equal1);
            Assert.IsFalse(equal2);
        }

        [Test]
        public void Feature_Null_Instance_Equals_Null_Issue94()
        {
            var equal1 = true;

            Text.Feature.Feature feature = null;
            Assert.DoesNotThrow(() =>
            {
                equal1 = feature != null;
            });

            Assert.IsFalse(equal1);
        }

        [Test]
        public void Feature_Equals_Itself_Issue94()
        {
            bool equal1 = false;
            bool equal2 = false;

            var feature = new Text.Feature.Feature(new Point(new Position(12, 123)));
            Assert.DoesNotThrow(() =>
            {
#pragma warning disable CS1718 // Comparison made to same variable
                equal1 = feature == feature;
#pragma warning restore CS1718 // Comparison made to same variable
                equal2 = feature.Equals(feature);
            });

            Assert.IsTrue(equal1);
            Assert.IsTrue(equal2);
        }

        [Test]
        public void Feature_Equals_Geometry_Null_Issue115()
        {
            bool equal1 = false;
            bool equal2 = false;

            var feature1 = new Text.Feature.Feature(null);
            var feature2 = new Text.Feature.Feature(new Point(new Position(12, 123)));

            Assert.DoesNotThrow(() =>
            {
                equal1 = feature1 == feature2;
                equal2 = feature1.Equals(feature2);
            });

            Assert.IsFalse(equal1);
            Assert.IsFalse(equal2);
        }

        [Test]
        public void Feature_Equals_Other_Geometry_Null_Issue115()
        {
            bool equal1 = false;
            bool equal2 = false;

            var feature1 = new Text.Feature.Feature(new Point(new Position(12, 123)));
            var feature2 = new Text.Feature.Feature(null);

            Assert.DoesNotThrow(() =>
            {
                equal1 = feature1 == feature2;
                equal2 = feature1.Equals(feature2);
            });

            Assert.IsFalse(equal1);
            Assert.IsFalse(equal2);
        }

        [Test]
        public void Feature_Equals_All_Geometry_Null_Issue115()
        {
            bool equal1 = false;
            bool equal2 = false;

            var feature1 = new Text.Feature.Feature(null);
            var feature2 = new Text.Feature.Feature(null);

            Assert.DoesNotThrow(() =>
            {
                equal1 = feature1 == feature2;
                equal2 = feature1.Equals(feature2);
            });

            Assert.IsTrue(equal1);
            Assert.IsTrue(equal2);
        }


        private static IGeometryObject GetGeometry()
        {
            var coordinates = new List<LineString>
            {
                new LineString(new List<IPosition>
                {
                    new Position(52.370725881211314, 4.889259338378906),
                    new Position(52.3711451105601, 4.895267486572266),
                    new Position(52.36931095278263, 4.892091751098633),
                    new Position(52.370725881211314, 4.889259338378906)
                }),
                new LineString(new List<IPosition>
                {
                    new Position(52.370725881211314, 4.989259338378906),
                    new Position(52.3711451105601, 4.995267486572266),
                    new Position(52.36931095278263, 4.992091751098633),
                    new Position(52.370725881211314, 4.989259338378906)
                })
            };
            var multiLine = new MultiLineString(coordinates);
            return multiLine;
        }

        public static IDictionary<string, object> GetPropertiesInRandomOrder()
        {
            var properties = new Dictionary<string, object>()
            {
                { "DateTimeProperty",  DateTime.Now },
                { "IntProperty",  -1 },
                { "EnumProperty",  TestFeatureEnum.Value1 },
                { "BooleanProperty", true },
                { "DoubleProperty",  1.2345d },
                { "StringProperty",  "Hello, GeoJSON !" }
            };
            var randomlyOrdered = new Dictionary<string, object>();
            var randomlyOrderedKeys = properties.Keys.Select(k => Guid.NewGuid() + k).OrderBy(k => k).ToList();
            foreach (var key in randomlyOrderedKeys)
            {
                var theKey = key.Substring(36);
                randomlyOrdered.Add(theKey, properties[theKey]);
            }
            return randomlyOrdered;
        }

        private void Assert_Are_Equal(Text.Feature.Feature left, Text.Feature.Feature right)
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

        private void Assert_Are_Not_Equal(Text.Feature.Feature left, Text.Feature.Feature right)
        {
            Assert.AreNotEqual(left, right);

            Assert.IsFalse(left.Equals(right));
            Assert.IsFalse(right.Equals(left));

            Assert.IsFalse(left == right);
            Assert.IsFalse(right == left);

            Assert.IsTrue(left != right);
            Assert.IsTrue(right != left);

            Assert.AreNotEqual(left.GetHashCode(), right.GetHashCode());
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
        [JsonSerializable(typeof(FeatureCollection))]

        [JsonSerializable(typeof(int))]
        [JsonSerializable(typeof(bool))]
        [JsonSerializable(typeof(DateTime))]
        [JsonSerializable(typeof(TestFeatureEnum))]
        [JsonSerializable(typeof(Text.Feature.Feature))]
        private partial class FeatureContext : JsonSerializerContext
        {
        }
    }
}