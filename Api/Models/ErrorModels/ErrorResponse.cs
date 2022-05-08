using System.Collections.Generic;

namespace ArrayCalculator.Api.Models.ErrorModels
{
    public class ErrorResponse
    {
        private readonly string traceIdValue;

        public ErrorResponse(string traceid = null) => this.traceIdValue = traceid;

        public List<ErrorMessageDetails> Errors { get; set; }

        public string TraceId => this.traceIdValue;
    }
}
