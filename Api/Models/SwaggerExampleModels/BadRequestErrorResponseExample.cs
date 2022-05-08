using System;
using System.Collections.Generic;
using System.Globalization;
using ArrayCalculator.Api.Models.ErrorModels;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;

namespace ArrayCalculator.Api.Models.SwaggerExampleModels
{
    public class BadRequestErrorResponseExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new (Guid.NewGuid().ToString())
        {
            Errors = new List<ErrorMessageDetails>
                {
                    new ErrorMessageDetails
                    {
                        Code = StatusCodes.Status400BadRequest.ToString(CultureInfo.InvariantCulture),
                        Message = "productIds: The value 'StringTestValue' is not valid."
                    }
                }
        };
    }
}
