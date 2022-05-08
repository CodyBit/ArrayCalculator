using System;
using System.Linq;
using ArrayCalculator.Api.Services;
using NUnit.Framework;

namespace UnitTests.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        private ProductService service;

        [SetUp]
        public void Setup()
        {
            service = new ProductService();
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5 })]
        public void ReverseTestWithValidMultipleProductIds(int[] productIds)
        {
            var expectedResponse = new int[] { 5, 4 , 3 , 2, 1 };
            var actualResponse = service.Reverse(productIds);

            Assert.IsNotNull(actualResponse);
            Assert.AreEqual(actualResponse.Length, expectedResponse.Length);
            CollectionAssert.AreEqual(actualResponse, expectedResponse);
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5 })]
        public void ReverseTestWithDeafultFunction(int[] productIds)
        {
            var expectedResponse = productIds.Reverse().ToArray();
            var actualResponse = service.Reverse(productIds);

            Assert.IsNotNull(actualResponse);
            Assert.AreEqual(expectedResponse.Length, actualResponse.Length);            
            CollectionAssert.AreEqual(actualResponse, expectedResponse);
        }

        [TestCase(new int[] { 1 })]
        public void ReverseTestWithValidSingleProductIds(int[] productIds)
        {
            var response = service.Reverse(productIds);

            Assert.IsNotNull(response);
            CollectionAssert.AreEqual(productIds, response);
        }

        [TestCase(null)]
        [TestCase(new int[] { })]
        public void ReverseTestWithEmptyProductId(int[] productIds)
        {
            var response = service.Reverse(productIds);

            Assert.IsNull(response);
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5 }, 3)]
        public void DeletePartTestWithValidPositionAndMultipleProductIds(int[] productIds, int position)
        {
            var expectedResponse = new int[] { 1, 2, 4, 5 };
            var actualResponse = service.DeletePart(productIds, position);

            Assert.IsNotNull(actualResponse);
            Assert.AreEqual(actualResponse.Length, productIds.Length - 1);            
            CollectionAssert.AreEqual(actualResponse, expectedResponse);            
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5 }, 3)]
        public void DeletePartTestWithDeafultFunction(int[] productIds, int position)
        {
            var index = position - 1;
            var defaultResponse = productIds.ToList();
            defaultResponse.RemoveAt(index);

            var expectedResponse = defaultResponse.ToArray();
            var actualResponse = service.DeletePart(productIds, position);

            Assert.IsNotNull(actualResponse);
            CollectionAssert.AreEqual(actualResponse, expectedResponse);
        }

        [TestCase(new int[] { 1 }, 1)]
        public void DeletePartTestWithValidPositionAndSingleProductIds(int[] productIds, int position)
        {
            var response = service.DeletePart(productIds, position);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(productIds, response);
            Assert.AreEqual(response.Length, productIds.Length - 1);
            CollectionAssert.IsEmpty(response);
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5 }, null)]
        [TestCase(null, 3)]
        [TestCase(new int[] { }, 3)]
        public void DeletePartTestWithEmptyInputs(int[] productIds, int? position)
        {
            var response = service.DeletePart(productIds, position);

            Assert.IsNull(response);
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5 }, -1)]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, 0)]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, 6)]
        public void DeletePartTestThroughExceptionForInvalidPosition(int[] productIds, int? position)
        {
            Assert.Throws<IndexOutOfRangeException>(() => service.DeletePart(productIds, position));
        }
    }
}