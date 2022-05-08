using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using ArrayCalculator.Api.Models.ErrorModels;
using ArrayCalculator.Api.Models.SwaggerExampleModels;
using ArrayCalculator.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ArrayCalculator.Api
{
    [ApiController]
    [Route("Api/ArrayCalc")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExample))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public class ArrayCalcController : ControllerBase
    {
        private readonly IProductService productService;

        public ArrayCalcController(IProductService productService) => this.productService = productService;

        /// <summary>
        /// Reverse the product Id array
        /// </summary>
        /// <remarks>Implementation with pure array manipulation without using Array.Reverse() method ad LINQ.</remarks>
        /// <param name="productIds" example="[1,2,3,4,5]">Array of product Ids</param>
        /// <returns>Product Id array</returns>
        [HttpGet]
        [Route("Reverse")]
        [ProducesResponseType(typeof(int[]), StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(int[]))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ReverseResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestErrorResponseExample))]
        public IActionResult Reverse(
            [Required, MinLength(1)][FromQuery] int[] productIds)
        {
            var result = productService.Reverse(productIds);

            return Ok(result);
        }

        /// <summary>
        /// Delete item from product Id array
        /// </summary>
        /// <remarks>Implementation with pure array manipulation without using RemoveAt() method ad LINQ.</remarks>
        /// <param name="productIds" example="[1,2,3,4,5]">Array of product Ids</param>
        /// <param name="position" example="3">Position of product Id List</param>
        /// <returns>Product Id array</returns>
        [HttpGet]
        [Route("DeletePart")]
        [ProducesResponseType(typeof(int[]), StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(int[]))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeletePartResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestErrorResponseExample))]
        public IActionResult DeletePart(
            [Required, MinLength(1)][FromQuery] int[] productIds,
            [Range(1, int.MaxValue)]int? position = null)
        {
            var result = productService.DeletePart(productIds, position);

            return Ok(result);
        }
    }
}
