namespace ArrayCalculator.Api.Services
{
    public interface IProductService
    {
        public int[] Reverse(int[] productIds);

        public int[] DeletePart(int[] productIds, int? position);
    }

    public class ProductService : IProductService
    {
        public int[] Reverse(int[] productIds)
        {
            var productIdLength = productIds?.Length ?? 0;
            if (productIdLength > 0)
            {
                // Ignoring array item swapping to unchange the original request
                var newProductIds = new int[productIdLength];
                for (int i = 0; i < productIdLength; i++)
                {
                    newProductIds[i] = productIds[productIdLength - 1 - i];
                }

                return newProductIds;
            }

            return null;
        }

        public int[] DeletePart(int[] productIds, int? position)
        {
            var productIdLength = productIds?.Length ?? 0; ;
            if (productIdLength > 0 && position.HasValue)
            {
                var newProductIds = new int[productIdLength - 1];
                var j = -1;
                for (int i = 0; i < productIdLength; i++)
                {
                    // Skipping the postion
                    if (position != i + 1)
                    {
                        newProductIds[++j] = productIds[i];
                    }
                }

                return newProductIds;
            }

            return null;
        }
    }
}
