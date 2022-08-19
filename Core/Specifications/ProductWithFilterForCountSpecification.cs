namespace Core.Specifications;
public class ProductWithFilterForCountSpecification : BaseSpecification<Product>
{
    public ProductWithFilterForCountSpecification(ProductSpecificationParams productParams)
        : base(product =>
            (string.IsNullOrEmpty(productParams.Search) || product.Name.ToLower()
                                                                  .Contains(productParams.Search)) &&
            ((!productParams.BrandId.HasValue) || product.ProductBrandId == productParams.BrandId) &&
            ((!productParams.TypeId.HasValue) || product.ProductTypeId == productParams.TypeId))
    {

    }
}

