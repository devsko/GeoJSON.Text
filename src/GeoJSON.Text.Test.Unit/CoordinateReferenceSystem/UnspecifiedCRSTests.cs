using GeoJSON.Text.CoordinateReferenceSystem;
using GeoJSON.Text.Feature;
using NUnit.Framework;
using System.Text.Json;

namespace GeoJSON.Text.Tests.CoordinateReferenceSystem
{
    [TestFixture]
    public class UnspecifiedCRSTests : TestBase
    {
        [Test]
        public void Has_Correct_Type()
        {
            var crs = new UnspecifiedCRS();

            Assert.AreEqual(CRSType.Unspecified, crs.Type);
        }

        [Test]
        public void Can_Serialize_To_Null()
        {
            var collection = new FeatureCollection { CRS = new UnspecifiedCRS() };
            var expectedJson = "{\"type\":\"FeatureCollection\",\"crs\":null,\"features\":[] }";
            var actualJson = JsonSerializer.Serialize(collection);
            var actualJson_sg = JsonSerializer.Serialize(collection, GeoJSONSerializerContext.Default.FeatureCollection);
            
            JsonAssert.AreEqual(expectedJson, actualJson);
            JsonAssert.AreEqual(expectedJson, actualJson_sg);
        }

        [Test]
        public void Can_Deserialize_From_Null()
        {
            var json = "{\"type\":\"FeatureCollection\",\"crs\":null,\"features\":[] }";
            var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json);
            var featureCollection_sg = JsonSerializer.Deserialize(json, GeoJSONSerializerContext.Default.FeatureCollection);

            Assert.IsInstanceOf<UnspecifiedCRS>(featureCollection.CRS);
        }

        [Test]
        public void Equals_GetHashCode_Contract()
        {
            var left = new UnspecifiedCRS();
            var right = new UnspecifiedCRS();

            Assert.AreEqual(left, right);

            Assert.IsTrue(left == right);
            Assert.IsTrue(right == left);

            Assert.IsTrue(left.Equals(right));
            Assert.IsTrue(right.Equals(left));

            Assert.IsTrue(left.Equals(left));
            Assert.IsTrue(right.Equals(right));

            Assert.AreEqual(left.GetHashCode(), right.GetHashCode());
        }
    }
}