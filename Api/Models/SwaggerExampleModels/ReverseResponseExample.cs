using Swashbuckle.AspNetCore.Filters;

namespace ArrayCalculator.Api.Models.SwaggerExampleModels
{
    public class ReverseResponseExample : IExamplesProvider<int[]>
    {
        public int[] GetExamples() => new int[] { 5, 4, 3, 2, 1 };
    }
}
