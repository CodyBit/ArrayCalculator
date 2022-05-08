using System;
using System.Collections.Generic;
using ArrayCalculator.Api.Models.ErrorModels;

namespace ArrayCalculator.Api.Common
{
    public static class ExtensionHelper
    {
        public static ErrorResponse GetErrorResponse(this Exception exception, string traceId, bool getFullDetails = true)
        {
            return new ErrorResponse(traceId)
            {
                Errors = new List<ErrorMessageDetails>
                {
                    new ErrorMessageDetails
                    {
                        Code = exception.HResult.ToString(),
                        Message = exception.GetErrorMessage(getFullDetails)
                    }
                }
            };
        }

        public static string GetErrorMessage(this Exception exception, bool getFullDetails = true)
        {
            var baseException = exception.GetBaseException();
            var errorMessage = baseException == exception
                ? $"{baseException.GetType()}: {baseException.Message} {baseException.StackTrace}"
                : $"{exception.GetType()}: {exception.Message} ---> " +
                  $"{baseException.GetType()}: {baseException.Message} {baseException.StackTrace} {exception.StackTrace}";

            if (!getFullDetails)
            {
                errorMessage = $"{baseException.GetType()}: {baseException.Message}";
            }

            return errorMessage.Trim();
        }
    }
}
