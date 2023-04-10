namespace Core.Common.ConditionExpression;
public static class ProductCriteria
{
    public static Expression<Func<Product, bool>> Expression(ProductSpecificationParams productParams)
    {
        Expression<Func<Product, bool>> isProductMeetsCriteria = product =>
        (string.IsNullOrEmpty(productParams.Search) || product.Name
                                                              .ToLower()
                                                              .Contains(productParams.Search)) &&
        ((!productParams.BrandId.HasValue) || product.ProductBrandId == productParams.BrandId) &&
        ((!productParams.TypeId.HasValue) || product.ProductTypeId == productParams.TypeId);
        return isProductMeetsCriteria;
    }
}

