namespace Core.Specifications;
public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification(int id)
        : base(product => product.Id == id)

    {
        AddInclude(product => product.ProductType);
        AddInclude(product => product.ProductBrand);
    }
    public ProductsWithTypesAndBrandsSpecification(ProductSpecificationParams productParams)
        : base(ProductCriteria.Expression(productParams))

    {
        AddInclude(product => product.ProductType);
        AddInclude(product => product.ProductBrand);
        ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);
        HandleSortCriteria(productParams.Sort)?.Invoke();
    }

    private Action HandleSortCriteria(string sort)
    {
        if (string.IsNullOrEmpty(sort)) return null;

        return sort switch
        {
            "priceAsc" => () => AddOrderBy(product => product.Price),
            "priceDesc" => () => AddOrderByDescending(product => product.Price),
            _ => () => AddOrderBy(product => product.Name)
        };

    }
}

