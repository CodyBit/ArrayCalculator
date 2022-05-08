using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ArrayCalculator.Api.Common;
using ArrayCalculator.Api.Models.ErrorModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArrayCalculator.Api.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var errorResponse = context.Exception.GetErrorResponse(context.HttpContext?.TraceIdentifier);
            if (context.Exception is OperationCanceledException || context.Exception is TaskCanceledException)
            {
                var apiErrorResponse = HandleException(context.HttpContext, context.Exception, HttpStatusCode.RequestTimeout);
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.RequestTimeout,
                    Value = apiErrorResponse
                };
            }
            else
            {
                var apiErrorResponse = HandleException(context.HttpContext, context.Exception, HttpStatusCode.InternalServerError);
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Value = apiErrorResponse
                };
            }
        }

        private static ErrorResponse HandleException(HttpContext httpContext, Exception exception, HttpStatusCode httpStatusCode)

        {
            var baseException = exception.GetBaseException();
            string exceptionMessage;
            if (httpStatusCode == HttpStatusCode.RequestTimeout)
            {
                exceptionMessage = "Request got timed out.";
            }
            else if (exception != baseException)
            {
                exceptionMessage = $"{Regex.Replace(exception.Message, "see inner exception for details", string.Empty, RegexOptions.IgnoreCase).Replace(", .", ".")} {baseException.Message}";
            }
            else
            {
                exceptionMessage = $"Exception occurred in {baseException.Source}. {baseException.Message}";
            }

            // Exception can be logged here

            return new ErrorResponse(httpContext?.TraceIdentifier)
            {
                Errors = new List<ErrorMessageDetails>
                {
                    new ErrorMessageDetails
                    {
                        Code = ((int)httpStatusCode).ToString(CultureInfo.InvariantCulture),
                        Message = exceptionMessage
                    }
                }
            };
        }
    }
}