using System;
using System.Collections.Generic;
using System.Globalization;
using ArrayCalculator.Api.Models.ErrorModels;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;

namespace ArrayCalculator.Api.Models.SwaggerExampleModels
{
    public class InternalServerErrorResponseExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new(Guid.NewGuid().ToString())
        {
            Errors = new List<ErrorMessageDetails>
                {
                    new ErrorMessageDetails
                    {
                        Code = StatusCodes.Status500InternalServerError.ToString(CultureInfo.InvariantCulture),
                        Message = "Exception occurred in Api."
                    }
                }
        };
    }
}
