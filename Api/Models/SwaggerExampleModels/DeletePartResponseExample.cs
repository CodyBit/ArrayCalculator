using Swashbuckle.AspNetCore.Filters;

namespace ArrayCalculator.Api.Models.SwaggerExampleModels
{
    public class DeletePartResponseExample : IExamplesProvider<int[]>
    {
        public int[] GetExamples() => new int[] { 1, 2, 4, 5 };
    }
}
