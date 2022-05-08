using System;
using System.Collections.Generic;
using ArrayCalculator.Api;
using ArrayCalculator.Api.Filters;
using ArrayCalculator.Api.Models.ErrorModels;
using ArrayCalculator.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace UnitTests.Controllers
{
    [TestFixture]
    public class ArrayCalcControllerTests
    {
        private ArrayCalcController controller;
        private IProductService productService;
        private IHttpContextAccessor httpContextAccessor;
        private ApiExceptionFilter apiExceptionFilter;
        private ExceptionContext exceptionContext;

        [SetUp]
        public void Setup()
        {
            productService = Substitute.For<IProductService>();
            httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext = new DefaultHttpContext();
            controller = new ArrayCalcController(productService);
            exceptionContext = new ExceptionContext(new ActionContext(httpContextAccessor.HttpContext, new RouteData(), Substitute.For<ActionDescriptor>()), new List<IFilterMetadata>());
            apiExceptionFilter = new ApiExceptionFilter();
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5 })]
        public void ReverseTestWithMultipleProductIds(int[] productIds)
        {
            var expectedResult = new int[] { 5, 4, 3, 2, 1 };
            productService.Reverse(productIds)
                .Returns(expectedResult);

            var actualResponse = controller.Reverse(productIds);

            Assert.IsNotNull(actualResponse);
            Assert.IsInstanceOf(typeof(ObjectResult), actualResponse);
            var httpResponse = actualResponse as ObjectResult;
            Assert.IsNotNull(httpResponse);
            Assert.AreEqual(StatusCodes.Status200OK, httpResponse.StatusCode);
            Assert.IsInstanceOf(typeof(int[]), httpResponse.Value);
            var httpResposeData = httpResponse.Value as int[];
            Assert.IsNotNull(httpResposeData);
            CollectionAssert.AreEqual(httpResposeData, expectedResult);
        }

        [TestCase(new int[] { 1 })]
        public void ReverseTestWithSingleProductIds(int[] productIds)
        {
            var expectedResult = productIds;
            productService.Reverse(productIds)
                .Returns(expectedResult);

            var actualResponse = controller.Reverse(productIds);

            Assert.IsNotNull(actualResponse);
            Assert.IsInstanceOf(typeof(ObjectResult), actualResponse);
            var httpResponse = actualResponse as ObjectResult;
            Assert.IsNotNull(httpResponse);
            Assert.AreEqual(StatusCodes.Status200OK, httpResponse.StatusCode);
            Assert.IsInstanceOf(typeof(int[]), httpResponse.Value);
            var httpResposeData = httpResponse.Value as int[];
            Assert.IsNotNull(httpResposeData);
            CollectionAssert.AreEqual(httpResposeData, expectedResult);
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5 })]
        public void ReverseShouldThrowInternalServerErrorForServiceException(int[] productIds)
        {
            productService.Reverse(productIds)
                .ThrowsForAnyArgs(new AggregateException());

            try
            {
                var actualResponse = controller.Reverse(productIds);
            }
            catch (Exception ex)
            {
                exceptionContext.Exception = ex;
                apiExceptionFilter.OnException(exceptionContext);
            }

            Assert.IsNotNull(exceptionContext.Result);
            Assert.IsInstanceOf(typeof(ObjectResult), exceptionContext.Result);
            var httpResponse = exceptionContext.Result as ObjectResult;
            Assert.IsNotNull(httpResponse);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, httpResponse.StatusCode);
            Assert.IsInstanceOf(typeof(ErrorResponse), httpResponse.Value);
            var httpResposeData = httpResponse.Value as ErrorResponse;
            Assert.IsNotNull(httpResposeData);
            Assert.AreEqual(httpResposeData.TraceId, httpContextAccessor.HttpContext.TraceIdentifier);
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5 }, 3)]
        public void DeletePartTestWithMultipleProductIdsAndPosition(int[] productIds, int? position)
        {
            var expectedResult = new int[] { 1, 2, 4, 5 };
            if (!position.HasValue)
            {
                expectedResult = null;
            }

            productService.DeletePart(productIds, position)
                .Returns(expectedResult);

            var actualResponse = controller.DeletePart(productIds, position);

            Assert.IsNotNull(actualResponse);
            Assert.IsInstanceOf(typeof(ObjectResult), actualResponse);
            var httpResponse = actualResponse as ObjectResult;
            Assert.IsNotNull(httpResponse);
            Assert.AreEqual(StatusCodes.Status200OK, httpResponse.StatusCode);
            Assert.IsInstanceOf(typeof(int[]), httpResponse.Value);
            var httpResposeData = httpResponse.Value as int[];
            Assert.IsNotNull(httpResposeData);
            CollectionAssert.AreEqual(httpResposeData, expectedResult);
        }

        [TestCase(new int[] { 1 }, 1)]
        public void DeletePartTestWithSingleProductIdAndPosition(int[] productIds, int? position)
        {
            var expectedResult = productIds;
            if (position.HasValue)
            {
                expectedResult = Array.Empty<int>();
            }

            productService.DeletePart(productIds, position)
                .ReturnsForAnyArgs(expectedResult);

            var actualResponse = controller.DeletePart(productIds, position);

            Assert.IsNotNull(actualResponse);
            Assert.IsInstanceOf(typeof(ObjectResult), actualResponse);
            var httpResponse = actualResponse as ObjectResult;
            Assert.IsNotNull(httpResponse);
            Assert.AreEqual(StatusCodes.Status200OK, httpResponse.StatusCode);
            Assert.IsInstanceOf(typeof(int[]), httpResponse.Value);
            var httpResposeData = httpResponse.Value as int[];
            Assert.IsNotNull(httpResposeData);
            CollectionAssert.AreEqual(httpResposeData, expectedResult);
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5 }, null)]
        [TestCase(new int[] { 1 }, null)]
        public void DeletePartTestWithoutPosition(int[] productIds, int? position)
        {
            int[] expectedResult = null;

            productService.DeletePart(productIds, position)
                .Returns(expectedResult);

            var actualResponse = controller.DeletePart(productIds, position);

            Assert.IsNotNull(actualResponse);
            Assert.IsInstanceOf(typeof(ObjectResult), actualResponse);
            var httpResponse = actualResponse as ObjectResult;
            Assert.IsNotNull(httpResponse);
            Assert.AreEqual(StatusCodes.Status200OK, httpResponse.StatusCode);
            Assert.IsNull(httpResponse.Value);
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5 }, 3)]
        public void DeletePartShouldThrowInternalServerErrorForServiceException(int[] productIds, int position)
        {
            productService.DeletePart(productIds, position)
                .ThrowsForAnyArgs(new AggregateException());

            try
            {
                var actualResponse = controller.DeletePart(productIds, position);
            }
            catch (Exception ex)
            {
                exceptionContext.Exception = ex;
                apiExceptionFilter.OnException(exceptionContext);
            }

            Assert.IsNotNull(exceptionContext.Result);
            Assert.IsInstanceOf(typeof(ObjectResult), exceptionContext.Result);
            var httpResponse = exceptionContext.Result as ObjectResult;
            Assert.IsNotNull(httpResponse);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, httpResponse.StatusCode);
            Assert.IsInstanceOf(typeof(ErrorResponse), httpResponse.Value);
            var httpResposeData = httpResponse.Value as ErrorResponse;
            Assert.IsNotNull(httpResposeData);
            Assert.AreEqual(httpResposeData.TraceId, httpContextAccessor.HttpContext.TraceIdentifier);
        }
    }
}
